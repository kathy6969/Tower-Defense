using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            TowerHP towerHP = collision.gameObject.GetComponent<TowerHP>();
            if (towerHP != null)
            {
                towerHP.TakeDamage(10); // Deal 10 damage to the tower
            }
        }
    }
}
