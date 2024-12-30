using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern (isteðe baðlý)


    [Header("Tren ve Vagonlar")]
    public GameObject wagonPrefab; // Vagon prefab'ý
    public Transform wagonsParent; // Vagonlarýn baðlanacaðý ana nesne
    public float wagonSpacing = 2f; // Vagonlar arasýndaki mesafe

    [Header("Oyun Parametreleri")]
    public int totalCoins = 100; // Toplam coin
    public int wagonCost = 50; // Bir vagonun maliyeti
    public Text coinText; // UI coin göstergesi

    [Header("UI")]
    public GameObject upgradePanel; // GameManager içinde tanýmlayýn


    private List<GameObject> wagons = new List<GameObject>(); // Tren vagonlarý

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

    // Coin UI güncelleme
    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "" + totalCoins;
        }
    }

    // Durakta durma paneli açma
    public void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
        }
    }

    public void AddWagon()
    {
        TrainController.Instance.AddWagon(); // TrainController üzerinden yeni vagon ekle
    }

    public void UpgradeWagon()
    {
        TrainController.Instance.UpgradeWagons(); // Mevcut vagonu geliþtir
    }

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false); // Paneli kapat
            TrainController.Instance.Resume(); // Treni yeniden baþlat
        }
    }


    public void AddMultipleWagons()
    {
        while (totalCoins >= wagonCost)
        {
            AddWagon(); // Her bir vagon ekleme iþlemi için mevcut fonksiyon çaðrýlýr
        }
    }
}
