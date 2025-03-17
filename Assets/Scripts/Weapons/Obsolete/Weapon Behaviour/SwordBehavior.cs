using UnityEngine;

public class SwordBehavior : MeleeWeaponBehaviour
{
    SwordController sc;

    [SerializeField] private GameObject slashAnimaPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    PlayerController playerController;
    private GameObject slashAnim;
    private Animator animator;

    private KnockBack knockBack;
    private Flash flash;

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        sc = Object.FindAnyObjectByType<SwordController>();
        playerController = Object.FindAnyObjectByType<PlayerController>();

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        slashAnim = Instantiate(slashAnimaPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    public void SwingDownFlipAnimEvent()
    {
        if (playerController.lastHorizontalVector < 0)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
