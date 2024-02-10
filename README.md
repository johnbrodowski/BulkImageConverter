# imageConverter

Overview
The ImageConverter program leverages the ImageMagick library, a powerful toolset for image manipulation, to perform its operations. It provides an interactive console interface for users to specify their conversion tasks, including format conversion, watermarking, and file management options.

How It Works
Initial Prompt and Help: Upon starting, the program displays a message inviting users to type /help for instructions or press ENTER to proceed. This feature is designed to assist first-time users or provide quick access to command summaries.

Argument Handling: The application accepts command-line arguments, using the first argument as the path to an image file or a directory containing multiple images. If a second argument is provided, it's treated as the path to a watermark image. This flexibility allows for both drag-and-drop operations and traditional command-line input.

Conversion Logic:

Single File: If the specified path points to a file, the program processes this single image, offering options for target format, watermarking, and original file deletion.
Multiple Files: If the path leads to a directory, the program iterates over all files within, applying the conversion process to each. Users can opt to include subdirectories, specify source and target formats, and manage watermarking and file deletion in bulk operations.
User Interaction:

Format Specification: Users specify the target format for conversion from a list of supported types (e.g., webp, png, jpg).
Watermarking: Optionally, a watermark can be applied to the converted images. Users can provide a watermark image path when prompted.
File Deletion: The program asks users if they wish to delete the original files after conversion, including a confirmation step to prevent accidental data loss.
Conversion Process: Utilizing the ImageMagick library, the application reads the source image(s), applies any specified watermark, and saves the result in the target format. Watermarks are placed at the southeast corner of images with reduced opacity for subtlety.

Error Handling: The program includes basic error handling, displaying messages in case of issues during the conversion process.

Key Methods
ConvertSingleFile: Handles conversion tasks for individual files, including user interactions for conversion options.
ConvertMultipleFiles: Manages the conversion of all image files within a specified directory, respecting user preferences for recursion, source/target formats, watermarking, and file deletion.
ConvertImage: The core conversion function that applies format changes and watermarking using ImageMagick.
ConvertImagesInDirectory: Recursively processes directories for image conversion tasks, adhering to user-specified options.
CheckForHelpCommand and DisplayInstructions: Provide user assistance through the /help command, outlining the program's functionality and usage instructions.
Usage Example
plaintext
Copy code
ImageConverter.exe "C:\path\to\directory" "C:\path\to\watermark.png"
This command initiates the conversion process for all images within the specified directory, applying the provided watermark image to each.

Dependencies
ImageMagick for .NET: This program requires the ImageMagick library (Magick.NET package) to be referenced in your project. Ensure it is installed via NuGet or your preferred package manager.
Conclusion
The ImageConverter program is a comprehensive solution for batch image processing tasks, including format conversion and watermarking, designed for ease of use and flexibility. Its reliance on ImageMagick underscores a powerful backend capable of high-quality image manipulations. This application is suitable for both casual users and professionals seeking to automate their image processing workflows.
