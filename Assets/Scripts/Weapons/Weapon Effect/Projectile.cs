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
        float aimAngle;

        EnemyStats[] targets = FindObjectsOfType<EnemyStats>();

        if (targets.Length > 0)
        {
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 difference = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else
        {
            aimAngle = Random.Range(0f, 360f);
        }

        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
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
