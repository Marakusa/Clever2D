using System;
using Clever2D.Core;
using Clever2D.Engine;
using Eto.Drawing;
using Eto.Forms;

namespace Example
{
    class Program : Clever
    {
        [STAThread]
        static void Main(string[] args)
        {
            Player.Log("Starting...");

            Time.Initialize();

            ApplicationConfig config = new()
            {
                ProductName = "Example Project",
                CompanyName = "Company",
                Version = "0.1.0"
            };

            Clever2D.Engine.Application.Config = config;

            OperatingSystem os = System.Environment.OSVersion;

            Clever clever = new();
            clever.OnInitialized += () =>
            {
                clever.Start(new Scene[] {
                    new MainScene()
                });
            };

            clever.Initialize(config);
        }
    }
}
