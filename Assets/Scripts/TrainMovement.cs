using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public float speed = 10f; // Normal h�z
    public float slowSpeed = 2f; // Yava�lama h�z�
    private bool isSlowingDown = false;
    private bool isStopped = false;

    void Update()
    {
        if (!isStopped)
        {
            float currentSpeed = isSlowingDown ? slowSpeed : speed;
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }

    public void SlowDown()
    {
        Debug.Log("Train is slowing down...");
        isSlowingDown = true;
    }

    public void SpeedUp()
    {
        Debug.Log("Train is speeding up...");
        isSlowingDown = false;
    }

    public void Stop()
    {
        Debug.Log("Train has stopped.");
        isStopped = true;
    }

    public void Resume()
    {
        Debug.Log("Train is resuming.");
        isStopped = false;
        SpeedUp();
    }
}
