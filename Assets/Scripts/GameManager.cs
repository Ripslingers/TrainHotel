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
    public Text coinText; // UI coin göstergesi

    [Header("UI")]
    public GameObject upgradePanel; // Geliþtirme paneli

    private List<GameObject> wagons = new List<GameObject>(); // Tren vagonlarý

    private void Awake()
    {
        // Singleton kontrolü
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

    // Vagon ekleme
    public void AddWagon()
    {
        int wagonCount = wagons.Count;
        Vector3 newPosition = wagonsParent.position + Vector3.back * wagonSpacing * (wagonCount + 1);

        GameObject newWagon = Instantiate(wagonPrefab, newPosition, Quaternion.identity, wagonsParent);
        wagons.Add(newWagon);
    }

    // Vagon geliþtirme
    public void UpgradeWagon()
    {
        foreach (GameObject wagon in wagons)
        {
            Wagon wagonScript = wagon.GetComponent<Wagon>();
            if (wagonScript.upgradeLevel < 3)
            {
                wagonScript.UpgradeWagon();
                return;
            }
        }
    }

    // Coin UI güncelleme
    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + totalCoins;
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

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            TrainController.Instance.Resume(); // Treni yeniden baþlat
        }
    }

}
