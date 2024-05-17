using UnityEngine;
//using static Zenject.CheatSheet;


internal class Rocket : Weapon
{
    public Rocket(WeaponSettings settings) : base(settings)
    {
    }

    public override void Shoot()
    {
        GameObject newRocket = Instantiate(settings.Prefab, settings.GunTransform.position, settings.GunTransform.rotation) as GameObject;
        newRocket.GetComponentInChildren<Rigidbody>().velocity = settings.GunTransform.forward * settings.ShootPower;
        Destroy(newRocket, 1);

        newRocket.GetComponent<Damager>().Damage = settings.Damage;

        SoundManager.Instance.PlaySound(settings.ShootSound);
    }
}
