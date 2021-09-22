using UnityEngine;

/*
 * This script manage Enemy's movement between waypoints
 * 
 * WIP: adding a behaviour allowing to manage enemies animation's root motion instead of tranform.translate
 * Probably adding pathfinding support
 *
 */

[RequireComponent(typeof(Enemy))]
//Specify to Unity that this script needs the Enemy component
//Allowing to Unity to automatically add Enemy script if needed 

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private bool isRootMotion = false;
    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
    }


    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        if (!isRootMotion)
        {
            transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);
            enemy.speed = enemy.startSpeed;
        }
        
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
        enemy.transform.LookAt(target);
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        else
        {
            waypointIndex++;
            target = Waypoints.points[waypointIndex];
        }
        enemy.transform.LookAt(target);
        
    }

    private void EndPath()
    {
        PlayerStats.lives--;
        WaveSpawner._enemiesAlive--;
        Destroy(gameObject);
    }
}
