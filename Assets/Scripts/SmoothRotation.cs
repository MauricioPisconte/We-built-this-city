using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float positionSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float additionalRotationY = 180f;

    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.Lerp(transform.position, target.position, positionSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(target.parent.position - transform.position);

        targetRotation *= Quaternion.Euler(0f, additionalRotationY, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
