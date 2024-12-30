using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Kullanaca��n�z kameralar� buraya atay�n
    private int currentCameraIndex = 0; // Hangi kameran�n aktif oldu�unu takip eder
    public GameObject switchButton; // Kamera ge�i� butonu
    public GameObject upgradePanel; // Upgrade paneli

    void Start()
    {
        // �lk kameray� aktif yap, di�erlerini kapat
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }

        // Upgrade paneli a��kken butonu gizle
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }

    public void SwitchCamera()
    {
        // Aktif kameray� kapat
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Sonraki kameraya ge�
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Yeni kameray� aktif yap
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }

    public void ToggleButtonVisibility(bool isVisible)
    {
        if (switchButton != null)
        {
            switchButton.SetActive(isVisible);
        }
    }
}
