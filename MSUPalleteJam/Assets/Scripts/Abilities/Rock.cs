using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2.5f;
    [SerializeField] private AudioSource _audioSource; 

    private void Awake()
    {
        Invoke(nameof(Cleanup), _lifeTime); 
    }



    private void Cleanup()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _audioSource.Play();
    }
}
