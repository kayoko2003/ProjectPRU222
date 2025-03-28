using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats owner;
    [HideInInspector] public Weapon weapon;

    public float GetDamage()
    {
        return weapon.GetDamage();
    }
}
