using BetESS;

namespace Sports
{
    public interface Observable
    {
         void Subscribe(Observer o);
         void Unsubscribe(Observer o);
         void NotifyObservers(ObservableEvents obsEvs);
    }
}
