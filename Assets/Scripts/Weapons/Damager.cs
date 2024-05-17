using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    float damage;

    [Header("SFX")]
    [SerializeField]
    private AudioClip shootSound;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    float radius;


    public float Damage { get => damage; set => damage = value; }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (explosionPrefab != null)
        {
            Explosion.Create(transform.position, explosionPrefab);
        }
        if (radius > 0)
        {

            CauseExplosionDamage();
        }
        else
        {
            Destructable target = collision.gameObject.GetComponent<Destructable>();
            if (target != null)
            {
                target.Hit(Damage);
            }

        }

        ParticleSystem trail = gameObject.GetComponentInChildren<ParticleSystem>();
        if (trail != null)
        {
            Destroy(trail.gameObject, trail.main.startLifetime.constant);

            trail.Stop();

            trail.transform.SetParent(null);
        }
        Destroy(gameObject);

        SoundManager.Instance.PlaySound(shootSound);
    }

    void CauseExplosionDamage()
    {
        Collider[] explposionVictims = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < explposionVictims.Length; i++)
        {
            Vector3 vectorToVictim = explposionVictims[i].transform.position - transform.position;
            float decay = 1 - (vectorToVictim.magnitude / radius);

            Destructable currentVictim = explposionVictims[i].gameObject.GetComponent<Destructable>();

            if (currentVictim != null)
            {
                currentVictim.Hit(damage * decay);
            }

            Rigidbody victimRigidbody = explposionVictims[i].gameObject.GetComponent<Rigidbody>();
            if (victimRigidbody != null)
            {
                victimRigidbody.AddForce(vectorToVictim.normalized * decay * 1000);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
