using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion 
{
    public static void Create(Vector3 position, GameObject prefab)
    {
        GameObject newExplosion = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity) as GameObject;
        ParticleSystem particleSystem = newExplosion.GetComponent<ParticleSystem>();

        // Используйте main.startLifetime вместо startLifetime
        MonoBehaviour.Destroy(newExplosion, particleSystem.main.startLifetime.constant);
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
