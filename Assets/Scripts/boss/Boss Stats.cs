using Assets.FantasyMonsters.Scripts;
using System.Collections;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerStats>().transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }
    public Monster Monster;
    public void TakeDame(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log(gameObject.name + " nhận " + dmg + " sát thương!");
        if (currentHealth <= 0)
        {
            //Monster.Die();
            Kill();
        }
    }
    public void Kill()
    {
        Destroy(gameObject);

    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }
        EnemySpawn es = Object.FindAnyObjectByType<EnemySpawn>();
        es.OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        EnemySpawn es = Object.FindAnyObjectByType<EnemySpawn>();
        transform.position = player.position + es.relativeSpawnPoint[Random.Range(0, es.relativeSpawnPoint.Count)].position;
    }
}