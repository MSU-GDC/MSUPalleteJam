using UnityEngine;
using UnityEngine.InputSystem;

public class SceneQuitter : MonoBehaviour
{
    private bool _canQuit = false;

    public void QuitScene()
    {
        _canQuit = true;
    }


    void Update()
    {
        if (_canQuit && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit(); 
        }
    }
}
