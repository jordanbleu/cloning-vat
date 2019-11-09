using Cloning_Vat.Helpers;
using Cloning_Vat.Serialization;
using Cloning_Vat.Serialization.Data;
using Cloning_Vat.Service;
using System;
using System.IO;

namespace CloningVatApp
{
    public class Program
    {
        private static readonly string configFileName = "CloneInstrutions.xml";

        static void Main(string[] args)
        {
            DisplayIntroScreen();

            if (!File.Exists(configFileName))
            {
                DisplayConfigFileErrorScreen();
            }

            ConfigurationFile file = DisplayConfigSummary();

            DisplayActionScreen(file);
        }

        private static void DisplayIntroScreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine(@"  ______ _             _                _    _          ");
            Console.WriteLine(@" / _____) |           (_)              | |  | |    _    ");
            Console.WriteLine(@"| /     | | ___  ____  _ ____   ____ __| |  | |___| |_  ");
            Console.WriteLine(@"| |     | |/ _ \|  _ \| |  _ \ / _  (___) \/ / _  |  _) ");
            Console.WriteLine(@"| \_____| | |_| | | | | | | | ( ( | |    \  ( ( | | |__ ");
            Console.WriteLine(@" \______)_|\___/|_| |_|_|_| |_|\_|| |     \/ \_||_|\___)");
            Console.WriteLine(@"                              (_____|           v0.0.1  ");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Written By Jordan Bleu");
            Console.WriteLine("https://github.com/jordanbleu/cloning-vat");
            Console.WriteLine();
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void DisplayConfigFileErrorScreen()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Clear();

            Console.WriteLine("Looks like you're missing the CloneInstructions.xml file in the ");
            Console.WriteLine("Executable's root directory.  You're gonna need that.  I went ahead and ");
            Console.WriteLine("Just created an example file for you, so feel free to modify that and try again.");
            Console.WriteLine();

            CreateExampleXmlFile();

            Console.WriteLine("Press a key to close");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static ConfigurationFile DisplayConfigSummary()
        {
            Console.WriteLine("Loading and parsing the instructions file...");

            ConfigurationSerializer serializer = new ConfigurationSerializer();
            ConfigurationFile instructions = serializer.LoadFromFile(configFileName);

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();

            Console.WriteLine("Instructions:");
            Console.WriteLine("Make sure these are correct!");
            Console.WriteLine("All folders that get cloned into (directories with a ->) will be deleted and re-copied.");
            
            foreach (CloneTask task in instructions.CloneTasks)
            {
                Console.WriteLine();
                Console.WriteLine(task.Source);

                foreach (CloneDestination destination in task.Destinations)
                {
                    Console.WriteLine($"\t ->{destination.Path}");
                }
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
            return instructions;
        }

        private static void DisplayActionScreen(ConfigurationFile file)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Magenta;

            CloningVatService service = new CloningVatService(file);
            service.StatusUpdated += Service_StatusUpdated;

            
        }

        private static void Service_StatusUpdated(object sender, EventArgs e)
        {
                        
        }

        /// <summary>
        /// Copies the embedded example xml into a new file 
        /// </summary>
        private static void CreateExampleXmlFile()
        {
            string exampleXml = LoadExampleXmlFile();
            EasyFileWriter easyFileWriter = new EasyFileWriter();
            easyFileWriter.WriteFile($"{configFileName}.example", exampleXml);
        }


        /// <summary>
        /// Reads the embedded xml into a string
        /// </summary>
        /// <returns></returns>
        private static string LoadExampleXmlFile()
        {
            EmbeddedResourceReader embeddedResourceReader = new EmbeddedResourceReader();
            return embeddedResourceReader.ReadFile("XML.example.xml");
        }
    }
}
