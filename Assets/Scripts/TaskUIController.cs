using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIController : MonoBehaviour
{
    public static TaskUIController Instance;
    public GameObject taskPanel;       // Görev paneli
    public GameObject upgradePanel;       // Görev paneli
    public Button acceptButton;        // Kabul Et butonu
    public Button declineButton;       // Çýk butonu (reddetme)
    public Button cameraSwitchButton;  // Kamera deðiþtirme butonu
    public Button backToPanel;   // Kamera geçiþi için
    public TrainController trainController; // Tren kontrolü (tren yönünü deðiþtirecek)
    public CameraSwitcher cameraSwitcher;   // Kamera geçiþi için


    void Start()
    {
        taskPanel.SetActive(false); // Baþlangýçta panel gizli olacak

        // Butonlara iþlevsellik ekleyelim
        acceptButton.onClick.AddListener(AcceptTask);
        declineButton.onClick.AddListener(DeclineTask);
    }

    // Görev kabul edilirse tren 2. yola gidecek
    void AcceptTask()
    {
        // Görev kabul edildiðinde tren 2. yola gidecek
        trainController.GoToSecondPath();

        // Kamera geçiþ butonunu gizle
        cameraSwitchButton.gameObject.SetActive(false);

        // Görev panelini kapat
        CloseTaskPanel();

        // Tren hareket etsin
        trainController.ResumeTrain(); // Treni baþlat
    }

    // Görev reddedilirse tren düz devam edecek
    void DeclineTask()
    {
        // Tren düz devam etsin
        trainController.ContinueStraight();

        // Kamera geçiþ butonunu gizle
        cameraSwitchButton.gameObject.SetActive(false);

        // Görev panelini kapat
        CloseTaskPanel();

        // Tren hareket etsin
        trainController.ResumeTrain(); // Treni baþlat
    }

    // Görev panelini kapatma
    void CloseTaskPanel()
    {
        taskPanel.SetActive(false); // Paneli gizle
    }

    // Görev panelini açma
    public void OpenTaskPanel()
    {
        taskPanel.SetActive(true); // Paneli aktif et
        upgradePanel.SetActive(false); // Paneli aktif et
        backToPanel.gameObject.SetActive(true);

        // Kamera deðiþtirme butonunu gizle
        cameraSwitchButton.gameObject.SetActive(false);
    }

    public void BackToPanel()
    {
        taskPanel.SetActive(false); // Paneli aktif et
        upgradePanel.SetActive(true); // Paneli aktif et
        backToPanel.gameObject.SetActive(false);

        // Kamera deðiþtirme butonunu gizle
        cameraSwitchButton.gameObject.SetActive(true);
    }
}
