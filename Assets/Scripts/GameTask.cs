using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour, IGameTask
{
    public bool IsTaskAccepted { get; private set; } = false; // Görev kabul edilip edilmediðini kontrol eder
    private List<IGameTaskObserver> observers = new List<IGameTaskObserver>();

    // Görev kabul edildiðinde yapýlacak iþlemler
    public void AcceptTask()
    {
        IsTaskAccepted = true;
        Debug.Log("Görev kabul edildi, ikinci yolu tercih ediyorum.");
        NotifyObservers(); // Gözlemcilere bildirim gönder
    }

    // Görev reddedildiðinde yapýlacak iþlemler
    public void DeclineTask()
    {
        IsTaskAccepted = false;
        Debug.Log("Görev reddedildi, düz yolda ilerliyorum.");
        NotifyObservers(); // Gözlemcilere bildirim gönder
    }

    // Gözlemcileri kaydetme ve bildirme
    public void AddObserver(IGameTaskObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IGameTaskObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.UpdateTaskStatus(this);
        }
    }
}
