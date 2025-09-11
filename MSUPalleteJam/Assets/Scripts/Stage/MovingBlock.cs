using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public Vector2[] positionTargets;
    public bool isActive;
    private Vector2 targetPosition;
    private int travelIndex;
    private float elapsedTime;
    private float duration = 1f;
    private float speed = 0.01f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positionTargets[0] = transform.position;
        targetPosition = positionTargets[1];
        travelIndex = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime;
        // Calculate the interpolation factor (0 to 1)
        float t = Mathf.Clamp01(elapsedTime / duration);
        //checks if the block is supposed to be moving
        //forbidden tech
        Vector2 roundedPosition = new Vector2((float)Math.Round(transform.position.x, 2, MidpointRounding.AwayFromZero), (float)Math.Round(transform.position.y, 2, MidpointRounding.AwayFromZero));
        if (isActive && roundedPosition != targetPosition)
        {
            //Debug.Log("moving towards: " + targetPosition);
            transform.position = Vector2.Lerp(transform.position, targetPosition, t * speed);
        }
        else if (isActive)
        {
            //Debug.Log("switching destinations" + positionTargets[travelIndex]);
            if (travelIndex == positionTargets.Length - 1)
            {
                
                travelIndex = 0;
            }
            else
            {
                travelIndex++;
            }
            targetPosition = positionTargets[travelIndex];
        }
    }
}
