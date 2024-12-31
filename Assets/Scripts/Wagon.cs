using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public int upgradeLevel = 0; // Mevcut seviye
    public GameObject[] upgradePrefabs; // Her seviye i�in prefab
    public int upgradeCost = 50; // Upgrade maliyeti

    public int GetCoins()
    {
        // Seviye 0 = 5 coin, Seviye 1 = 10 coin, Seviye 2 = 15 coin
        return (upgradeLevel + 1) * 5;
    }

    public void UpgradeWagon()
    {
        // Maksimum seviyeye ula��ld� m� kontrol et
        if (upgradeLevel >= upgradePrefabs.Length - 1)
        {
            Debug.Log("Bu vagon maksimum seviyeye ula�t�.");
            return;
        }

        // Yeterli para var m� kontrol et
        if (GameManager.Instance.totalCoins >= upgradeCost)
        {
            GameManager.Instance.totalCoins -= upgradeCost; // Coin d��
            GameManager.Instance.UpdateCoinUI();

            // Mevcut vagonu yok edip yerine yeni prefab koy
            GameObject currentWagon = this.gameObject;

            // Yeni prefab olu�turuluyor
            GameObject newWagon = Instantiate(
                upgradePrefabs[upgradeLevel + 1], // Yeni seviyenin prefab�
                currentWagon.transform.position,
                currentWagon.transform.rotation,
                currentWagon.transform.parent
            );

            // Yeni prefab�n 'Wagon' scriptini al ve upgradeLevel'� g�ncelle
            Wagon newWagonScript = newWagon.GetComponent<Wagon>();
            if (newWagonScript != null)
            {
                newWagonScript.upgradeLevel = upgradeLevel + 1; // Yeni seviyeyi ayarla
            }

            // Eski vagonu listeden ��kar ve yenisiyle de�i�tir
            TrainController.Instance.ReplaceWagon(currentWagon, newWagon);

            // Eski vagonu yok et
            Destroy(currentWagon);

            Debug.Log($"Vagon y�kseltildi! Yeni Seviye: {newWagonScript.upgradeLevel}, Kalan Coins: {GameManager.Instance.totalCoins}");
        }
        else
        {
            Debug.Log("Yeterli paran�z yok!");
        }
    }
}
