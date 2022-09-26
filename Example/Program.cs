using System;
using System.IO;
using Clever2D.Core;
using Clever2D.Engine;
using Newtonsoft.Json;

namespace Example
{
    public class Program : Clever
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            ApplicationConfig config = new()
            {
                ProductName = "Example Project",
                CompanyName = "Company",
                Version = "0.1.0"
            };

            Application.Config = config;

            OnInitialized += () =>
            {
                string a = File.ReadAllText($"{Application.ExecutableDirectory}/assets/scenes/MainScene.json");
                Scene scene = JsonConvert.DeserializeObject<Scene>(a, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                
                Scene[] scenes = {
                    scene
                };

                Start(scenes);
            };

            Initialize(config);
        }
    }
}
