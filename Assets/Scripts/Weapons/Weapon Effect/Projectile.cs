using System.Collections.Generic;
using UnityEngine;

public class Projectile : WeaponEffect
{
    public enum DamageSource { projectile, owner};
    public DamageSource damageSource = DamageSource.projectile;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);

    protected Rigidbody2D rb;
    protected int piercing;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Weapon.Starts starts = weapon.GetStarts();
        if(rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.linearVelocity = transform.right * starts.speed;
        }

        float area = starts.area == 0 ? 1 : starts.area;
        transform.localScale = new Vector3(
            area * Mathf.Sign(transform.localScale.x),
            area * Mathf.Sign(transform.localScale.y), 1
        );

        piercing = starts.piercing;

        if(starts.lifespan > 0) Destroy(gameObject, starts.lifespan);

        if (hasAutoAim) AcquireAutoAimFacing();
    }

    public virtual void AcquireAutoAimFacing()
    {
        EnemyStats[] enemyTargets = FindObjectsByType<EnemyStats>(FindObjectsSortMode.None);
        GameObject[] propObjects = GameObject.FindGameObjectsWithTag("Prop");

        List<GameObject> targets = new List<GameObject>();
        foreach (EnemyStats enemy in enemyTargets)
        {
            if (enemy != null)
                targets.Add(enemy.gameObject);
        }
        foreach (GameObject prop in propObjects)
        {
            if (prop != null)
                targets.Add(prop);
        }

        if (targets.Count > 0)
        {
            GameObject closestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject target in targets)
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            if (closestTarget != null)
            {
                Vector2 difference = closestTarget.transform.position - transform.position;
                float aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, aimAngle);
            }
        }
        else
        {
            float aimAngle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }
    }



    protected virtual void FixedUpdate()
    {
        if(rb.bodyType == RigidbodyType2D.Kinematic)
        {
            Weapon.Starts starts = weapon.GetStarts();
            transform.position += transform.right * starts.speed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStats es = collision.GetComponent<EnemyStats>();
        BreakableProps p = collision.GetComponent<BreakableProps>();

        if (es)
        {
            Vector3 source = damageSource == DamageSource.owner && owner ? owner.transform.position : transform.position;

            es.TakeDame(GetDamage(), source);

            Weapon.Starts starts = weapon.GetStarts();
            piercing--;
            if (starts.hitEffect)
            {
                Destroy(Instantiate(starts.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }
        else if (p)
        {
            p.TakeDamage(GetDamage());
            piercing--;

            Weapon.Starts starts = weapon.GetStarts();
            if (starts.hitEffect)
            {
                Destroy(Instantiate(starts.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }

        if(piercing <=0) Destroy(gameObject);
    }
}
