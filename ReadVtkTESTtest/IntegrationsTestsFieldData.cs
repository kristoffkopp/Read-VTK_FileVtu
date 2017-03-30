using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using Kitware.VTK;
using System.IO;
using System.Windows;

namespace ReadVtkTESTtest
{
    [TestClass]
    public class IntegrationsTestsFieldData
    {
        private vtkUnstructuredGrid unstructuredGrid;
        private VTKFieldDataReader fieldDataReader;
        private double[,] allForcesBeam;
        [TestInitialize]
        public void init()
        {
            RedirectVTKOutput errorObserver = new RedirectVTKOutput();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTest.vtu");
            VTKreader reader = new VTKreader(@path);
            unstructuredGrid = reader.readFile();
            fieldDataReader = new VTKFieldDataReader(unstructuredGrid);
            allForcesBeam = fieldDataReader.readAllForcesBeam(true, null);
            int[] elIDsBeam = new int[20];
            for (int i = 0; i < 20; i++)
                elIDsBeam[i] = i;
        }
        
        [TestMethod]
        public void Canary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void VTUtestFileShouldReturnTwentyBeamForcesResuslts()
        {
            Assert.AreEqual(20, allForcesBeam.GetLength(0));
        }

        [TestMethod]
        public void VTUtestFileShouldReturnTwelveForcesPerResult()
        {
            Assert.AreEqual(12, allForcesBeam.GetLength(1));
        }

        [TestMethod]
        public void VTUtestFileShouldReturnZeroForZTranslationForNodeOneInFirstBeamResult()
        {
            Assert.AreEqual(0, allForcesBeam[0, 2]);
        }

        [TestMethod]
        public void VTUtestFileShouldReturnZeroForXMomentumForNodeOneInSecondBeamResult()
        {
            Assert.AreEqual(0, allForcesBeam[1, 3]);
        }

        [TestMethod]
        public void VTUtestFileShouldReturnSpecificValueZMomentumForNodeOneInThirdBeamResult()
        {
            Assert.AreEqual(1879.0074463, allForcesBeam[2, 5]);
        }
        [TestMethod]
        public void VTUtestFileShouldReturnZeroForZTranslationForNodeTwoInFirstBeamResult()
        {
            Assert.AreEqual(0, allForcesBeam[0, 8]);
        }

        [TestMethod]
        public void VTUtestFileShouldReturnZeroForXMomentumForNodeTwoInSecondBeamResult()
        {
            Assert.AreEqual(0, allForcesBeam[1, 9]);
        }
        [TestMethod]
        public void VTUtestFileShouldReturnNegativeValueZMomentumForNodeOneInSixthBeamResult()
        {
            bool negative = allForcesBeam[5,5] < 0;
            Assert.IsTrue(negative);
        }
    }
}
