using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponSettings settings;

    public Weapon(WeaponSettings settings)
    {
        this.settings = settings;
    }

    public abstract void Shoot();
}
