using UnityEngine;

public class StealAxes : MonoBehaviour
{
    public GameObject axePrefab;
    public TowerShooter towerShooter;
    public Transform throwPoint;
    public float throwCooldown = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerShooter = GetComponentInParent<TowerShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        throwCooldown -= Time.deltaTime;
        if (throwCooldown <= 0f)
        {
            ThrowAxe();
            throwCooldown = 2f; // Reset cooldown
        }
    }
    void ThrowAxe()
    {
        var targets = towerShooter.currentTargets;
        if (targets == null || targets.Count == 0) return;
        foreach (Transform target in targets)
        {
            if (target == null) continue;
            GameObject axe = Instantiate(axePrefab, throwPoint.position, Quaternion.identity);   
            Vector3 direction = (target.position - transform.position).normalized;
            Bullet axeBullet = axe.GetComponent<Bullet>();
            if (axeBullet != null)
            {
                axeBullet.Launch(target);
            }
        }
    }
}
