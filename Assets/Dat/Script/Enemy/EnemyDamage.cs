using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10;
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
                towerHP.TakeDamage(damageAmount); // Deal damageAmount damage to the tower
            }
        }
    }
}
