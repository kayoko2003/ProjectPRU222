using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool gettingKnockedBack {  get; private set; }

    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damgeSource, float knockBackThrust)
    {
        gettingKnockedBack = true;
        Vector2 difference = (transform.position - damgeSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRountine());
    }

    private IEnumerator KnockRountine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.linearVelocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
