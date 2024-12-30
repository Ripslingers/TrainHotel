using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public int upgradeLevel = 0; // Mevcut geliþtirme seviyesi
    public int[] coinPerUpgrade = { 2, 4, 6, 10 }; // Seviyelere göre kazanç

    public int GetCoins()
    {
        return coinPerUpgrade[upgradeLevel];
    }

    public void UpgradeWagon()
    {
        if (upgradeLevel < coinPerUpgrade.Length - 1)
        {
            upgradeLevel++;
        }
    }
}
