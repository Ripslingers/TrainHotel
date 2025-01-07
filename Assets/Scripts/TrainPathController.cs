using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPathController : MonoBehaviour, IGameTaskObserver
{
    private bool isTaskAccepted = false;

    private void Start()
    {
        // Eðer tren yolu ve görev oluþturulmuþsa, bu gözlemciyi ekle
        GameTask task = FindObjectOfType<GameTask>(); // Varsayýlan olarak bir görev var
        if (task != null)
        {
            task.AddObserver(this); // Gözlemci olarak kendimizi ekliyoruz
        }
    }

    // Görev durumu deðiþtiðinde çaðrýlýr
    public void UpdateTaskStatus(IGameTask task)
    {
        if (task.IsTaskAccepted)
        {
            // Görev kabul edildiyse ikinci yolu tercih et
            isTaskAccepted = true;
            Debug.Log("Görev kabul edildi, ikinci yol tercih edilecek.");
            // Tren yolunu deðiþtirme kodu burada olacak
            ChangeToSecondPath();
        }
        else
        {
            // Görev reddedildiyse düz yola devam et
            isTaskAccepted = false;
            Debug.Log("Görev reddedildi, düz yola devam edilecek.");
            // Tren yolunu deðiþtirme kodu burada olacak
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
