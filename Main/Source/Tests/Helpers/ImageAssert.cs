using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helpers
{
    public class ImageAssert
    {
        internal const string IMAGE_FILE = @"ControlPanel.png";

        internal static void AssertExpectedImage(string deploymentDirectory, Image favoriteIcon)
        {
            Image expectedImage = Image.FromFile(Path.Combine(deploymentDirectory, IMAGE_FILE));
            Assert.IsTrue(ImageCompare(expectedImage, favoriteIcon), "The file wasnt assigned properly");
        }

        private static bool ImageCompare(Image firstImage, Image secondImage)
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