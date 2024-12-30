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
        return (upgradeLevel + 1) * 2; // Örneðin: Seviye 1 = 2 coin
    }

    public void UpgradeWagon()
    {
        if (upgradeLevel >= upgradePrefabs.Length - 1)
        {
            Debug.Log("Bu vagon maksimum seviyeye ulaþtý.");
            return;
        }

        if (GameManager.Instance.totalCoins >= upgradeCost)
        {
            GameManager.Instance.totalCoins -= upgradeCost;
            GameManager.Instance.UpdateCoinUI();

            GameObject currentWagon = this.gameObject;

            GameObject newWagon = Instantiate(
                upgradePrefabs[upgradeLevel + 1],
                currentWagon.transform.position,
                currentWagon.transform.rotation,
                currentWagon.transform.parent
            );

            TrainController.Instance.ReplaceWagon(currentWagon, newWagon);

            Destroy(currentWagon);

            upgradeLevel++; // Upgrade seviyesini artýr
        }
        else
        {
            Debug.Log("Yeterli paranýz yok!");
        }
    }
}
