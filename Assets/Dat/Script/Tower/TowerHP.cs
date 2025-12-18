using UnityEngine;
using System.Collections;

public class TowerHP : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public float invincibilityTime = 0.25f;

    public float healRegenRate = 1f;
    public int healAmount = 1;
    public float regenDelay = 3f;

    public int armor;
    public int damageReduction = 5;

    private bool isInvincible = false;
    private float lastDamageTime;
    private float nextHealTime;
    private Coroutine healCoroutine;
    private Coroutine invincibilityCoroutine;

    void Start()
    {
        currentHP = maxHP;
        lastDamageTime = Time.time;
        nextHealTime = Time.time + regenDelay;
        
        healCoroutine = StartCoroutine(HealLoop());
    }

    void OnDisable()
    {
        if (healCoroutine != null)
            StopCoroutine(healCoroutine);
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        
        int finalDamage = Mathf.RoundToInt(damage - armor / 2f);
        int finalDamageA = finalDamage * (100 - damageReduction) / 100;
        currentHP -= finalDamageA;
        lastDamageTime = Time.time;
        nextHealTime = lastDamageTime + regenDelay;
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);
        invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator HealLoop()
    {
        var waitTime = new WaitForSeconds(healRegenRate);
        
        while (true)
        {
            if (Time.time >= nextHealTime && currentHP < maxHP)
            {
                currentHP += healAmount;
                currentHP = Mathf.Min(currentHP, maxHP);
            }
            yield return waitTime;
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    public void IncreaseMaxHP(int amount)
    {
        maxHP += amount;
    }

    public void HealInstant(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    public void Die()
    {
        Debug.Log("Tower destroyed");
    }
}
