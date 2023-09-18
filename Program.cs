using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;

namespace ImageConverter
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Type '/help' at any time to display instructions, Or press ENTER to continue.");
            Console.WriteLine(" ");
            Console.ResetColor();
            CheckForHelpCommand();
            string path = args.Length > 0 ? args[0].Trim('"') : null;
            string watermarkPath = args.Length > 1 ? args[1].Trim('"') : null;

            if (!string.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                {
                    ConvertSingleFile(path, watermarkPath);
                }
                else if (Directory.Exists(path))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Selected directory: {path}");
                    Console.ResetColor();
                    ConvertMultipleFiles(path, watermarkPath);
                }
            }
            else
            {
                ConvertMultipleFiles(watermarkPath: watermarkPath);
            }
        }

        static void CheckForHelpCommand()
        {
            string command = Console.ReadLine();
            if (command != null && command.ToLower().Trim() == "/help")
            {
                DisplayInstructions();
            }
        }

        static void ConvertSingleFile(string filePath, string watermarkPath = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Available image types: webp, png, bmp, tif, gif, jpg, jpeg");
            Console.Write("Enter the target image type: ");
            string targetType = Console.ReadLine();
            if (string.IsNullOrEmpty(watermarkPath))
            {
                Console.Write("Add watermark? (yes/no): ");
                string addWatermark = Console.ReadLine().ToLower();
                if (addWatermark == "yes")
                {
                    Console.Write("Drag and drop or enter the path of the watermark image: ");
                    watermarkPath = Console.ReadLine().Trim('"');
                }
            }
            Console.Write("Delete original file after conversion? (yes/no): ");
            string deleteOriginal = Console.ReadLine().ToLower();
            if (deleteOriginal == "yes")
            {
                Console.Write("Are you sure you want to delete the original file? (yes/no): ");
                string confirmDelete = Console.ReadLine().ToLower();
                deleteOriginal = confirmDelete;
            }
            Console.ResetColor();
            ConvertImage(filePath, targetType, watermarkPath, deleteOriginal == "yes");
        }

        static void ConvertMultipleFiles(string rootDirectory = null, string watermarkPath = null)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (string.IsNullOrEmpty(rootDirectory))
            {
                Console.Write("Enter the directory path or drag and drop the folder: ");
                rootDirectory = Console.ReadLine().Trim('"');
            }
            Console.Write("Include subdirectories? (yes/no): ");
            string includeSubdirs = Console.ReadLine().ToLower();
            List<string> imageExtensions = new List<string> { "webp", "png", "bmp", "tif", "gif", "jpg", "jpeg" };
            Console.WriteLine("Available image types: webp, png, bmp, tif, gif, jpg, jpeg");
            Console.Write("Enter the source image type: ");
            string sourceType = Console.ReadLine();
            Console.Write("Enter the target image type: ");
            string targetType = Console.ReadLine();
            if (string.IsNullOrEmpty(watermarkPath))
            {
                Console.Write("Add watermark? (yes/no): ");
                string addWatermark = Console.ReadLine().ToLower();
                if (addWatermark == "yes")
                {
                    Console.Write("Drag and drop or enter the path of the watermark image: ");
                    watermarkPath = Console.ReadLine().Trim('"');
                }
            }
            Console.Write("Delete original files after conversion? (yes/no): ");
            string deleteOriginal = Console.ReadLine().ToLower();
            if (deleteOriginal == "yes")
            {
                Console.Write("Are you sure you want to delete the original files? (yes/no): ");
                string confirmDelete = Console.ReadLine().ToLower();
                deleteOriginal = confirmDelete;
            }
            Console.ResetColor();
            if (sourceType == "all")
            {
                foreach (var extension in imageExtensions)
                {
                    ConvertImagesInDirectory(rootDirectory, extension, targetType, watermarkPath, deleteOriginal == "yes", includeSubdirs == "yes");
                }
            }
            else
            {
                ConvertImagesInDirectory(rootDirectory, sourceType, targetType, watermarkPath, deleteOriginal == "yes", includeSubdirs == "yes");
            }
        }

        static void ConvertImagesInDirectory(string directoryPath, string sourceType, string targetType, string watermarkPath, bool deleteOriginal, bool includeSubdirs)
        {
            string[] sourceFiles = Directory.GetFiles(directoryPath, $"*.{sourceType}");
            int imageIndex = 1;
            foreach (var sourceFile in sourceFiles)
            {
                string targetFileName = $"{Path.GetFileName(directoryPath)}_{imageIndex}.{targetType}";
                ConvertImage(sourceFile, targetFileName, watermarkPath, deleteOriginal);
                imageIndex++;
            }
            if (includeSubdirs)
            {
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                foreach (var subDir in subDirectories)
                {
                    ConvertImagesInDirectory(subDir, sourceType, targetType, watermarkPath, deleteOriginal, includeSubdirs);
                }
            }
        }

        static void ConvertImage(string sourceFilePath, string targetFileName, string watermarkPath, bool deleteOriginal)
        {
            try
            {
                string targetFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), targetFileName);
                using (MagickImage image = new MagickImage(sourceFilePath))
                {
                    if (!string.IsNullOrEmpty(watermarkPath))
                    {
                        using (MagickImage watermark = new MagickImage(watermarkPath))
                        {
                            watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 2.0);
                            image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);
                        }
                    }
                    image.Write(targetFilePath);
                }
                if (deleteOriginal)
                {
                    File.Delete(sourceFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting {sourceFilePath} to {targetFileName}: {ex.Message}");
            }
        }

        static void DisplayInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nInstructions:");
            Console.WriteLine("1. When prompted, provide the path for the image or directory of images you want to convert.");
            Console.WriteLine("2. Specify the target image type (e.g., jpg, png).");
            Console.WriteLine("3. Optionally, provide a watermark image.");
            Console.WriteLine("4. Optionally, you can delete the original file(s) after conversion.");
            Console.WriteLine("5. The converted image(s) will be saved in the same directory as the original file(s).");
            Console.ResetColor();
        }
    }
}
