using UnityEngine;

public class Character2 : MonoBehaviour
{
    [Header("Guns_Prefab")]
    [SerializeField]
    private Transform RGun;
    [SerializeField]
    private Transform LGun;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject rocketPrefab;

    [Header("Guns Settings")]
    [SerializeField]
    private float bulletDamage = 40.0f;

    [SerializeField]
    private float rocketDamage = 40.0f;
    [SerializeField]
    private float shootPower = 1.0f;
    [SerializeField]
    private float fireRate = 0.5f; // Время между выстрелами в секундах


    [Header("SFX")]
    [SerializeField]
    private AudioClip bulletShootSound;
    [SerializeField]
    private AudioClip rocketShootSound;

    private float lastFireTime; // Время последнего выстрела


    void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastFireTime + fireRate)
        {
            lastFireTime = Time.time; // Обновляем время последнего выстрела

            ShootBullet();
        }

        if (Input.GetButtonDown("Fire2") && Time.time > lastFireTime + fireRate)
        {
            lastFireTime = Time.time; // Обновляем время последнего выстрела

            ShootRocket();
        }

    }

    public void ShootBullet()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bulletPrefab, LGun.position, LGun.rotation) as GameObject;
            newBullet.GetComponent<Rigidbody>().AddForce(LGun.forward * shootPower, ForceMode.Impulse);
            Destroy(newBullet, 1);

            newBullet.GetComponent<Damager>().Damage = bulletDamage;

            SoundManager.Instance.PlaySound(bulletShootSound);
        }
    }
    public void ShootRocket()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject newRocket = Instantiate(rocketPrefab, RGun.position, RGun.rotation) as GameObject;
            newRocket.GetComponentInChildren<Rigidbody>().velocity = RGun.forward * 8;
            Destroy(newRocket, 1);

            newRocket.GetComponent<Damager>().Damage = rocketDamage;

            SoundManager.Instance.PlaySound(rocketShootSound);
        }
    }
}
