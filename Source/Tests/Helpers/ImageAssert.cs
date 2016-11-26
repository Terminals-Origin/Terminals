using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helpers
{
    public class ImageAssert
    {
        internal const string IMAGE_FILE = @"ControlPanel.png";

        internal static void AssertExpectedIcon(string deploymentDirectory, Image favoriteIcon)
        {
            var areEqual = IconEquals(deploymentDirectory, favoriteIcon);
            Assert.IsTrue(areEqual, "The file wasnt assigned properly");
        }

        internal static bool IconEquals(string deploymentDirectory, Image favoriteIcon)
        {
            string fullIconPath = Path.Combine(deploymentDirectory, IMAGE_FILE);
            Image expectedImage = Image.FromFile(fullIconPath);
            return AreEqual(expectedImage, favoriteIcon);
        }

        private static bool AreEqual(Image firstImage, Image secondImage)
        {
            using (var ms = new MemoryStream())
            {
                firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] firstBitmap = ms.ToArray();
                ms.Position = 0;

                secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] secondBitmap = ms.ToArray();

                return firstBitmap.SequenceEqual(secondBitmap);
            }
        }
    }
}