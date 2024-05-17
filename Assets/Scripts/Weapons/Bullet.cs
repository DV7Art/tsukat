using UnityEngine;
using static Zenject.CheatSheet;

public class Bullet : Weapon
{
    public Bullet(WeaponSettings settings) : base(settings)
    {
    }

    public override void Shoot()
    {
        GameObject newBullet = Instantiate(settings.Prefab, settings.GunTransform.position, settings.GunTransform.rotation) as GameObject;
        newBullet.GetComponent<Rigidbody>().AddForce(settings.GunTransform.forward * settings.ShootPower, ForceMode.Impulse);
        Destroy(newBullet, 1);

        newBullet.GetComponent<Damager>().Damage = settings.Damage;

        SoundManager.Instance.PlaySound(settings.ShootSound);
    }
}

