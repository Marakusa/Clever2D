using System;
using Clever2D.Core;
using Clever2D.Engine;

namespace Example
{
    class Program : Clever
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Time.Initialize();

            ApplicationConfig config = new()
            {
                ProductName = "Example Project",
                CompanyName = "Company",
                Version = "0.1.0"
            };

            Application.Config = config;

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
