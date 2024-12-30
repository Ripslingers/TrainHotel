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
        return (upgradeLevel + 1) * 2; // �rne�in: Seviye 1 = 2 coin
    }

    public void UpgradeWagon()
    {
        if (upgradeLevel >= upgradePrefabs.Length - 1)
        {
            Debug.Log("Bu vagon maksimum seviyeye ula�t�.");
            return;
        }

        if (GameManager.Instance.totalCoins >= upgradeCost)
        {
            GameManager.Instance.totalCoins -= upgradeCost;
            GameManager.Instance.UpdateCoinUI();

            GameObject currentWagon = gameObject;

            GameObject newWagon = Instantiate(
                upgradePrefabs[upgradeLevel + 1],
                currentWagon.transform.position,
                currentWagon.transform.rotation,
                currentWagon.transform.parent
            );

            TrainController.Instance.ReplaceWagon(this.gameObject, newWagon.gameObject);

            Destroy(currentWagon);
        }
        else
        {
            Debug.Log("Yeterli paran�z yok!");
        }
    }
}
