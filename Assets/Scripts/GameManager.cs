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
    public Text coinText; // UI coin g�stergesi

    [Header("UI")]
    public GameObject upgradePanel; // Geli�tirme paneli

    private List<GameObject> wagons = new List<GameObject>(); // Tren vagonlar�

    private void Awake()
    {
        // Singleton kontrol�
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

    // Vagon geli�tirme
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

    // Coin UI g�ncelleme
    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + totalCoins;
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

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            TrainController.Instance.Resume(); // Treni yeniden ba�lat
        }
    }

}
