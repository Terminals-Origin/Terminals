using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Forms.Controls;

namespace Tests.UserInterface
{
    [TestClass]
    public class ProtocolImageListBuilderTests
    {
        private const string TESTIMAGEA = "testImageA";

        private readonly Image testImageA = Terminals.Properties.Resources.NewFolderHS;
        private readonly Image testImageB = Terminals.Properties.Resources.DeleteFolderHS;

        [TestMethod]
        public void UniqueImageKyes_Build_AddsAllImages()
        {
            const string MESSAGE = "We cant affect existing images, if the ImageList is already populated";
            this.AssertImagesBuild(3, "extraImage", MESSAGE);
        } 
        
        [TestMethod]
        public void DuplicitKey_Build_ReplacesConflictingImages()
        {
            const string MESSAGE = "In case of confliciting key we replace the current, ImageList doesnt handle duplicit keys.";
            this.AssertImagesBuild(2, TESTIMAGEA, MESSAGE);
        }

        private void AssertImagesBuild(int expectedImageCount, string addImageKey, string message)
        {
            var imageCount = this.BuildImages(addImageKey);
            Assert.AreEqual(expectedImageCount, imageCount, message);
        }

        private int BuildImages(string extraimage)
        {
            var builder = new ProtocolImageListBuilder(this.CreateTwoIcons);

            using (var imageList = new ImageList())
            {
                imageList.Images.Add(extraimage, this.testImageA); //content is irrelevant here
                builder.Build(imageList);
                return imageList.Images.Count;
            }
        }

        private IDictionary<string, Image> CreateTwoIcons()
        {
            return new Dictionary<string, Image>()
            {
                { TESTIMAGEA, this.testImageA },
                { "testImageB", this.testImageB }
            };
        }
    }
}
