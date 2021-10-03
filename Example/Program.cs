using System;
using Clever2D.Core;
using Clever2D.Engine;
using SDL2;

namespace Example
{
    class Program : Clever
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            ApplicationConfig config = new()
            {
                ProductName = "Example Project",
                CompanyName = "Company",
                Version = "0.1.0"
            };

            Application.Config = config;

            OperatingSystem os = System.Environment.OSVersion;

            Clever.OnInitialized += () =>
            {
                Clever.Start(new Scene[] {
                    new MainScene()
                });
            };

            Clever.Initialize(config);
        }
    }
}
