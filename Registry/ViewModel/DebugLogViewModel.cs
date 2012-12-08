using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Registry.ViewModel
{
    public sealed class DebugLogViewModel : ClosableViewModel, IDisposable
    {
        private static DebugLogViewModel _instance;
        private Listener _listener;

        class Listener : TraceListener
        {
            internal ObservableCollection<Tuple<DateTime, string>> _messages = new ObservableCollection<Tuple<DateTime, string>>();
            
            public override void Write(string message)
            {
                _messages.Add(Tuple.Create(DateTime.Now, message));
            }

            public override void WriteLine(string message)
            {
                _messages.Add(Tuple.Create(DateTime.Now, message));
            }

            public string GetText()
            {
                string text = "";
                foreach (var msg in _messages)
                    text += msg.Item1 + " " + msg.Item2 + "\n";
                return text;
            }
        }

        DebugLogViewModel()
        {
            _listener = new Listener();
            _listener._messages.CollectionChanged += (s, e) => OnPropertyChanged("Text");
        }

        public override string DisplayName
        {
            get { return "Debug"; }
        }

        public static DebugLogViewModel Instance
        {
            get {
                if(_instance == null)
                    _instance = new DebugLogViewModel();
                return _instance;
            }
        }

        public string Text
        {
            get {
                return _listener.GetText();
            }
        }

        public void StartListening()
        {
            Debug.Listeners.Add(_listener);
            Debug.Print("Started listening debug log");
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}
