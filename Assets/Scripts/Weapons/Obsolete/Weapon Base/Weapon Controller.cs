using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 
    protected PlayerController playerController;

    protected virtual void Start()
    {
        playerController = Object.FindAnyObjectByType<PlayerController>();
        currentCooldown = weaponData.CooldownDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
            Attack();
        }
        
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }

}
