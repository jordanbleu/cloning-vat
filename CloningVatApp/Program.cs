using Cloning_Vat.Events;
using Cloning_Vat.Helpers;
using Cloning_Vat.Serialization;
using Cloning_Vat.Serialization.Data;
using Cloning_Vat.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CloningVatApp
{
    public class Program
    {
        private static readonly string configFileName = "CloneInstrutions.xml";

        private static List<string> log = new List<string>();

        private static bool skipUI = false;

        static void Main(string[] args)
        {

            ParseArgs(args);

            Console.WriteLine("Loading and parsing the instructions file...");

            if (!File.Exists(configFileName))
            {
                DisplayConfigFileErrorScreen();
            }
            
            ConfigurationSerializer serializer = new ConfigurationSerializer();
            ConfigurationFile instructions = serializer.LoadFromFile(configFileName);

            if (!skipUI)
            {
                DisplayIntroScreen();


                DisplayConfigSummary(instructions);
                DisplayActionScreen(instructions);
            }
            else
            {
                ExecuteCloning(instructions);
            }
            

            SaveLogs();
        }

        private static void ParseArgs(string[] args)
        {
            skipUI = args.Contains("-skipUI", StringComparer.OrdinalIgnoreCase);
        }

        private static void SaveLogs()
        {
            string filename = $@"Logs\{DateTime.Now.ToString("MM_dd_yyyy.H_m_s_FFF")}.log";
            EasyFileWriter fileWriter = new EasyFileWriter();
            fileWriter.WriteFile(filename, string.Join("\n", log));
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

        

        private static ConfigurationFile DisplayConfigSummary(ConfigurationFile instructions)
        {

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();

            Console.WriteLine("Instructions:");
            Console.WriteLine("Make sure these are correct!");
            Console.WriteLine("All folders that get cloned into (directories with a ->) will be deleted and re-copied.");

            bool valid = true;

            foreach (CloneTask task in instructions.CloneTasks)
            {
                if (!Directory.Exists(task.Source))
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    valid = false;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }


                Console.WriteLine();
                Console.WriteLine(task.Source);

                foreach (CloneDestination destination in task.Destinations)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    if (!Directory.Exists(destination.Path))
                    {
                        Console.WriteLine($"\t -> CREATE ->{destination.Path}");
                    }
                    else
                    {
                        Console.WriteLine($"\t /!\\ -> UPDATE ->{destination.Path}");
                    }
                }
            }

            if (!valid)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("One or more source directories cannot be found.  These are highlighted in red.");
                Console.WriteLine("Please update the CloneInstructions.xml file and try again.");
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();

            if (!valid)
            {
                Environment.Exit(0);
            }


            return instructions;
        }

        private static void ExecuteCloning(ConfigurationFile file)
        {
            CloningVatService service = new CloningVatService(file);
            service.StatusUpdated += Service_StatusUpdated;

            try
            {
                service.ExecuteCloningProcess();
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("There was an error in the cloning process...");
                Console.WriteLine(ex.Message);
                log.Add("<<<EXCEPTION OCCURED>>>");
                log.Add(ex.Message);
                log.Add(ex.StackTrace);
            }

        }

        private static void DisplayActionScreen(ConfigurationFile file)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Clear();

            ExecuteCloning(file);

            Console.WriteLine("Process has completed.");
            Console.WriteLine("Press a key to close.");
            if (!skipUI)
            {
                Console.ReadKey();
            }
        }



        private static void Service_StatusUpdated(object sender, EventArgs e)
        {
            StatusUpdatedEventArgs args = (StatusUpdatedEventArgs)e;
            Console.WriteLine(args.Status);
            log.Add(args.Status);
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
