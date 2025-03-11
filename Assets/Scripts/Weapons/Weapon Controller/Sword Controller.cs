using System.Collections;
using UnityEngine;

public class SwordController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        GameObject spawnSword = Instantiate(weaponData.Prefab);

        if (mousePos.x < playerScreenPoint.x)
        {
            transform.rotation = Quaternion.Euler(-180, 0, angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        spawnSword.transform.position = playerController.transform.position;
        spawnSword.transform.parent = transform;
    }
}
