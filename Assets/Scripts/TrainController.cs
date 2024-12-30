using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float speed = 25f;       // Normal h�z
    public float slowSpeed = 5f;   // Yava� h�z
    private bool isSlowingDown = false; // Yava�lama durumu
    private bool isStopped = false;    // Durma durumu

    void Update()
    {
        if (isStopped)
        {
            return; // Tren tamamen durduysa hareket etmeyecek
        }

        float currentSpeed = isSlowingDown ? slowSpeed : speed;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    // H�z ayar�
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        isSlowingDown = false; // H�z do�rudan ayarland���nda yava�lama iptal edilir
    }

    // Tren yava�lar
    public void SlowDown()
    {
        isSlowingDown = true;
    }

    // Tren durur
    public void Stop()
    {
        isStopped = true;
    }

    // Tren yeniden ba�lar
    public void Resume()
    {
        isStopped = false;
        isSlowingDown = false;
    }

    // Tetikleyiciye g�re h�z ayar�
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "SlowTrigger1":
                SetSpeed(17f);
                Debug.Log("H�z 17'ye ayarland�.");
                break;
            case "SlowTrigger2":
                SetSpeed(10f);
                Debug.Log("H�z 10'a ayarland�.");
                break;
            case "SlowTrigger3":
                SetSpeed(5f);
                Debug.Log("H�z 5'e ayarland�.");
                break;
            case "StopTrigger":
                Stop();
                Debug.Log("Tren durduruldu.");
                break;
            default:
                Debug.LogWarning($"Bilinmeyen tetikleyici: {other.tag}");
                break;
        }
    }
}
