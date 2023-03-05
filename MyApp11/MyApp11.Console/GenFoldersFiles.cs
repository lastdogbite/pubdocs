using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace MyApp11.Console
{
    public class GenFoldersFiles
    {


        static Random rand = new Random();
        static object locker = new object();
        static int folderCount = 0;
        static int subFolderCount = 0;
        static int fileCount = 0;
        static long totalSize = 0;
        static bool isRunning = true;



        static string[] directories = null;
        static string [] GetDirectories(string rootDirectory)
        {
            if (directories != null) { return directories; }
            directories = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories);
            return directories;
        }
            

        public static void CreateRandomFolders(string root, int rootFolderCount, int folderCount)
    {
        Random random = new Random();
        for (int i = 1; i <= rootFolderCount; i++)
        {
            string rootDirectory = Path.Combine(root, $"Folder{i}_{Path.GetRandomFileName()}");
            Directory.CreateDirectory(rootDirectory);
            CreateFoldersRecursive(rootDirectory, folderCount - 1);
        }
    }

    public static void CreateFoldersRecursive(string rootDirectory, int folderCount)
    {
        if (folderCount <= 0) return; // stop creating folders if we've reached the target count
        string directoryPath = Path.Combine(rootDirectory, $"Folder{folderCount}");
        Directory.CreateDirectory(directoryPath);
        CreateFoldersRecursive(directoryPath, folderCount - 1); // recursively create subfolders
    }      

    public static (string, long) AddRandomFile(string rootDirectory, int maxFileSizeKB = 10000)
    {
        Random random = new Random();
        int maxFileSizeBytes = maxFileSizeKB * 1024;

        directories = GetDirectories(rootDirectory);          
        if (directories.Length == 0) return ("", 0); // stop if there are no subdirectories
            rootDirectory = directories[random.Next(directories.Length)]; // randomly choose a subdirectory
        

        string fileName = Path.GetRandomFileName() + ".txt";
        string filePath = Path.Combine(rootDirectory, fileName);

        Random _random = new Random();
        int randomNumber = random.Next(1, 4);

            try
            {

                if (randomNumber == 1)
                {
                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                    {
                        int contentSize = random.Next(1024, maxFileSizeBytes); // randomly choose a content size between 1KB and 10MB
                        int remainingSize = contentSize;
                        while (remainingSize > 0)
                        {
                            int writeSize = Math.Min(remainingSize, 1000);
                            string content = new string('x', writeSize); // generate a string of x's with length writeSize
                            streamWriter.Write(content); // write the content to the file using AppendAllText
                            remainingSize -= writeSize;
                        }
                    }
                }

                else if (randomNumber == 2)
                {

                    int contentSize = random.Next(1024, maxFileSizeBytes); // randomly choose a content size between 1KB and 1000MB
                    int remainingSize = contentSize;
                    while (remainingSize > 0)
                    {
                        int writeSize = Math.Min(remainingSize, 1000);
                        string content = new string('x', writeSize); // generate a string of x's with length writeSize
                        File.AppendAllText(filePath, content); // write the content to the file using AppendAllText
                        remainingSize -= writeSize;
                    }

                }

                else
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.CreateNew))
                    {
                        int contentSize = random.Next(1024, maxFileSizeBytes); // randomly choose a content size between 1KB and 1000MB
                        byte[] content = new byte[contentSize];
                        random.NextBytes(content); // fill the byte array with random data
                        stream.Write(content, 0, content.Length); // write the byte array to the file
                    }
                }


            }//try
            catch(IOException ex)
            {
                try
                {
                    FileInfo _fileInfo = new FileInfo(filePath);
                    long _fileSize = _fileInfo.Length; // get the size of the file in bytes
                    return ("error_" + filePath, _fileSize);

                }
                catch { }
            }
        FileInfo fileInfo = new FileInfo(filePath);
        long fileSize = fileInfo.Length; // get the size of the file in bytes
        return (filePath, fileSize);
    }//fun

    public static (string, long) ZipRandomFile(string sourceDirectory, string destinationDirectory)
    {
        Random random = new Random();
        string[] fileNames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
        string randomFileName = fileNames[random.Next(fileNames.Length)];
        string destinationFileName = Path.Combine(destinationDirectory, Path.GetFileNameWithoutExtension(randomFileName) + ".zip");
        int retryCount = 0;
        while (true)
        {
            try
            {
                using (FileStream stream = File.Open(randomFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (ZipArchive archive = ZipFile.Open(destinationFileName, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(randomFileName, Path.GetFileName(randomFileName));
                    }
                }
                FileInfo fileInfo = new FileInfo(destinationFileName);
                long fileSize = fileInfo.Length;
                return (destinationFileName, fileSize);
            }
            catch (IOException)
            {
                if (retryCount < 100)
                {
                    retryCount++;
                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                        return ("error_file_use_" + destinationFileName, 0);
                }
            }//catch
        }//while
    }//fn










        //public static void CreateRandomFolders(string rootDirectory, int level, int count)
        //{
        //    if (level > 10) return; // stop creating subfolders if we've reached max depth
        //    Random random = new Random();
        //    for (int i = 1; i <= count; i++)
        //    {
        //        string folderName = Path.GetRandomFileName();
        //        string directoryPath = Path.Combine(rootDirectory, folderName);
        //        Directory.CreateDirectory(directoryPath);
        //        CreateRandomFolders(directoryPath, level + 1, count); // recursively create subfolders
        //    }
        //}

        public static void GenerateRandomFiles(string path)
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
                    throw ex;
                }

                // Sleep for random time between 100ms and 500ms
                Thread.Sleep(rand.Next(100, 500));
            }
        }
        // call the function with the root directory and desired count


    }
}
