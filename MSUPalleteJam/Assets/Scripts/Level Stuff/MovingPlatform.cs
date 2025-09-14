using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _destination;

    [SerializeField] private float _transversalTime = 2.0f;


    private Vector3 _lerpStart;

    private Vector3 _lerpEnd;

    private bool _currentlyMoving;

    private float _cTime;


    private IEnumerator MovePlatform()
    {
        _currentlyMoving = true;
        _cTime = 0.0f;

        while (!transform.position.Equals(_lerpEnd))
        {
            float lerpPos = _cTime / _transversalTime;

            transform.position = Vector3.Lerp(_lerpStart, _lerpEnd, lerpPos);

            _cTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        _currentlyMoving = false; 

    }





    public void MoveToStart()
    {
        if (_currentlyMoving) return;
        _lerpStart = _destination.position;
        _lerpEnd = _start.position;

        StartCoroutine(MovePlatform());


    }

    public void MoveToDestination()
    {
        if (_currentlyMoving) return;

        _lerpStart = _start.position;
        _lerpEnd = _destination.position;
        StartCoroutine(MovePlatform()); 
    }




}
