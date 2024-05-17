using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField]
    private AudioClip healthBonusSound;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HealthBonus - 1");
        Destructable otherHealth = other.gameObject.GetComponent<Destructable>();

        if (otherHealth != null)
        {
            otherHealth.HitPointsCurrent = otherHealth.HitPoints;
            SoundManager.Instance.PlaySound(healthBonusSound);
            Destroy(gameObject);
        }
    }
    public static void Create(Vector3 position) =>
        Instantiate(Resources.Load("Prefabs/HealthBonus"), position, Quaternion.identity);

}
