using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FleetManagement.Patterns
{
    public class UserRegistrationNotifier
    {
        private readonly List<IObserver> _observers = new();

        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

    public interface IObserver
    {
        void Update(string message);
    }

    public class LogObserver : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }
    }

    public class UiObserver : IObserver
    {
        public void Update(string message)
        {
            MessageBox.Show(message);
        }
    }
}
