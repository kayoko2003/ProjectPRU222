using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    protected Transform player;
    protected EnemyStats enemyStats;

    protected Vector2 knockbackVelocity;
    protected float knockbackDuration;

    public enum OutOfFrameAction { none, respawnAtEdge, despawn}
    public OutOfFrameAction outOfFrameAction = OutOfFrameAction.respawnAtEdge;

    protected bool spawnedOutOFrame = false;

    protected virtual void Start()
    {
        spawnedOutOFrame = !SpawnManager.IsWithinBoundaries(transform);
        enemyStats = GetComponent<EnemyStats>();

        PlayerController[] allPlayers = Resources.FindObjectsOfTypeAll<PlayerController>();

        player = allPlayers[Random.Range(0, allPlayers.Length)].transform;
    }

    protected virtual void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            Move();
            HandleOutOfFrameAction();
        }
    }

    protected virtual void HandleOutOfFrameAction()
    {
        if (!SpawnManager.IsWithinBoundaries(transform))
        {
            switch (outOfFrameAction)
            {
                case OutOfFrameAction.none: default: break;
                case OutOfFrameAction.respawnAtEdge:
                    transform.position = SpawnManager.GeneratePosition();
                    break;
                case OutOfFrameAction.despawn:
                    if (!spawnedOutOFrame)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }
        else spawnedOutOFrame = false;
    }

    public virtual void KnockBack(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0)
        {
            return;
        }

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }

    public virtual void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyStats.currentMoveSpeed * Time.deltaTime);
    }
}
