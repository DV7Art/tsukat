using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private float hitPoints = 100f;

    private float hitPointsCurrent;

    public float HitPointsCurrent { get => hitPointsCurrent; set => hitPointsCurrent = value; }
    public float HitPoints { get => hitPoints; set => hitPoints = value; }

    void Start()
    {
        hitPointsCurrent = hitPoints;
    }

    public void Hit(float damage)
    {
        HitPointsCurrent -= damage;

        if (HitPointsCurrent <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        BroadcastMessage("Destroyed");
        Destroy(gameObject);

    }
}
