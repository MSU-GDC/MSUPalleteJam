using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public Animator anim;
    public GameObject menu;
    public GameObject creditsBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        menu.SetActive(false);
        creditsBack.SetActive(true);
        anim.SetTrigger("Credits");

    }

    public void BackToMenu()
    {
        menu.SetActive(true);
        creditsBack.SetActive(false);
        anim.SetTrigger("BackToMenu");
    }
}
