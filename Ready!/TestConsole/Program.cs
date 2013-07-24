using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExecuteMATaskSchedulerTest();
            ExecuteEVGatewayWinServiceTest();
        }

        static void ExecuteMATaskSchedulerTest()
        {
            const bool useDefault = false;

            //Default settings
            string templateFilePath = @"C:\TEST\ma.xml";
            int numFiles = 1000;
            int startIndex = 1;
            string baseName = "ma_01";

            if (!useDefault)
            {
                string input;
                Console.Write("Template file path: ");
                input = Console.ReadLine();
                templateFilePath = input;

                Console.Write("Number of files: ");
                input = Console.ReadLine();
                if (!int.TryParse(input, out numFiles))
                {
                    Console.WriteLine("Invalid number!");
                    Console.ReadLine();
                    return;
                }

                Console.Write("Start index: ");
                input = Console.ReadLine();
                if (!int.TryParse(input, out startIndex))
                {
                    Console.WriteLine("Invalid number!");
                    Console.ReadLine();
                    return;
                }

                Console.Write("Base name: ");
                input = Console.ReadLine();
                baseName = input;
            }

            //MATaskSchedulerTest.GenerateMarketingAuthorisationXmlFiles(templateFilePath, numFiles, startIndex, baseName);
            MATaskSchedulerTest.GenerateMarketingAuthorisationAttachments(templateFilePath, numFiles, startIndex, baseName);
        }

        static void ExecuteEVGatewayWinServiceTest()
        {
            while (true)
            {
                Console.WriteLine("1 - Send xevprm message");
                Console.WriteLine("2 - Process received message");
                Console.WriteLine("q - Quit\n");
                var key = Console.ReadKey(false);

                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
                {
                    Console.WriteLine();
                    Console.Write("Xevprm message Pk: ");
                    var input = Console.ReadLine();

                    int pk;

                    if (!int.TryParse(input, out pk))
                    {
                        Console.WriteLine("'{0}' is not valid integer!", input);
                        continue;
                    }

                    EVGatewayWinServiceTest.SubmitXevprmMessages(pk);
        
                    Console.WriteLine("Xevprm message submitted.");
                }
                else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
                {
                    Console.WriteLine();
                    Console.Write("Received message Pk: ");
                    var input = Console.ReadLine();

                    int pk;

                    if (!int.TryParse(input, out pk))
                    {
                        Console.WriteLine("'{0}' is not valid integer!", input);
                        continue;
                    }

                    EVGatewayWinServiceTest.ProcessReceivedMessages(pk);

                    Console.WriteLine("Received message processed.");
                }
                else if (key.Key == ConsoleKey.Q)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(false);
                Console.WriteLine();
            }
        }
    }
}
