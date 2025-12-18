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

    public int armor = 0;
    public int damageReductionPerArmor = 0;

    private bool isInvincible = false;
    private float lastDamageTime;

    void Start()
    {
        currentHP = maxHP;
        lastDamageTime = Time.time;

        StartCoroutine(HealLoop());
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        int effectiveDamage = damage * (1-((armor*damageReductionPerArmor)/100));
        if (effectiveDamage < 0) effectiveDamage = 0;

        currentHP -= effectiveDamage;
        lastDamageTime = Time.time;

        Debug.Log($"Tower took {effectiveDamage} damage. HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator HealLoop()
    {
        while (true)
        {
            // ⏱ chờ đủ regen delay kể từ lần bị đánh gần nhất
            if (Time.time - lastDamageTime >= regenDelay &&
                currentHP < maxHP)
            {
                currentHP += healAmount;
                currentHP = Mathf.Min(currentHP, maxHP);

                Debug.Log("Tower healed. HP: " + currentHP);
            }

            yield return new WaitForSeconds(healRegenRate);
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
        // không reset regen delay
        Debug.Log("Max HP increased to " + maxHP);
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
