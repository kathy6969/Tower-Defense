using UnityEngine;

public class SpinAxe : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f; // độ/giây

    void Update()
    {
        RotateX();
    }

    private void RotateX()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
