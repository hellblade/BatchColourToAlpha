using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BatchColourToAlpha.Tests
{
    public abstract class TestBase
    {
        const int ImageWidth = 32;
        const int ImageHeight = 32;
        const string ImagesFolder = "images";

        [ClassInitialize]
        public void TestSetup()
        {
            if (Directory.Exists(ImagesFolder))
            {                
                Directory.Delete(ImagesFolder);
            }
            Directory.CreateDirectory(ImagesFolder);
        }        

        protected static void CreateImage(string path, Color mainColour, Color alphaColour)
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format32bppArgb);

            for (int x = 0; x < ImageWidth; x++)
            {
                for (int y = 0; y < ImageHeight; y++)
                {
                    bitmap.SetPixel(x, y, x == y ? mainColour : alphaColour);
                }
            }

            bitmap.Save(path);
            Thread.Sleep(100); // Give it time to save to be sure...
        }

        protected void CheckImage(string path, Color mainColour)
        {
            using (var bmp = new Bitmap(Image.FromFile(path)))
            {
                CheckImage(bmp, mainColour);
            }
        }

        protected void CheckImage(Bitmap bitmap, Color mainColour)
        {
            Thread.Sleep(100); // Give it time to save to be sure...

            for (int x = 0; x < ImageWidth; x++)
            {
                for (int y = 0; y < ImageHeight; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);

                    if (x == y)
                    {
                        Assert.AreEqual(mainColour.ToArgb(), pixel.ToArgb());
                    }
                    else
                    {
                        Assert.AreEqual(0, pixel.A);
                    }
                }
            }
        }

        protected void RunProgram(Color? alphaColour, string file, string outDir = null)
        {
            Program.ProcessImages(alphaColour, outDir == null ? null : new DirectoryInfo(ImagesFolder + "/" + outDir), new FileInfo[] { new FileInfo(file) });
        }

        protected string GetFileName(bool isPng = true, [CallerMemberName] string caller = "")
        {
            return ImagesFolder + "/" + caller + (isPng ? ".png" : ".bmp");
        }

        protected string GetFileName(string outdir, [CallerMemberName] string caller = "")
        {
            return ImagesFolder + "/" + outdir + "/" + caller + ".png";
        }

    }
}
