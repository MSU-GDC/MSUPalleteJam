using UnityEngine;

public class ObliskLaser : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _lifeTime = 3f;


    void Awake()
    {
        _rb.linearVelocity = -transform.up * _projectileSpeed;

        Invoke(nameof(Cleanup), _lifeTime);

    }


    private void Cleanup()
    {
        Destroy(gameObject);
    }

}
