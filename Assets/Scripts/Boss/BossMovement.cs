using UnityEngine;
using System.Collections;

public class BossMovement : MonoBehaviour
{
    public float attackRange = 2f; // Khoảng cách để tấn công
    public float attackCooldown = 2f; // Thời gian chờ giữa các đòn đánh
    public float moveSpeed = 3f; // Tốc độ di chuyển

    private Transform player;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        PlayerController playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        FlipTowardsPlayer();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StartCoroutine(Attack());
        }
    }


    void MoveTowardsPlayer()
    {
        if (player == null) return;

        // Tính hướng di chuyển
        Vector3 direction = (player.position - transform.position).normalized;

        // Di chuyển Boss theo hướng của Player
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // Kiểm tra nếu animation đang chạy
        if (!animator.GetBool("Walk"))
        {
            animator.SetBool("Walk", true);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("Walk", false);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        // Kiểm tra vị trí của player so với boss
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
    }

}
