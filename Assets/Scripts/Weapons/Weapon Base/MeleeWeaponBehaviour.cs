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

    protected virtual void Awake()
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDame(currentDamage);

            KnockBack knockBack = collision.GetComponent<KnockBack>();
            if (knockBack != null)
            {
                knockBack.GetKnockedBack(PlayerController.Instance.transform, 15f);
            }

            Flash flash = collision.GetComponent<Flash>();
            if (flash != null)
            {
                flash.StartCoroutine(flash.FlashRoutine());
            }
        }
    }
}
