using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TestConsole
{
    public static class MATaskSchedulerTest
    {
        /// <summary>
        /// Generate marketing authorisation xml files based on ma xml template.
        /// Files will be generated in subfolder "Generated" in template folder.
        /// </summary>
        public static void GenerateMarketingAuthorisationXmlFiles(string templateFilePath, int numFiles, int startIndex, string baseName)
        {
            if (!File.Exists(templateFilePath))
            {
                Console.WriteLine("File '{0}' doesn't exists.", templateFilePath);
                Console.ReadLine();
                return;
            }

            if (numFiles < 1)
            {
                Console.WriteLine("Invalid number of files to generate '{0}'. 0 files generated.", numFiles);
                Console.ReadLine();
                return;
            }

            if (startIndex < 1)
            {
                Console.WriteLine("Invalid start index '{0}'. 0 files generated.", startIndex);
                Console.ReadLine();
                return;
            }

            if (baseName.Length < 1 || baseName.Length > 20)
            {
                Console.WriteLine("Base name length must be [1,20] '{0}'. 0 files generated.", baseName);
                Console.ReadLine();
                return;
            }

            string filePath;

            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(templateFilePath)))
            {
                filePath = Path.GetDirectoryName(templateFilePath) + "\\Generated\\";
            }
            else
            {
                filePath = Directory.GetCurrentDirectory() + "\\Generated\\";
            }

            

            if (!Directory.Exists(filePath))
            {
                try
                {
                    Directory.CreateDirectory(filePath);
                }
                catch
                {
                    Console.WriteLine("Can't create directory '{0}'. 0 files generated.", filePath);
                    Console.ReadLine();
                    return;
                }
            }

            string templateData = File.ReadAllText(templateFilePath);

            bool isValidRegNum = false;
            bool isValidRegId = false;

            Match regNumMatch = Regex.Match(templateData, "<registrationnumber>([\\s\\S]*)</registrationnumber>", RegexOptions.IgnoreCase);
            if (regNumMatch.Success && !string.IsNullOrWhiteSpace(regNumMatch.Groups[1].Value))
            {
                isValidRegNum = true;
            }

            Match regIdMatch = Regex.Match(templateData, "<registrationid>([\\s\\S]*)</registrationid>", RegexOptions.IgnoreCase);
            if (regIdMatch.Success && !string.IsNullOrWhiteSpace(regIdMatch.Groups[1].Value))
            {
                isValidRegId = true;
            }

            int fileNameIndex = startIndex;

            int numSuccessfullyGenerated = 0;

            string guid = Guid.NewGuid().ToString("N").Substring(0, 5);

            for (int index = 0; index < numFiles; index++)
            {
                try
                {
                    fileNameIndex = startIndex + index;

                    string filedata = templateData;

                    string registrationnumber = string.Format("{0}_{1}", baseName, guid);
                    string registrationid = fileNameIndex.ToString();

                    if (isValidRegNum)
                    {
                        regNumMatch = Regex.Match(filedata, "(<registrationnumber>[\\s\\S]*</registrationnumber>)", RegexOptions.IgnoreCase);
                        if (regNumMatch.Success)
                        {
                            filedata = filedata.Replace(regNumMatch.Groups[1].Value, "<registrationnumber>" + registrationnumber + "</registrationnumber>");
                        }
                    }

                    if (isValidRegId)
                    {
                        regIdMatch = Regex.Match(filedata, "(<registrationid>[\\s\\S]*</registrationid>)", RegexOptions.IgnoreCase);
                        if (regIdMatch.Success)
                        {
                            filedata = filedata.Replace(regIdMatch.Groups[1].Value, "<registrationid>" + registrationid + "</registrationid>");
                        }
                    }

                    registrationid = fileNameIndex.ToString().PadLeft((startIndex + numFiles).ToString().Length,'0');

                    string filename = string.Format("{0}{1}_{2}.xml", filePath, registrationnumber, registrationid);

                    File.WriteAllText(filename, filedata);

                    numSuccessfullyGenerated++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0} StackTrace: {1}", ex.Message, ex.StackTrace);
                }
            }

            Console.WriteLine("{0}/{1} successfully generated.", numSuccessfullyGenerated, numFiles);
            Console.ReadLine();
        }

        public static void GenerateMarketingAuthorisationAttachments(string templateFilePath, int numFiles, int startIndex, string baseName)
        {
            if (!File.Exists(templateFilePath))
            {
                Console.WriteLine("File '{0}' doesn't exists.", templateFilePath);
                Console.ReadLine();
                return;
            }

            if (numFiles < 1)
            {
                Console.WriteLine("Invalid number of files to generate '{0}'. 0 files generated.", numFiles);
                Console.ReadLine();
                return;
            }

            if (startIndex < 1)
            {
                Console.WriteLine("Invalid start index '{0}'. 0 files generated.", startIndex);
                Console.ReadLine();
                return;
            }

            if (baseName.Length < 1 || baseName.Length > 20)
            {
                Console.WriteLine("Base name length must be [1,20] '{0}'. 0 files generated.", baseName);
                Console.ReadLine();
                return;
            }

            string filePath;

            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(templateFilePath)))
            {
                filePath = Path.GetDirectoryName(templateFilePath) + "\\Generated\\";
            }
            else
            {
                filePath = Directory.GetCurrentDirectory() + "\\Generated\\";
            }

            if (!Directory.Exists(filePath))
            {
                try
                {
                    Directory.CreateDirectory(filePath);
                }
                catch
                {
                    Console.WriteLine("Can't create directory '{0}'. 0 files generated.", filePath);
                    Console.ReadLine();
                    return;
                }
            }

            byte[] filedata = File.ReadAllBytes(templateFilePath);

            int numSuccessfullyGenerated = 0;

            string guid = Guid.NewGuid().ToString("N").Substring(0, 5);

            for (int index = 0; index < numFiles; index++)
            {
                try
                {
                    int fileNameIndex = startIndex + index;

                    string fileNum = fileNameIndex.ToString().PadLeft((startIndex + numFiles).ToString().Length, '0');
                    string extension = Path.GetExtension(templateFilePath);
                    
                    string filename = string.Format("{0}{1}_{2}_{3}{4}", filePath, baseName, guid, fileNum, extension);

                    File.WriteAllBytes(filename, filedata);

                    numSuccessfullyGenerated++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0} StackTrace: {1}", ex.Message, ex.StackTrace);
                }
            }

            Console.WriteLine("{0}/{1} successfully generated.", numSuccessfullyGenerated, numFiles);
            Console.ReadLine();
        }
    }
}
