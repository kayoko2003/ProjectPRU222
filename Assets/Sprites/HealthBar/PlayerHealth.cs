using UnityEngine;
using UnityEngine.Events;

public class Healtha : MonoBehaviour
{
    [SerializeField] int maxHealth;
    int currentHealth;

    public HealthBar healthBar;
    public UnityEvent OnDeath;

    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }
    private void Start()
    {
        currentHealth = maxHealth;

        /*healthBar.UpdateBar(currentHealth, maxHealth);*/

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            OnDeath.Invoke();
        }

       /* healthBar.UpdateBar(currentHealth, maxHealth);*/
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
            TakeDamage(20);
            }
    }
}
