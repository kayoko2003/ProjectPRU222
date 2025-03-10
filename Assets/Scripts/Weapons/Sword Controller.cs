using UnityEngine;

public class SwordController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        base.Start();   
    }

    // Update is called once per frame
    protected virtual void Attack()
    {
        base.Attack();    
    }
}
