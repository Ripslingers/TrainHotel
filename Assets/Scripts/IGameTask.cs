public interface IGameTask
{
    void AcceptTask(); // G�revi kabul etme
    void DeclineTask(); // G�revi reddetme
    bool IsTaskAccepted { get; } // G�rev kabul edildi mi?
    void NotifyObservers(); // G�zlemcilere bildirim g�nder
}
