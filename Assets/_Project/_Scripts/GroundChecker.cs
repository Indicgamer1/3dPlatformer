using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float maxRayDistance;

    public bool IsGrounded {  get; private set; }

    public Vector3 RayOffset = Vector3.up * 0.1f;
    private float SphereRadius = 0.3f;
    private int GroundLayer;
        
    private void Start()=> GroundLayer = LayerMask.GetMask("Ground");

    private void Update()
    {
        Vector3 _RayOrigin = transform.position + RayOffset;

        IsGrounded = Physics.SphereCast(_RayOrigin, SphereRadius, Vector3.down, out _, maxRayDistance, GroundLayer); 
        //Debug.Log($"IsGrounded: {IsGrounded}");
    }

    public float GroundDistance()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, maxRayDistance, GroundLayer);
        return hit.point.y;
    }
}
