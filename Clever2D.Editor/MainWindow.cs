using System;
using System.Threading.Tasks;
using System.IO;
using Clever2D.Engine;
using Gtk;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using UI = Gtk.Builder.ObjectAttribute;

namespace Clever2D.Editor
{
    class MainWindow : Window
    {
        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Gtk.Application.Quit();
        }
    }
}