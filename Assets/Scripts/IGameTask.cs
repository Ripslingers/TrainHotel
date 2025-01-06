public interface IGameTask
{
    void AcceptTask(); // Görevi kabul etme
    void DeclineTask(); // Görevi reddetme
    bool IsTaskAccepted { get; } // Görev kabul edildi mi?
    void NotifyObservers(); // Gözlemcilere bildirim gönder
}
