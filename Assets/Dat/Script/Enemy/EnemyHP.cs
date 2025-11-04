using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EnemyHP : MonoBehaviour
{
    public int BaseHP;
    public int maxHP; // Maximum health points
    public int currentHP; // Current health points
    public float InvincibilityTime = 0.17f; // Invincibility time in seconds after taking damage
    public Image healthBar; // Reference to the health bar UI element
    private bool isInvincible = false; // Invincibility state

    private ChildActivator childActivator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childActivator = GetComponentInParent<ChildActivator>();
        maxHP = BaseHP;
        currentHP = maxHP;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHP -= damage;
        //Debug.Log("Enemy took " + damage + " damage. Current HP: " + currentHP);
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHP / maxHP;
        }
    }
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(InvincibilityTime);
        isInvincible = false;
    }
    public void Die()
    {
        //Debug.Log("Enemy died");
        //Destroy(gameObject);
        if (childActivator != null)
        {
            childActivator.EnableRandomChildren(1, 2);
        }
    }
}
