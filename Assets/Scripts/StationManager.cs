using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public GameObject stationPanel; // Durak UI Paneli
    public TrainMovement trainMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StationTrigger"))
        {
            Debug.Log("Train entered StationTrigger.");
            trainMovement.SlowDown();
        }
        else if (other.CompareTag("StopStationTrigger"))
        {
            Debug.Log("Train entered StopStationTrigger.");
            trainMovement.Stop();
            stationPanel.SetActive(true);
            Time.timeScale = 0f; // Oyunu durdur
        }
    }

    public void ResumeGame()
    {
        stationPanel.SetActive(false);
        Time.timeScale = 1f; // Oyunu baþlat
        trainMovement.Resume();
    }
}
