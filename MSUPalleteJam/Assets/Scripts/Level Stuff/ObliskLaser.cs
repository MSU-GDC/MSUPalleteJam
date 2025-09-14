using Unity.VisualScripting;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Singleton.Die(); 
        }
    }


    private void Cleanup()
    {
        Destroy(gameObject);
    }

}
