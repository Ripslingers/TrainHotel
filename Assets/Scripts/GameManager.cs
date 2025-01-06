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
    public int totalCoins = 0; // Toplam coin
    public int wagonCost = 50; // Bir vagonun maliyeti
    public Text coinText; // UI coin göstergesi

    [Header("UI")]
    public GameObject upgradePanel; // GameManager içinde tanýmlayýn
    public GameObject taskPanel; // GameManager içinde tanýmlayýn


    [Header("Oyun Baþlatma")]
    public GameObject startButton; // Baþlat butonu referansý
    private bool isGameStarted = false; // Oyunun baþlatýlýp baþlatýlmadýðýný takip eder


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
        TrainController.Instance.Stop(); // Baþlangýçta tren durur
        isGameStarted = false; // Oyun baþlamadý
        UpdateCoinUI();
        StartCoroutine(CollectCoinsPeriodically()); // Coin toplama coroutine'i çalýþtýrýlýr
    }

    // Coin toplama
    public void CollectCoins()
    {
        int coinsThisCycle = 1; // Tren için sabit coin (vagonsuz tren için)

        foreach (GameObject wagon in TrainController.Instance.wagons)
        {
            Wagon wagonScript = wagon.GetComponent<Wagon>();
            if (wagonScript != null)
            {
                coinsThisCycle += wagonScript.GetCoins();
            }
        }

        totalCoins += coinsThisCycle;
        UpdateCoinUI();

        // Coin toplama iþlemini logla
    }

    // Coin UI güncelleme
    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = totalCoins.ToString();
        }
        else
        {
            Debug.LogWarning("coinText is not assigned in the Inspector!");
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
        // Eðer yeterli coin yoksa iþlem yapma
        if (totalCoins < wagonCost)
        {
            Debug.Log("Yeterli paranýz yok!");
            return;
        }

        // Coin düþme iþlemi
        totalCoins -= wagonCost;

        // TrainController'da yeni vagon ekleme iþlemini çaðýr
        TrainController.Instance.AddWagon();

        // Yeni maliyet hesaplama (her vagon ekleme sonrasý artýþ istiyorsanýz)
        wagonCost += Mathf.RoundToInt(wagonCost * 0.1f); // %10 artýþ örneði

        // Coin UI'sini güncelle
        UpdateCoinUI();

        Debug.Log($"Yeni vagon eklendi! Kalan para: {totalCoins}, Yeni vagon maliyeti: {wagonCost}");
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

    public void CloseTaskPanel()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(false); // Paneli kapat
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

    public void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true; // Oyun baþlatýldý
            if (startButton != null)
            {
                startButton.SetActive(false); // Baþlat butonunu gizle
            }
            TrainController.Instance.Resume(); // Treni baþlat
        }
    }

    private IEnumerator CollectCoinsPeriodically()
    {
        while (true)
        {
            if (isGameStarted)
            {
                CollectCoins();
            }
            yield return new WaitForSeconds(1); // 5 saniyede bir çalýþýr
        }
    }
}
