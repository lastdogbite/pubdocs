using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

class Program
{
    static Random rand = new Random();
    static object locker = new object();
    static int folderCount = 0;
    static int subFolderCount = 0;
    static int fileCount = 0;
    static long totalSize = 0;
    static bool isRunning = true;

    static void Main(string[] args)
    {
        // Define root folder and run duration
        string rootFolder = @"C:\temp\_folder2";
        int runDurationMins = 5;

        // Create root folder if it does not exist
        if (!Directory.Exists(rootFolder))
        {
            Directory.CreateDirectory(rootFolder);
        }

        // Start threads
        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < 50; i++)
        {
            Thread t = new Thread(() => GenerateRandomFiles(rootFolder));
            t.Start();
            threads.Add(t);
        }

        // Wait for run duration
        Thread.Sleep(runDurationMins * 60 * 1000);

        // Stop threads
        isRunning = false;
        foreach (Thread t in threads)
        {
            t.Join();
        }

        // Write record to JSON file
        Dictionary<string, object> record = new Dictionary<string, object>();
        record.Add("folderCount", folderCount);
        record.Add("subFolderCount", subFolderCount);
        record.Add("fileCount", fileCount);
        record.Add("totalSize", totalSize);
        string recordJson = JsonConvert.SerializeObject(record, Formatting.Indented);
        File.WriteAllText(Path.Combine(rootFolder, "record.json"), recordJson);

        Console.WriteLine("Done!");
        Console.ReadLine();
    }

    static void GenerateRandomFiles(string path)
    {
        while (isRunning)
        {
            try
            {
                // Generate random folder name
                string folderName = Path.Combine(path, "Folder" + rand.Next(10000));
                lock (locker)
                {
                    Directory.CreateDirectory(folderName);
                    folderCount++;
                }

                // Generate random subfolders
                int subFolderNum = rand.Next(10);
                for (int i = 0; i < subFolderNum; i++)
                {
                    string subFolderName = Path.Combine(folderName, "SubFolder" + rand.Next(10000));
                    lock (locker)
                    {
                        Directory.CreateDirectory(subFolderName);
                        subFolderCount++;
                    }
                }

                // Generate random files
                int fileNum = rand.Next(10);
                for (int i = 0; i < fileNum; i++)
                {
                    string fileName = Path.Combine(folderName, "File" + rand.Next(10000) + ".txt");
                    int fileSizeKB = rand.Next(2 * 1024, 1000 * 1024);
                    long totalBytesWritten = 0;

                    using (StreamWriter writer = File.CreateText(fileName))
                    {
                        while (totalBytesWritten < fileSizeKB * 1024)
                        {
                            int bytesToWrite = rand.Next(1, 1000);
                            byte[] data = new byte[bytesToWrite];
                            rand.NextBytes(data);
                            string text = Convert.ToBase64String(data);
                            writer.WriteLine(text);
                            totalBytesWritten += bytesToWrite;
                        }
                    }

                    lock (locker)
                    {
                        fileCount++;
                        totalSize += fileSizeKB;
                    }

                    // Randomly zip file
                    if (rand.Next(2) == 0)
                    {
                        string zipFileName = Path.ChangeExtension(fileName, ".zip");
                        using (FileStream fs = File.Create(zipFileName))
                        {
                            using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                            {
                                ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(fileName));
                                using (StreamWriter writer = new StreamWriter(entry.Open()))
                                {
                                    using (StreamReader reader = File.OpenText(fileName))
                                    {
                                        string line;
                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            writer.WriteLine(line);
                                        }
                                    }
                                }
                            }
                        }

                        lock (locker)
                        {
                            fileCount++;
                            totalSize += new FileInfo(zipFileName).Length;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Sleep for random time between 100ms and 500ms
            Thread.Sleep(rand.Next(100, 500));
        }
    }
}

