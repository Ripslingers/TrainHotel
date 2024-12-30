using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern (iste�e ba�l�)


    [Header("Tren ve Vagonlar")]
    public GameObject wagonPrefab; // Vagon prefab'�
    public Transform wagonsParent; // Vagonlar�n ba�lanaca�� ana nesne
    public float wagonSpacing = 2f; // Vagonlar aras�ndaki mesafe

    [Header("Oyun Parametreleri")]
    public int totalCoins = 100; // Toplam coin
    public int wagonCost = 50; // Bir vagonun maliyeti
    public Text coinText; // UI coin g�stergesi

    [Header("UI")]
    public GameObject upgradePanel; // GameManager i�inde tan�mlay�n


    private List<GameObject> wagons = new List<GameObject>(); // Tren vagonlar�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    // Coin toplama
    public void CollectCoins()
    {
        int coinsThisCycle = 0;
        foreach (GameObject wagon in wagons)
        {
            Wagon wagonScript = wagon.GetComponent<Wagon>();
            coinsThisCycle += wagonScript.GetCoins();
        }

        totalCoins += coinsThisCycle;
        UpdateCoinUI();
    }

    // Coin UI g�ncelleme
    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "" + totalCoins;
        }
    }

    // Durakta durma paneli a�ma
    public void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
        }
    }

    public void AddWagon()
    {
        TrainController.Instance.AddWagon(); // TrainController �zerinden yeni vagon ekle
    }

    public void UpgradeWagon()
    {
        TrainController.Instance.UpgradeWagons(); // Mevcut vagonu geli�tir
    }

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false); // Paneli kapat
            TrainController.Instance.Resume(); // Treni yeniden ba�lat
        }
    }


    public void AddMultipleWagons()
    {
        while (totalCoins >= wagonCost)
        {
            AddWagon(); // Her bir vagon ekleme i�lemi i�in mevcut fonksiyon �a�r�l�r
        }
    }
}
