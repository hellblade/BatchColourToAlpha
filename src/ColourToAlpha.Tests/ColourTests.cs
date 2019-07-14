using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BatchColourToAlpha.Tests
{
    [TestClass]
    public class ColourTests : TestBase
    {
        [TestMethod]
        public void HandlesPngFiles()
        {
            Color mainColour = Color.Orange;
            Color alphaColour = Color.Magenta;

            string fileName = GetFileName();

            CreateImage(fileName, mainColour, alphaColour);
            RunProgram(alphaColour, fileName);

            CheckImage(fileName, mainColour);
        }

        [TestMethod]
        public void HandlesBmpFiles()
        {
            Color mainColour = Color.Orange;
            Color alphaColour = Color.Magenta;

            string fileName = GetFileName(false);

            CreateImage(fileName, mainColour, alphaColour);
            RunProgram(alphaColour, fileName);

            CheckImage(GetFileName(), mainColour);
        }

        [TestMethod]
        public void HandlesPngFilesOutDir()
        {
            Color mainColour = Color.Orange;
            Color alphaColour = Color.Magenta;

            string fileName = GetFileName();
            string expectedNewFileName = GetFileName("out");

            CreateImage(fileName, mainColour, alphaColour);
            RunProgram(alphaColour, fileName, "out");

            CheckImage(expectedNewFileName, mainColour);
        }

        [TestMethod]
        public void HandlesBmpFilesOutDir()
        {
            Color mainColour = Color.Orange;
            Color alphaColour = Color.Magenta;

            string fileName = GetFileName(false);
            string expectedNewFileName = GetFileName("out");

            CreateImage(fileName, mainColour, alphaColour);
            RunProgram(alphaColour, fileName, "out");

            CheckImage(expectedNewFileName, mainColour);
        }
    }
}
