using UnityEngine;

public class ProjectileWeaponBehavior : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;
    public float offsetAngle = 0f;


    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float getCurrentDamage()
    {
        return currentDamage *= Object.FindAnyObjectByType<PlayerStats>().CurrentMight;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
        DirectionChecker();
    }

    public void DirectionChecker()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + offsetAngle);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDame(getCurrentDamage());

            Flash flash = collision.GetComponent<Flash>();
            if (flash != null)
            {
                flash.StartCoroutine(flash.FlashRoutine());
            }

            ReducePiere();
        }
        else if (collision.CompareTag("Prop"))
        {
            if(collision.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(getCurrentDamage());
                ReducePiere();
            }
        }
    }

    void ReducePiere()
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
