using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelController : MonoBehaviour
{
    public GameObject upgradePanel; // Upgrade paneli
    public CameraSwitcher cameraSwitcher; // Kamera geçiþinden sorumlu script

    public void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
        }

        if (cameraSwitcher != null)
        {
            cameraSwitcher.ToggleButtonVisibility(false); // Butonu gizle
        }
    }

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        if (cameraSwitcher != null)
        {
            cameraSwitcher.ToggleButtonVisibility(true); // Butonu geri getir
        }
    }
}
