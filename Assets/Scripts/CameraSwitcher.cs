using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Kullanacaðýnýz kameralarý buraya atayýn
    private int currentCameraIndex = 0; // Hangi kameranýn aktif olduðunu takip eder
    public GameObject switchButton; // Kamera geçiþ butonu
    public GameObject upgradePanel; // Upgrade paneli

    void Start()
    {
        // Ýlk kamerayý aktif yap, diðerlerini kapat
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }

        // Upgrade paneli açýkken butonu gizle
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }

    public void SwitchCamera()
    {
        // Aktif kamerayý kapat
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Sonraki kameraya geç
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Yeni kamerayý aktif yap
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
