using UnityEngine;
using System.Collections;

public class TowerHP : MonoBehaviour
{
    public int maxHP; // Maximum health points
    public int currentHP; // Current health points
    public float InvincibilityTime = 0.17f; // Invincibility time in seconds after taking damage
    private bool isInvincible = false; // Invincibility state
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHP -= damage;
        Debug.Log("Tower took " + damage + " damage. Current HP: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
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
        Debug.Log("Tower destroyed");
        // Implement tower destruction logic here
    }
}
