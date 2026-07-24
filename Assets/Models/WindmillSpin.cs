using UnityEngine;

public class WindmillSpin:MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 spinDirection = new Vector3(0f, 1f, 0f);
    [SerializeField] private float spinSpeed = 100f;

    [Header("Movement Type")]
    [SerializeField] private bool useLocalSpace = true;

    void Update()
    {
        // Calculate rotation for this frame
        float rotationAmount = spinSpeed * Time.deltaTime;

        // Apply rotation based on chosen coordinate space
        if (useLocalSpace)
        {
            transform.Rotate(spinDirection * rotationAmount, Space.Self);
        }
        else
        {
            transform.Rotate(spinDirection * rotationAmount, Space.World);
        }
    }
}
