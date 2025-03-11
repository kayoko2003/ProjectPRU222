using System;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    public float destroyAfterSeconds;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    protected void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogError("Weapon data is null on " + gameObject.name);
            return;
        }

        Debug.Log("Weapon damage loaded: " + weaponData.Damage);

        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log(currentDamage);
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDame(currentDamage);
        }
    }
}
