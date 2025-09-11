using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2.5f;

    private void Awake()
    {
        Invoke(nameof(Cleanup), _lifeTime); 
    }



    private void Cleanup()
    {
        Destroy(gameObject);
    }
}
