using UnityEngine;

public class Bullet : MonoBehaviour
{

    private AudioManager sounds;

    private Transform target;
    public GameObject impactEffect;

    public int damage = 50;

    public float speed = 50f;
    public float explosionRadius = 0f;

    private Vector3 creationPoint;

    private void Awake()
    {
        sounds = FindObjectOfType<AudioManager>();

        //Debug.Log("firepoint bullet: "+this.transform.position);
        //Debug.Log("creation point" + creationPoint);
        if (explosionRadius > 0)
        {
            sounds.Play("MissileShot");
        }
        else
        {
            //Add a pitch randomness between 0.5 and 2 to turret shots sound
            sounds.Play("TurretShot",.5f,2f);
        }
    }

    //Permet de d�finir la target a cibler, communiqu�e par le script turret
    public void Seek(Transform _target)
    {
        target = _target;
    }

    //Permet de d�finir la target a cibler, communiqu�e par le script turret
    public void SetOrigin(Vector3 _target)
    {
        creationPoint = _target;
    }
    
    void Update()
    {
        //Si la target n'existe pas/plus, on supprime le GO
        //par ex la target s'est fait d�truire par une autre boullette
        if (target == null)
        {
            if (explosionRadius > 0)
            {
                Explode();
            }
            Destroy(gameObject);
            return;
        }

        //Si la target existe, on calcule la direction
        Vector3 dir = target.position - transform.position;
        //On calcule la distance qui va �tre pparcourue d'ici la prochaine frame
        float distanceThisFrame = speed * Time.deltaTime;

        //Si on atteint la cible d'ici la prochaine frame alors on hit et on quitte la fct
        if (dir.magnitude <= distanceThisFrame)
        {
            hit();
            return;
        }

        //Si a la prochaine frame on a pas atteint la cible alors on se d�place
        //Normalize permet de pas ralentir la vitesse en se rapprochant
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }

    private void hit()
    {
        Vector3 dir = creationPoint - target.position;
        //Debug.Log("firepoint: " + creationPoint+ "target: " + target.position);
        //Debug.Log(dir);
        //Debug.Log("rot: "+Quaternion.LookRotation(dir));
        //Quand on hit on fait spawn des particules
        GameObject effectIns=(GameObject) Instantiate(impactEffect, transform.position, Quaternion.LookRotation(dir));
        //On planifie la destruction de ces m�mes particules dans 3s longueur particle missile
        Destroy(effectIns, 3f);

        if (explosionRadius > 0)
        {
            Explode();
        }
        else
        {
            //effectIns.transform.position = target.position + dir.normalized * 1.5f;
            Damage(target);
        }

        //Destroy(target.gameObject);

        //on d�truit la boulette
        Destroy(gameObject);
    }

    void Explode()
    {
        if (sounds != null)
        {
            sounds.Play("MissileExplode");
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e == null)
        {
            Debug.LogError("pas de script ennemi trouv�");
        }
        else
        {
            e.TakeDamage(damage);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
