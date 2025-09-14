using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Killthatoneguy : MonoBehaviour
{
    private int _hp = 3;

    [SerializeField] private UnityEvent _onThatGuyDying;

    [SerializeField] private GameObject[] _explosionstuff; 

    [SerializeField] private string nextScene;

    [SerializeField] private GameObject[] _transitionScreen;


    public void SubtractLife()
    {
        _hp -= 1;

        if (_hp <= 0)
        {
            KillThatGuy();
        }
    }

    public void HealToFull()
    {
        _hp = 3; 
    }

    public void KillThatGuy()
    {
        _onThatGuyDying.Invoke();

        StartCoroutine(CoolSFX());
        Invoke(nameof(Cleanup), 3.0f);
    }

    private IEnumerator CoolSFX()
    {
        foreach (GameObject gameObject in _explosionstuff)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(.1f);
        }

    }


    private void Cleanup()
    {
        _transitionScreen[0].SetActive(true);
        Invoke(nameof(SceneTransiton), 1.0f);


    }

    public void SceneTransiton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        //Destroy(gameObject);

    }



}
