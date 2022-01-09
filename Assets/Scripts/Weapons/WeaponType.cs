using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    Rilfe,
    AssaultRife,
    Sniper,
    Schotgun,
    Pistol,
    SubMachineGun,
    MachineGun,
    Knife
}

public enum EAmmoType
{
    FullMetalJacket,        // Vollmantelgeschoss
    ArmourPricing,          // Hartkerngeschoss
    Subsonic,               // Unterschallgeschoss
    Explosive,              // Sprenggeschoss
    Incendiary,             // Brandgeschoss
    Ranged,                 // Langestreckengeschoss
    Slug,                   // Flintengeschoss
    Buckshot,               // Shrot
    Flechette               // Nadelgeschosse

}

public enum EFireModes
{
    SingleFire,
    Burst,
    Auto
}

[CreateAssetMenu(fileName = "Weapon Type", menuName = "Extended/Weapon/WeaponType")]
public class WeaponType : ScriptableObject
{
    [Header("Weapon Properties")]
    public EWeaponType type;
    public string weaponName;
    public EFireModes fireModes;
    public int magazineSize = 1;
    public int rateOfFire = 1;
    public float handling = 1f;
    public float accuracy = 1f;
    public float firePower = 1f;

    [Header("Ammo Properties")]
    public EAmmoType ammoType;
    public string ammoName;
    public int damage;
    public float range = 50f;
    
}
