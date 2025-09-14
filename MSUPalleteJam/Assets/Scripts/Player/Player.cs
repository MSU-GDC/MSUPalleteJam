using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Singleton;


    [Header("Player Scripts")]
    public Player_Controller Controller;
    public PlayerSoundController SoundController;

    [Header("Global References")]
    public Camera MainCamera;

    public Transform ProjectileSpawnPt;

    public Rigidbody2D PlayerRigidbody;


    public Transform RespawnPT;

    public UnityEvent OnDie;





    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogError("ERROR!!! MULTIPLE INSTANCES OF PLAYER.CS DETECTED, DEACTIVATING MOST RECENT INSTANCE!");
            gameObject.SetActive(false);
        }
        else Singleton = this;
    }


    public void Die()
    {
        transform.position = RespawnPT.position;

        OnDie.Invoke();
    }
}
