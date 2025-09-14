using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ObliskBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animator;


    [SerializeField] private GameObject _projectile;

    [SerializeField] private Transform _projectileSpawnPoint;

    [SerializeField] private float _shotCooldown = 1.0f;

    [SerializeField] private float _shotTimeSeconds = 0.3f;




    void Awake()
    {
        StartCoroutine(FireRoutine());
    }


    public IEnumerator FireRoutine()
    {
        yield return new WaitUntil(() => Player.Singleton != null);
        while (true)
        {
            yield return new WaitForSecondsRealtime(_shotCooldown);

            _animator.SetBool("IsFire", true);

            yield return new WaitForSecondsRealtime(_shotTimeSeconds);

            Vector2 playerPos = (Vector2)Player.Singleton.transform.position;

            Vector2 projSpawnPos = (Vector2)_projectileSpawnPoint.position;

            float dX = -(projSpawnPos.x - playerPos.x);
            float dY = (projSpawnPos.y - playerPos.y);


            float angle = Mathf.Atan2(dX, dY) * Mathf.Rad2Deg;


            GameObject.Instantiate(_projectile, projSpawnPos, Quaternion.Euler(0f, 0f, angle));

            _animator.SetBool("IsFire", false);

        }




    }







}
