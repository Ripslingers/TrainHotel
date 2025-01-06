using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour, IGameTask
{
    public bool IsTaskAccepted { get; private set; } = false; // G�rev kabul edilip edilmedi�ini kontrol eder
    private List<IGameTaskObserver> observers = new List<IGameTaskObserver>();

    // G�rev kabul edildi�inde yap�lacak i�lemler
    public void AcceptTask()
    {
        IsTaskAccepted = true;
        Debug.Log("G�rev kabul edildi, ikinci yolu tercih ediyorum.");
        NotifyObservers(); // G�zlemcilere bildirim g�nder
    }

    // G�rev reddedildi�inde yap�lacak i�lemler
    public void DeclineTask()
    {
        IsTaskAccepted = false;
        Debug.Log("G�rev reddedildi, d�z yolda ilerliyorum.");
        NotifyObservers(); // G�zlemcilere bildirim g�nder
    }

    // G�zlemcileri kaydetme ve bildirme
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
