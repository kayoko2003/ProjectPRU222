using System.Collections;
using UnityEngine;



public class BossMovement : MonoBehaviour
{
    public float attackRange = 2f; // Khoảng cách để tấn công
    public float attackCooldown = 2f; // Thời gian chờ giữa các đòn đánh

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
            
            if (!animator.GetBool("Move"))
            {
                Debug.Log("dang di bo vao player");
                animator.SetBool("Move", true);
            }
        }
        else
        {
            StartCoroutine(Attack());
        }
    }


    

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("Move", false);

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

