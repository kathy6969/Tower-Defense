using UnityEngine;
using System.Collections.Generic;

public class SpiritsParent : MonoBehaviour
{
    [Header("Orbit Settings")]
    public float orbitRadius = 2f;
    public float rotationSpeed = 100f; // độ/giây
    public bool autoUpdateChildren = true;

    private List<Transform> orbitObjects = new List<Transform>();
    private float currentAngle = 0f;

    void Start()
    {
        // Nếu autoUpdateChildren bật, tự thu thập các con có component OrbitingChild
        if (autoUpdateChildren)
        {
            UpdateOrbitObjects();
        }
    }

    void Update()
    {
        if (orbitObjects.Count == 0) return;

        currentAngle += rotationSpeed * Time.deltaTime;

        // Tính toán góc chia đều cho từng object con
        float angleStep = 360f / orbitObjects.Count;
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            if (orbitObjects[i] == null) continue;

            float angle = currentAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 newPos = transform.position + new Vector3(
                orbitRadius * Mathf.Cos(rad),
                orbitRadius * Mathf.Sin(rad),
                0f
            );

            orbitObjects[i].position = newPos;
        }
    }

    // Gọi khi có thay đổi số lượng vệ tinh
    public void UpdateOrbitObjects()
    {
        orbitObjects.Clear();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<Spitits>() != null)
            {
                orbitObjects.Add(child);
            }
        }
    }

    // Có thể dùng khi thêm mới vệ tinh động
    public void RegisterChild(Transform newChild)
    {
        if (!orbitObjects.Contains(newChild))
        {
            orbitObjects.Add(newChild);
        }
    }

    public void UnregisterChild(Transform child)
    {
        orbitObjects.Remove(child);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}
