using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public int upgradeLevel = 0; // Mevcut seviye
    public GameObject[] upgradePrefabs; // Her seviye için prefab
    public int upgradeCost = 50; // Upgrade maliyeti

    public int GetCoins()
    {
        // Seviye 0 = 5 coin, Seviye 1 = 10 coin, Seviye 2 = 15 coin
        return (upgradeLevel + 1) * 5;
    }

    public void UpgradeWagon()
    {
        // Maksimum seviyeye ulaþýldý mý kontrol et
        if (upgradeLevel >= upgradePrefabs.Length - 1)
        {
            Debug.Log("Bu vagon maksimum seviyeye ulaþtý.");
            return;
        }

        // Yeterli para var mý kontrol et
        if (GameManager.Instance.totalCoins >= upgradeCost)
        {
            GameManager.Instance.totalCoins -= upgradeCost; // Coin düþ
            GameManager.Instance.UpdateCoinUI();

            // Mevcut vagonu yok edip yerine yeni prefab koy
            GameObject currentWagon = this.gameObject;

            // Yeni prefab oluþturuluyor
            GameObject newWagon = Instantiate(
                upgradePrefabs[upgradeLevel + 1], // Yeni seviyenin prefabý
                currentWagon.transform.position,
                currentWagon.transform.rotation,
                currentWagon.transform.parent
            );

            // Yeni prefabýn 'Wagon' scriptini al ve upgradeLevel'ý güncelle
            Wagon newWagonScript = newWagon.GetComponent<Wagon>();
            if (newWagonScript != null)
            {
                newWagonScript.upgradeLevel = upgradeLevel + 1; // Yeni seviyeyi ayarla
            }

            // Eski vagonu listeden çýkar ve yenisiyle deðiþtir
            TrainController.Instance.ReplaceWagon(currentWagon, newWagon);

            // Eski vagonu yok et
            Destroy(currentWagon);

            Debug.Log($"Vagon yükseltildi! Yeni Seviye: {newWagonScript.upgradeLevel}, Kalan Coins: {GameManager.Instance.totalCoins}");
        }
        else
        {
            Debug.Log("Yeterli paranýz yok!");
        }
    }
}
