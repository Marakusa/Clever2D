using System;
using System.Threading.Tasks;
using System.IO;
using Clever2D.Engine;
using Gtk;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using UI = Gtk.Builder.ObjectAttribute;

namespace CleverInstaller
{
    class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;
        [UI] private ProgressBar _progress1 = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Gtk.Application.Quit();
        }

        private async void Button1_Clicked(object sender, EventArgs a)
        {
            progressHandler += ProgressChanged;
            completedHandler += Completed;
            await Task.Run(() =>
            {
                DirectoryInfo dir = new DirectoryInfo("bin/tmp/latest");
                if (dir.Exists) dir.Delete(true);
                
                Repository.Clone("https://github.com/Marakusa/Clever2D.git", "bin/tmp/latest", new CloneOptions()
                {
                    BranchName = "development",
                    OnTransferProgress = progressHandler,
                    RepositoryOperationCompleted = completedHandler
                });
            });
            _counter++;
            _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
            _progress1.Fraction = _counter / 100.00;
        }

        private RepositoryOperationCompleted completedHandler;
        private void Completed(RepositoryOperationContext c)
        {
            Player.Log("Completed");
        }
        
        private TransferProgressHandler progressHandler;
        private bool ProgressChanged(TransferProgress t)
        {
            float received = t.ReceivedObjects;
            float total = t.TotalObjects;
            
            Player.Log(received.ToString());
            Player.Log(total.ToString());
            //Player.Log(Math.Round(received / total * 100f).ToString());
            _progress1.Fraction = (received / total);
            return true;
        }
    }
}