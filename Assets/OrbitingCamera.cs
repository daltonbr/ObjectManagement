using UnityEngine;

public class OrbitingCamera : MonoBehaviour
{
    [SerializeField]
    private Transform focus = default;
    
    [Tooltip("degrees per second")]
    [SerializeField, Range(-360f, 360f)]
    private float angularSpeed = 20f;
    
    private void LateUpdate()
    {
        transform.RotateAround(focus.position, Vector3.up, angularSpeed * Time.deltaTime);
    }
    
}
