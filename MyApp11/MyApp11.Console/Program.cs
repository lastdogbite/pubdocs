using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;
using MyApp11.Console;
using MySqlX.XDevAPI.Common;

class Program
{
    static Random rand = new Random();
    static object locker = new object();      
    static int fileCount = 0;
    static long totalSize = 0;
    

    static void Main(string[] args)
    {
        // Define root folder and run duration
        string rootFolder = @"C:\temp\_folder5";
        

        // Create root folder if it does not exist
        if (!Directory.Exists(rootFolder))
        {
            Directory.CreateDirectory(rootFolder);
            GenFoldersFiles.CreateRandomFolders(rootFolder, 100, 10);
        }

  


        string rootDirectory = rootFolder;
        int totalThreads = 50;
        long totalSizeInBytes = 40L * 1024 * 1024 * 1024; // 40 GB
        long targetFileSizeInBytes = totalSizeInBytes / totalThreads;

        for (int i = 0; i < 50; i++)
        {
            Thread thread = new Thread(() =>
            {
                while (totalSize < totalSizeInBytes)
                {
                    Random oRand = new Random();    
                    int rand = oRand.Next(0, 20);
                    string filePath; long fileSize;
                    
                    if (rand == 9) (filePath, fileSize) = GenFoldersFiles.AddRandomFile(rootDirectory, 1000000); //1gb
                    else if (rand == 8 || rand == 7 || rand == 13) (filePath, fileSize) = GenFoldersFiles.AddRandomFile(rootDirectory, 100000); //100mb
                    else (filePath, fileSize) = GenFoldersFiles.AddRandomFile(rootDirectory); //10mb

                    lock (locker) { totalSize += fileSize; fileCount++; }
                    Console.WriteLine("TotalBytes: {0}:{1}:{2}\nFileCount: {3}\nPath:{4}\n", fileSize, totalSize, totalSizeInBytes, fileCount, filePath);                    
                }
            });
            thread.Name = i.ToString();
            // Start the thread
            thread.Start();
        }

        if (totalSize < totalSizeInBytes) { Thread.Sleep(1000 * 60); }

        
        Console.WriteLine("Done!");
        Console.ReadLine();
    }

  
}

