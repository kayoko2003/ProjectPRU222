using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnKnife = Instantiate(weaponData.Prefab);
        spawnKnife.transform.position = transform.position;
        spawnKnife.GetComponent<KnifeBehaviour>().DirectionChecker();
    }
}
