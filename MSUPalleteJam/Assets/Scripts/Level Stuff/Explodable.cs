using UnityEngine;
using UnityEngine.Events;

public class Explodable : MonoBehaviour
{
    public UnityEvent OnExplode;


    public void Explode()
    {
        if (OnExplode != null) OnExplode.Invoke();

        Destroy(gameObject);
    }
}
