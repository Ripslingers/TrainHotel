using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPathController : MonoBehaviour, IGameTaskObserver
{
    private bool isTaskAccepted = false;

    private void Start()
    {
        // E�er tren yolu ve g�rev olu�turulmu�sa, bu g�zlemciyi ekle
        GameTask task = FindObjectOfType<GameTask>(); // Varsay�lan olarak bir g�rev var
        if (task != null)
        {
            task.AddObserver(this); // G�zlemci olarak kendimizi ekliyoruz
        }
    }

    // G�rev durumu de�i�ti�inde �a�r�l�r
    public void UpdateTaskStatus(IGameTask task)
    {
        if (task.IsTaskAccepted)
        {
            // G�rev kabul edildiyse ikinci yolu tercih et
            isTaskAccepted = true;
            Debug.Log("G�rev kabul edildi, ikinci yol tercih edilecek.");
            // Tren yolunu de�i�tirme kodu burada olacak
            ChangeToSecondPath();
        }
        else
        {
            // G�rev reddedildiyse d�z yola devam et
            isTaskAccepted = false;
            Debug.Log("G�rev reddedildi, d�z yola devam edilecek.");
            // Tren yolunu de�i�tirme kodu burada olacak
            ContinueStraightPath();
        }
    }

    private void ChangeToSecondPath()
    {
        TrainController.Instance.GoToSecondPath();
    }

    private void ContinueStraightPath()
    {
        TrainController.Instance.ContinueStraight();
    }

}
