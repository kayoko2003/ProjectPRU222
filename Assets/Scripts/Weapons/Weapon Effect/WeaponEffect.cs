using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats owner;
    [HideInInspector] public Weapon weapon;

    public PlayerStats Owner;

    public float GetDamage()
    {
        return weapon.GetDamage();
    }
}
