using System.Collections.Generic;
using UnityEngine;

public class Aura : WeaponEffect
{
    Dictionary<EnemyStats, float> affectedTargets = new Dictionary<EnemyStats, float>();
    List<EnemyStats> targetsToUnaffect = new List<EnemyStats>();

    void Update()
    {
        Dictionary<EnemyStats, float> affectedTargsCopy = new Dictionary<EnemyStats, float>(affectedTargets);

        foreach(KeyValuePair<EnemyStats, float> pair in affectedTargsCopy)
        {
            affectedTargets[pair.Key] -= Time.deltaTime;
            if(pair.Value <= 0)
            {
                if(targetsToUnaffect.Contains(pair.Key))
                {
                    affectedTargets.Remove(pair.Key);
                    targetsToUnaffect.Remove(pair.Key);
                }
                else 
                {
                    Weapon.Starts stats = weapon.GetStarts();
                    affectedTargets[pair.Key] = stats.cooldown * weapon.Owner.Stats.cooldown;
                    pair.Key.TakeDame(GetDamage(), transform.position, stats.knockback );
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyStats es))
        {
            if(!affectedTargets.ContainsKey(es))
            {
                affectedTargets.Add(es, 0);
            }
            else
            {
                if(targetsToUnaffect.Contains(es))
                {
                    targetsToUnaffect.Remove(es);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyStats es))
        {
            if(affectedTargets.ContainsKey(es))
            {
                targetsToUnaffect.Add(es);
            }
        }
    }
}
