using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private Transform target;

    public Transform Target { get => target; set => target = value; }

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private float bulletDamage = 40.0f;

    [SerializeField]
    private float shootPower = 1.0f;

    [SerializeField]
    float shootDelay = 1.0f;

    //это1
    //[SerializeField]
    //GameObject explosionPrefab;

    private void Awake()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        InvokeRepeating("Shoot", 0.0f, shootDelay);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTargetVisibility();
        if (target != null)
        {
            _navMeshAgent.SetDestination(target.position);
        }

        //это1
        //Explosion.Create(transform.position, explosionPrefab);
    }


    private bool seeTarget;
    void CheckTargetVisibility()
    {
        if (Target == null)
        {
            seeTarget = false;
            return;
        }

        Vector3 targetDirection = Target.position - gun.transform.position;

        targetDirection.Normalize();

        Ray ray = new Ray(gun.transform.position, targetDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == Target)
            {
                seeTarget = true;
                return;
            }
        }
        seeTarget = false;
    }


    void Shoot()
    {
        if (seeTarget)
        {
            GameObject newRocket = Instantiate(bulletPrefab, gun.position, gun.rotation) as GameObject;
            newRocket.GetComponentInChildren<Rigidbody>().velocity = gun.forward * shootPower;
            Destroy(newRocket, 5);

            newRocket.GetComponent<Damager>().Damage = bulletDamage;
        }
    }

    private void Destroyed()
    {
        if (Random.Range(0, 100) < 50)
        {
            HealthBonus.Create(transform.position);
        }
    }
}
