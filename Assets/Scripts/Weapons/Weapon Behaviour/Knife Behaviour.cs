using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehavior
{
   
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.position += direction * weaponData.Speed * Time.deltaTime;
    }
}
