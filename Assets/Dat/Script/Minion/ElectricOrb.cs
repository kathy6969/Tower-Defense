using UnityEngine;

public class ElectricOrb : MonoBehaviour
{
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float orbitRadius = 5f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private int orbCount = 3;
    private GameObject[] orbs;
    private float currentAngle = 0f;
    // NEW – orb quay ngược chiều
    private GameObject[] reverseOrbs;
    private float reverseCurrentAngle = 0f;
    // scale mặc định cho orb mới
    private float newOrbScaleMultiplier = 1f;

    void Start()
    {
        SpawnOrbs();
    }

    void Update()
    {
        RotateOrbs();
        RotateReverseOrbs(); // NEW
    }
    //      Spawn orb bình thường
    private void SpawnOrbs()
    {
        // Xoá orb cũ
        if (orbs != null)
        {
            foreach (var orb in orbs)
                if (orb != null) Destroy(orb);
        }

        orbs = new GameObject[orbCount];

        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * (360f / orbCount) * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * orbitRadius,
                Mathf.Sin(angle) * orbitRadius,
                0f
            );

            orbs[i] = Instantiate(orbPrefab, transform.position + position, Quaternion.identity, transform);

            // NEW – scale chỉ áp dụng cho orb mới spawn
            orbs[i].transform.localScale *= newOrbScaleMultiplier;
        }
    }

    //      Quay orb bình thường
    private void RotateOrbs()
    {
        currentAngle -= rotationSpeed * Time.deltaTime;

        for (int i = 0; i < orbCount; i++)
        {
            float angle = (i * (360f / orbCount) + currentAngle) * Mathf.Deg2Rad;

            orbs[i].transform.localPosition = new Vector3(
                Mathf.Cos(angle) * orbitRadius,
                Mathf.Sin(angle) * orbitRadius,
                0f
            );
        }
    }
    // Tăng số lượng orbs và spawn lại vòng tròn
    public void IncreaseOrbCount(int amount)
    {
        orbCount += amount;
        if (orbCount < 1) orbCount = 1;

        SpawnOrbs();
    }
    //  NEW – spawn thêm orb bằng số lượng hiện có nhưng quay ngược
    public void SpawnReverseOrbs()
    {
        // Xoá orb cũ nếu gọi lại
        if (reverseOrbs != null)
        {
            foreach (var orb in reverseOrbs)
                if (orb != null) Destroy(orb);
        }

        reverseOrbs = new GameObject[orbCount];

        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * (360f / orbCount) * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * (orbitRadius + 1f), // NEW: lệch 1 radius cho dễ nhìn
                Mathf.Sin(angle) * (orbitRadius + 1f),
                0f
            );

            // Spawn orb quay ngược
            reverseOrbs[i] = Instantiate(orbPrefab, transform.position + position, Quaternion.identity, transform);

            // NEW – scale như orb bình thường
            reverseOrbs[i].transform.localScale *= newOrbScaleMultiplier;
        }
    }

    //      NEW – quay orb ngược chiều kim đồng hồ
    private void RotateReverseOrbs()
    {
        if (reverseOrbs == null) return;

        reverseCurrentAngle += rotationSpeed * Time.deltaTime; // quay ngược (+)

        for (int i = 0; i < orbCount; i++)
        {
            float angle = (i * (360f / orbCount) + reverseCurrentAngle) * Mathf.Deg2Rad;

            reverseOrbs[i].transform.localPosition = new Vector3(
                Mathf.Cos(angle) * (orbitRadius + 1f),
                Mathf.Sin(angle) * (orbitRadius + 1f),
                0f
            );
        }
    }
    // Tăng tốc độ quay
    public void IncreaseRotationSpeed(float amount)
    {
        rotationSpeed += amount;
    }
}
