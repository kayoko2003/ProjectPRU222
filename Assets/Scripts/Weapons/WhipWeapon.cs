using UnityEngine;

public class WhipWeapon : ProjectileWeapon
{
    int currentSpawnCount;
    float curentSpawnYOffset;

    protected override bool Attack(int attackCount = 1)
    {
        if(!currentStats.projectilePrefab)
        {
            currentCooldown = data.baseStarts.cooldown;
            return false;
        }

        if (!CanAttack())
        {
            return false;
        }

        if (currentCooldown <= 0)
        {
            currentSpawnCount = 0;
            curentSpawnYOffset = 0f;
        }

        float spawnDir = Mathf.Sign(movement.lastMovedVector.x) * (currentSpawnCount % 2 !=0 ? -1 : 1);
        Vector2 spawnOffset = new Vector2(
            spawnDir * Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),
            curentSpawnYOffset
        );

        Projectile prefab = Instantiate(
            currentStats.projectilePrefab,
            owner.transform.position + (Vector3)spawnOffset,
            Quaternion.identity);

        prefab.owner = owner;

        if(spawnDir < 0)
        {
            prefab.transform.localScale = new Vector3(

                -Mathf.Abs(prefab.transform.localScale.x),
                prefab.transform.localScale.y,
                prefab.transform.localScale.z
            );
        }

        prefab.weapon = this;
        currentCooldown = data.baseStarts.cooldown;
        attackCount--;

        currentSpawnCount++;
        if(currentSpawnCount > 1 && currentSpawnCount % 2 == 0)
        {
            curentSpawnYOffset += 1;
        }

        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = data.baseStarts.projectileInterval;
        }

        return true;
    }
}
