using System;
using System.IO;
using System.CommandLine;
using System.Drawing;
using System.CommandLine.Invocation;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace BatchColourToAlpha
{
    public class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand(description: "Converts a specfic colour (chroma key) in images and makes them transparent")
            {
                new Option(new string[] { "--color", "--colour", "-c" }, "The named colour to be changed") { Argument = new Argument<Color>() },
                new Option(new string[] { "--outdir" }, "The custom directory to output the changed image.") { Argument = new Argument<DirectoryInfo>() },
            };

            rootCommand.AddArgument(new Argument<FileInfo[]>("files"));

            rootCommand.Handler = CommandHandler.Create<Color?, DirectoryInfo, FileInfo[]>(ProcessImages);

            rootCommand.InvokeAsync(args).Wait();
        }

        public static void ProcessImages(Color? colour, DirectoryInfo outdir, FileInfo[] files)
        {
            Console.WriteLine("Batch Colour to Alpha");
            Console.WriteLine("------------------------");
            Console.WriteLine(files.Length + " files given");

            if (!TryGetColour(ref colour))
            {
                Console.WriteLine("No colour specified");
                return;
            }

            foreach (var file in files)
            {
                Console.Write("File: " + file);

                if (!file.Exists)
                {
                    Console.WriteLine("... Does not exist");
                    continue;
                }

                Bitmap image;

                using (var stream = file.Open(FileMode.Open, FileAccess.Read))
                {                    
                    try
                    {
                        image = new Bitmap(Image.FromStream(stream));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("... Error occurred while opening");
                        break;
                    }
                }

                image.MakeTransparent(colour.Value);

                SaveImage(image, outdir, file);
            }
        }

        static void SaveImage(Bitmap bitmap, DirectoryInfo outDir, FileInfo fileInfo)
        {
            string filePath;

            if (outDir != null)
            {
                if (!outDir.Exists)
                {
                    outDir.Create();
                }
                filePath = Path.Combine(outDir.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name)) + ".png";
            }
            else
            {
                filePath = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.Name)) + ".png";
            }

            Console.Write(" -> " + filePath);

            try
            {
                bitmap.Save(filePath, ImageFormat.Png);
                Console.WriteLine("... Succeeded");
            }
            catch (Exception)
            {
                Console.WriteLine("... Error occurred while saving.");
            }
        }

        /// <summary>
        /// Asks the user for a colour if one has not been specified
        /// </summary>
        /// <param name="colour">The <see cref="Color"/> to check</param>
        /// <returns>If a colour is present</returns>
        static bool TryGetColour(ref Color? colour)
        {
            if (colour.HasValue)
                return true;

            Console.WriteLine("Please select the colour to be changed to alpha");

            var colourDialog = new ColorDialog()
            {
                AllowFullOpen = true,
                AnyColor = true
            };

            if (colourDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            colour = colourDialog.Color;
            return true;
        }

    }
}
