using UnityEngine;

public class SwordBehavior : ProjectileWeaponBehavior
{
    SwordController sc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        sc = Object.FindAnyObjectByType<SwordController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
