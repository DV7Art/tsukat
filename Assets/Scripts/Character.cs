using UnityEngine;
//using Zenject;

public class Character : MonoBehaviour
{
    //[Inject(Id = "Bullet")] private IWeapon bullet;
    //[Inject(Id = "Rocket")] private IWeapon rocket;

    [SerializeField] private WeaponSettings bulletSettings; 
    [SerializeField] private WeaponSettings rocketSettings; 
    private Bullet bulletWeapon;
    private Rocket rocketWeapon;
    private float fireRate = 0.5f;
    private float lastFireTime;

    void Start()
    {
        // Создание экземпляров оружия
        bulletWeapon = new Bullet(bulletSettings);
        rocketWeapon = new Rocket(rocketSettings);
    }

    void Update()
    {
        Shoot();
    }

    protected void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastFireTime + fireRate)
        {
            lastFireTime = Time.time;
            bulletWeapon.Shoot();
        }

        if (Input.GetButtonDown("Fire2") && Time.time > lastFireTime + fireRate)
        {
            lastFireTime = Time.time;
            rocketWeapon.Shoot();
        }
    }
}
