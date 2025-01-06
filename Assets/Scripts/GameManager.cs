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
    public int totalCoins = 0; // Toplam coin
    public int wagonCost = 50; // Bir vagonun maliyeti
    public Text coinText; // UI coin g�stergesi

    [Header("UI")]
    public GameObject upgradePanel; // GameManager i�inde tan�mlay�n
    public GameObject taskPanel; // GameManager i�inde tan�mlay�n


    [Header("Oyun Ba�latma")]
    public GameObject startButton; // Ba�lat butonu referans�
    private bool isGameStarted = false; // Oyunun ba�lat�l�p ba�lat�lmad���n� takip eder


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
        TrainController.Instance.Stop(); // Ba�lang��ta tren durur
        isGameStarted = false; // Oyun ba�lamad�
        UpdateCoinUI();
        StartCoroutine(CollectCoinsPeriodically()); // Coin toplama coroutine'i �al��t�r�l�r
    }

    // Coin toplama
    public void CollectCoins()
    {
        int coinsThisCycle = 1; // Tren i�in sabit coin (vagonsuz tren i�in)

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

        // Coin toplama i�lemini logla
    }

    // Coin UI g�ncelleme
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
        // E�er yeterli coin yoksa i�lem yapma
        if (totalCoins < wagonCost)
        {
            Debug.Log("Yeterli paran�z yok!");
            return;
        }

        // Coin d��me i�lemi
        totalCoins -= wagonCost;

        // TrainController'da yeni vagon ekleme i�lemini �a��r
        TrainController.Instance.AddWagon();

        // Yeni maliyet hesaplama (her vagon ekleme sonras� art�� istiyorsan�z)
        wagonCost += Mathf.RoundToInt(wagonCost * 0.1f); // %10 art�� �rne�i

        // Coin UI'sini g�ncelle
        UpdateCoinUI();

        Debug.Log($"Yeni vagon eklendi! Kalan para: {totalCoins}, Yeni vagon maliyeti: {wagonCost}");
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

    public void CloseTaskPanel()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(false); // Paneli kapat
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

    public void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true; // Oyun ba�lat�ld�
            if (startButton != null)
            {
                startButton.SetActive(false); // Ba�lat butonunu gizle
            }
            TrainController.Instance.Resume(); // Treni ba�lat
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
            yield return new WaitForSeconds(1); // 5 saniyede bir �al���r
        }
    }
}
