using UnityEngine;
using System.Collections.Generic;

public class ToxicGases : MonoBehaviour
{
    [System.Serializable]
    private class GasInstance
    {
        public GameObject gameObject;
        public SpriteRenderer sr;
        public Rigidbody2D rb;
        public Vector3 direction;
        public float speed;
        public float timeLeft;
        public int type;
        public float lifeTime;
    }

    public GameObject[] toxicGasPrefabs;  
    public int poolSize = 20;
    public float lifeTime = 4f;
    public float emissionRate = 8f;    // số khí phun ra mỗi lần
    public float emissionInterval = 0.5f; // khoảng thời gian giữa các lần phun
    public float emissionTimer = 0f;

    public float startSpeed = 3f;        // tốc đẩy lúc phun
    public float deceleration = 0.95f;   // giảm tốc mỗi frame

    public float startScale = 0.25f;
    public float endScale = 1.2f;

    private Queue<GameObject>[] pools;
    private List<GasInstance> active = new List<GasInstance>();

    void Start()
    {
        pools = new Queue<GameObject>[toxicGasPrefabs.Length];
        for (int i = 0; i < toxicGasPrefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
                GameObject g = Instantiate(toxicGasPrefabs[i], transform);
                g.SetActive(false);
                pools[i].Enqueue(g);
            }
        }
    }

    void Update()
    {
        emissionTimer += Time.deltaTime;
        if (emissionTimer >= emissionInterval)
        {
            emissionTimer = 0f;
            SpawnGas((int)emissionRate);
        }
        UpdateGases();
    }

    // 🔥 PHUN THEO ĐỢT – gọi hàm này ở script lỗ phun khí
    public void SpawnGas(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int type = Random.Range(0, toxicGasPrefabs.Length);
            GameObject g = GetFromPool(type);

            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
            Rigidbody2D rb = g.GetComponent<Rigidbody2D>();

            g.SetActive(true);
            g.transform.localPosition = Vector3.zero;
            g.transform.localScale = Vector3.one * startScale;

            // tạo instance
            GasInstance inst = new GasInstance();
            inst.gameObject = g;
            inst.sr = sr;
            inst.rb = rb;
            inst.type = type;

            // hướng random
            inst.direction = Random.insideUnitCircle.normalized;

            // tốc độ random nhẹ
            inst.speed = Random.Range(startSpeed * 0.9f, startSpeed * 1.1f);

            inst.lifeTime = lifeTime;
            inst.timeLeft = lifeTime;

            // 🔥 Đẩy bằng RIGIDBODY
            if (rb != null)
            {
                rb.linearVelocity = inst.direction * inst.speed;
            }

            active.Add(inst);
        }
    }

    private GameObject GetFromPool(int type)
    {
        if (pools[type].Count > 0)
            return pools[type].Dequeue();

        GameObject g = Instantiate(toxicGasPrefabs[type], transform);
        g.SetActive(false);
        return g;
    }

    private void ReturnToPool(GasInstance g)
    {
        // reset rb
        if (g.rb != null)
            g.rb.linearVelocity = Vector2.zero;

        g.gameObject.SetActive(false);
        pools[g.type].Enqueue(g.gameObject);
    }

    void UpdateGases()
    {
        for (int i = active.Count - 1; i >= 0; i--)
        {
            GasInstance g = active[i];
            g.timeLeft -= Time.deltaTime;

            // 🔥 GIẢM TỐC VẼ BẰNG RIGIDBODY
            if (g.rb != null)
            {
                g.rb.linearVelocity *= deceleration;
            }

            // scale tăng dần
            float t = 1f - (g.timeLeft / g.lifeTime);
            g.gameObject.transform.localScale = Vector3.Lerp(
                Vector3.one * startScale,
                Vector3.one * endScale,
                t
            );

            // fade out
            Color c = g.sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            g.sr.color = c;

            if (g.timeLeft <= 0f)
            {
                // reset alpha
                c = g.sr.color;
                c.a = 1f;
                g.sr.color = c;

                ReturnToPool(g);
                active.RemoveAt(i);
            }
        }
    }
}
