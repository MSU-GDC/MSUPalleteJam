using UnityEngine;
using UnityEngine.Events;

public class Explodable : MonoBehaviour
{
    public UnityEvent OnExplode;

    public bool DestroyOnExplode = true;


    public void Explode()
    {
        if (OnExplode != null) OnExplode.Invoke();

        if (DestroyOnExplode) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
