using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("General")]
    public float range = 15f;
    private Transform target;
    private Enemy targetedEnemyComponent;


    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f; //tir/s
    private float fireCtdw = 0f; //cooldown


    [Header("Use Laser")]
    public bool useLaser;
    public int damageOverTime = 30; //Degat par seconde
    public float slowRatio = 0.5f;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    private AudioManager sounds;


    [Header("Unity setup fields")]
    public Transform firePoint;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    [SerializeField]
    private float rotSpeed = 9f;


    void Start()
    {
        if (useLaser)
        {

            sounds = FindObjectOfType<AudioManager>();

            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }
        //Permet de lancer la fonction UpdateTarget toutes les 0.5s à partir de 0s
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }


    //Est appelée de manière fixe toutes les 0.5s
    void UpdateTarget()
    {
        //Un array contenant tous les GO avec le tag Enemy dans la scène
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        //float pour stocker la distance d'enemis la plus proche de la tourelle
        float shortestDistance = Mathf.Infinity;
        //GO pour stocker le GO enemy le plus proche
        GameObject nearestEnemy = null;

        //On parcourt l'array des Enemy de la scène pour trouver le plus proche
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //S'il y un Enemy le + proche et qu'il est dans la portée de la tourelle, on stock sa transform comme target
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetedEnemyComponent = target.GetComponent<Enemy>();
        }
        //Sinon target null
        else
        {
            target = null;
        }
    }

    void Update()
    {
        //Si target est null, on quitte l'update
        if (target == null)
        {
            if (useLaser && lineRenderer.enabled)
            {
                sounds.Stop("LaserBeam");
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }

            return;
        }

        LockOnTarget();

        if (fireCtdw <= 0f)
        {
            if (useLaser)
            {
                sounds.Play("LaserBeam");
                Laser();
            }
            else
            {
                //On tire
                PooledShoot();

                fireCtdw = 1 / fireRate;
                //firerate correspond à nb coup/s; donc le cooldown est l'inverse
                //aka fireRate=2 donc fireCtdw=1/2=.5s

            }
        }

        fireCtdw -= Time.deltaTime;
    }

    void LockOnTarget()
    {

        //Si target n'est pas nul, aka un enemy est proche et dans la range
        //On calcule le vecteur entre la tourelle et la target
        Vector3 dir = target.position - transform.position;

        //Quaternion correspond à la class pour les rotations
        //Quaternion LookRotation(Vector3 forward, Vector3 upwards = Vector3.up);
        //permet de calculer l'angle de la tourelle en fct de la direction vers l'enemi
        Quaternion lookAtEnemy = Quaternion.LookRotation(dir);

        //Lerp passe de la rotation actuelle de la tourelle à lookAtEnemy avec un fondu calculé en fct du temps et non des frames
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookAtEnemy, Time.deltaTime * rotSpeed).eulerAngles;

        //Applique la rotation calculée
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        //On instantie la boullette a la position firePoint
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);


        //on récupère son composant
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        //Si on a qqe chose de non nul on lui indique quelle target cibler
        if (bullet != null)
        {
            bullet.Seek(target);
            bullet.SetOrigin(transform.position);
        }
    }
    void PooledShoot()
    {
        //On instantie la boullette a la position firePoint
        //GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        GameObject bulletGO = ObjectPool.SharedInstance.GetPooledObjects();
        if (bulletGO != null)
        {
            bulletGO.transform.position = firePoint.position;
            bulletGO.transform.rotation = firePoint.rotation;
            bulletGO.SetActive(true);
        }

        //on récupère son composant
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        //Si on a qqe chose de non nul on lui indique quelle target cibler
        if (bullet != null)
        {
            bullet.Seek(target);
            bullet.SetOrigin(transform.position);
        }
    }

    void Laser()
    {
        targetedEnemyComponent.TakeDamage((float)damageOverTime * Time.deltaTime);
        //Le slow ne se stackera pas en fct du nbre de tourelles sur un enemi
        targetedEnemyComponent.Slow(slowRatio);


        if (lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        impactEffect.transform.position = target.position + dir.normalized * 1.5f; ;
    }

    private void OnDrawGizmosSelected()
    {
        //Dessine une sphère rouge de taille range autour de la tourelle dans l'éditeur
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
