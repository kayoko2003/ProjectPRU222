using UnityEngine;

public abstract class Weapon : Item
{
    [System.Serializable]
    public struct Starts
    {
        public string name, description;

        [Header("Visuals")]
        public Projectile projectilePrefab;
        public Aura auraPrefab;
        public ParticleSystem hitEffect;
        public Rect spawnVariance;

        [Header("Values")]
        public float lifespan;
        public float damage, damageVariance, area, speed, cooldown, projectileInterval, knockback;
        public int number, piercing, maxInstances;

        public static Starts operator +(Starts s1, Starts s2)
        {
            Starts result = new Starts();
            result.name = s2.name ?? s1.name;
            result.description = s2.description ?? s1.description;
            result.projectilePrefab = s2.projectilePrefab ?? s1.projectilePrefab;
            result.auraPrefab = s2.auraPrefab ?? s1.auraPrefab;
            result.hitEffect = s2.hitEffect == null ? s1.hitEffect : s2.hitEffect;
            result.spawnVariance = s2.spawnVariance;
            result.lifespan = s1.lifespan + s2.lifespan;
            result.damage = s1.damage + s2.damage;
            result.damageVariance = s1.damageVariance + s2.damageVariance;
            result.area = s1.area + s2.area;
            result.speed = s1.speed + s2.speed;
            result.cooldown = s1.cooldown + s2.cooldown;
            result.number = s1.number + s2.number;
            result.piercing = s1.piercing + s2.piercing;
            result.projectileInterval = s1.projectileInterval + s2.projectileInterval;
            result.knockback = s1.knockback + s2.knockback;
            return result;
        }

        public float GetDamage()
        {
            return damage + Random.Range(0, damageVariance);
        }
    }

    protected Starts currentStats;

    public WeaponData data;

    protected float currentCooldown;

    protected PlayerController movement;

    public virtual void Initialise(WeaponData data)
    {
        base.Intialise(data);
        this.data = data;
        currentStats = data.baseStarts;
        movement = Object.FindAnyObjectByType<PlayerController>();
        currentCooldown = currentStats.cooldown;
    }

    protected virtual void Awake()
    {
        if (data)
        {
            currentStats = data.baseStarts;
        }
    }

    protected virtual void Start()
    {
        if (data)
        {
            Initialise(data);
        }
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
            Attack(currentStats.number);
        }
    }

    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        if (!CanLevelUp())
        {
            return false;
        }
        currentStats += data.GetLevelData(++currentLevel);
        return true; 
    }

    public virtual bool CanAttack()
    {
        return currentCooldown <= 0;
    }

    protected virtual bool Attack(int attackCount = 1)
    {
        if (CanAttack())
        {
            currentCooldown += currentStats.cooldown;
            return true;
        }
        return false;
    }

    public virtual float GetDamage()
    {
        return currentStats.GetDamage() * owner.CurrentMight;
    }

    public virtual Starts GetStarts() { return currentStats; }
}
