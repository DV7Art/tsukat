using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private Transform target;
    public Transform[] wayPoint;
    private int index = 0;

    void Start()
    {
        MoveToWayPoint();
    }

    void MoveToWayPoint() => target = wayPoint[index];

    void FixedUpdate()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;


        if ((transform.position - playerTransform.position).magnitude < 2)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            navMeshAgent.SetDestination(target.position);

            if ((transform.position - target.position).magnitude < 3f)
            {
                index++;
                if (index >= wayPoint.Length)
                {
                    index = 0;
                    MoveToWayPoint();
                }
                else
                {
                    MoveToWayPoint();
                }
            }
        }
    }
}
