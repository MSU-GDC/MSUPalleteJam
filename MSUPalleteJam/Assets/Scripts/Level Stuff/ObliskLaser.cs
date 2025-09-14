using UnityEngine;

public class ObliskLaser : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _projectileSpeed;


    void Awake()
    {
        _rb.linearVelocity = -transform.up * _projectileSpeed; 

    }

}
