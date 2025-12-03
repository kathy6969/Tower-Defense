using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinecraftTNT : MonoBehaviour
{
    [SerializeField] private GameObject tntPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] public float throwCooldown = 5f;
    [SerializeField] public int tntCountPerThrow = 3;
    
    private Queue<GameObject> tntPool = new Queue<GameObject>();
    private List<GameObject> activeTNTs = new List<GameObject>();
    private float cooldownTimer = 0f;

    void Start()
    {
        InitializePool();
        cooldownTimer = throwCooldown;
        ThrowTNT();
    }

    void Update()
    {
        UpdateThrowCooldown();
    }

    // Cập nhật cooldown và ném TNT
    private void UpdateThrowCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        
        if (cooldownTimer <= 0f)
        {
            ThrowTNT();
            cooldownTimer = throwCooldown;
        }
    }

    // Khởi tạo object pool
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tnt = Instantiate(tntPrefab, Vector3.zero, Quaternion.identity, transform);
            tnt.SetActive(false);
            tntPool.Enqueue(tnt);
        }
    }

    // Lấy TNT từ pool hoặc tạo mới nếu hết
    private GameObject GetTNTFromPool()
    {
        GameObject tnt;
        
        if (tntPool.Count > 0)
        {
            tnt = tntPool.Dequeue();
        }
        else
        {
            tnt = Instantiate(tntPrefab, Vector3.zero, Quaternion.identity, transform);
        }
        
        tnt.SetActive(true);
        activeTNTs.Add(tnt);
        return tnt;
    }

    // Trả TNT về pool
    private void ReturnTNTToPool(GameObject tnt)
    {
        tnt.SetActive(false);
        tnt.transform.position = Vector3.zero;
        
        // Reset velocity nếu có Rigidbody
        Rigidbody2D rb = tnt.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        activeTNTs.Remove(tnt);
        tntPool.Enqueue(tnt);
    }

    // Ném TNT đến vị trí ngẫu nhiên
    public void ThrowTNT()
    {
        for (int i = 0; i < tntCountPerThrow; i++)
        {
            ThrowSingleTNT();
        }
    }

    private void ThrowSingleTNT()
    {
        GameObject tnt = GetTNTFromPool();
        
        // Vị trí ngẫu nhiên gần quanh
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(throwRange * 0.5f, throwRange);
        Vector3 targetPosition = transform.position + (Vector3)randomDirection * randomDistance;
        
        tnt.transform.position = transform.position;
        
        // Ném TNT
        Rigidbody2D rb = tnt.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (targetPosition - tnt.transform.position).normalized;
            rb.linearVelocity = direction * throwForce;
        }
        
        // Sau 3 giây disable và trả về pool
        StartCoroutine(MoveToTargetAndStop(tnt, targetPosition));
    }

    private IEnumerator MoveToTargetAndStop(GameObject tnt, Vector3 targetPosition)
    {
        Rigidbody2D rb = tnt.GetComponent<Rigidbody2D>();
        
        // Di chuyển cho đến khi đến gần vị trí đích
        while (Vector3.Distance(tnt.transform.position, targetPosition) > 0.1f)
        {
            yield return null;
        }
        
        // Dừng lại
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        tnt.transform.position = targetPosition;
        
        // Chờ 3 giây rồi disable
        yield return new WaitForSeconds(explosionDelay);
        
        if (tnt.activeInHierarchy)
        {
            ReturnTNTToPool(tnt);
        }
    }
}
