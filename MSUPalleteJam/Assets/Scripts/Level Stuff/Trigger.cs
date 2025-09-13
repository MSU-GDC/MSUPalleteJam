using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onTrigger;

    [SerializeField] private LayerMask _validLayers;

    [SerializeField] private bool _triggerOnce;



    void OnTriggerEnter2D(Collider2D collision)
    {


        if (((1 << collision.gameObject.layer) & _validLayers) != 0)
        {
            _onTrigger.Invoke();
            if (_triggerOnce) Destroy(gameObject);
        }
    }

}
