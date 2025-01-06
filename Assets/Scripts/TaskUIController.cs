using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIController : MonoBehaviour
{
    public static TaskUIController Instance;
    public GameObject taskPanel;       // G�rev paneli
    public GameObject upgradePanel;       // G�rev paneli
    public Button acceptButton;        // Kabul Et butonu
    public Button declineButton;       // ��k butonu (reddetme)
    public Button cameraSwitchButton;  // Kamera de�i�tirme butonu
    public Button backToPanel;   // Kamera ge�i�i i�in
    public TrainController trainController; // Tren kontrol� (tren y�n�n� de�i�tirecek)
    public CameraSwitcher cameraSwitcher;   // Kamera ge�i�i i�in


    void Start()
    {
        taskPanel.SetActive(false); // Ba�lang��ta panel gizli olacak

        // Butonlara i�levsellik ekleyelim
        acceptButton.onClick.AddListener(AcceptTask);
        declineButton.onClick.AddListener(DeclineTask);
    }

    // G�rev kabul edilirse tren 2. yola gidecek
    void AcceptTask()
    {
        // G�rev kabul edildi�inde tren 2. yola gidecek
        trainController.GoToSecondPath();

        // Kamera ge�i� butonunu gizle
        cameraSwitchButton.gameObject.SetActive(false);

        // G�rev panelini kapat
        CloseTaskPanel();

        // Tren hareket etsin
        trainController.ResumeTrain(); // Treni ba�lat
    }

    // G�rev reddedilirse tren d�z devam edecek
    void DeclineTask()
    {
        // Tren d�z devam etsin
        trainController.ContinueStraight();

        // Kamera ge�i� butonunu gizle
        cameraSwitchButton.gameObject.SetActive(false);

        // G�rev panelini kapat
        CloseTaskPanel();

        // Tren hareket etsin
        trainController.ResumeTrain(); // Treni ba�lat
    }

    // G�rev panelini kapatma
    void CloseTaskPanel()
    {
        taskPanel.SetActive(false); // Paneli gizle
    }

    // G�rev panelini a�ma
    public void OpenTaskPanel()
    {
        taskPanel.SetActive(true); // Paneli aktif et
        upgradePanel.SetActive(false); // Paneli aktif et
        backToPanel.gameObject.SetActive(true);

        // Kamera de�i�tirme butonunu gizle
        cameraSwitchButton.gameObject.SetActive(false);
    }

    public void BackToPanel()
    {
        taskPanel.SetActive(false); // Paneli aktif et
        upgradePanel.SetActive(true); // Paneli aktif et
        backToPanel.gameObject.SetActive(false);

        // Kamera de�i�tirme butonunu gizle
        cameraSwitchButton.gameObject.SetActive(true);
    }
}
