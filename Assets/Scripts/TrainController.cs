using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float speed = 25f;       // Normal hýz
    public float slowSpeed = 5f;   // Yavaþ hýz
    private bool isSlowingDown = false; // Yavaþlama durumu
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

    // Hýz ayarý
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        isSlowingDown = false; // Hýz doðrudan ayarlandýðýnda yavaþlama iptal edilir
    }

    // Tren yavaþlar
    public void SlowDown()
    {
        isSlowingDown = true;
    }

    // Tren durur
    public void Stop()
    {
        isStopped = true;
    }

    // Tren yeniden baþlar
    public void Resume()
    {
        isStopped = false;
        isSlowingDown = false;
    }

    // Tetikleyiciye göre hýz ayarý
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "SlowTrigger1":
                SetSpeed(17f);
                Debug.Log("Hýz 17'ye ayarlandý.");
                break;
            case "SlowTrigger2":
                SetSpeed(10f);
                Debug.Log("Hýz 10'a ayarlandý.");
                break;
            case "SlowTrigger3":
                SetSpeed(5f);
                Debug.Log("Hýz 5'e ayarlandý.");
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
