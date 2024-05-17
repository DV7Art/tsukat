using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    public GameObject rocketPrefab;
    public GameObject bulletPrefab;

    public override void InstallBindings()
    {
        //Container.Bind<IWeapon>().WithId("Rocket").FromComponentInNewPrefab(rocketPrefab).AsTransient();
        //Container.Bind<IWeapon>().WithId("Bullet").FromComponentInNewPrefab(bulletPrefab).AsTransient();
    }
}
