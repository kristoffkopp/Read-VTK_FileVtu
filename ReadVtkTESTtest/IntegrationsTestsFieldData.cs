using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using Kitware.VTK;
using System.IO;

namespace ReadVtkTESTtest
{
    [TestClass]
    public class IntegrationsTestsFieldData
    {
        private vtkUnstructuredGrid unstructuredGrid;
        private VTKFieldDataReader fieldDataReader;
        private double[,] allForcesBeam;
        private bool isInAcentingContinuesOrder;
        [TestInitialize]
        public void init()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTest.vtu");
            VTKreader reader = new VTKreader(@path);
            unstructuredGrid = reader.readFile();
            fieldDataReader = new VTKFieldDataReader();
            allForcesBeam = fieldDataReader.readAllForcesBeam(unstructuredGrid, false);
            isInAcentingContinuesOrder = fieldDataReader.isIDsContinuesAcendingOrder(unstructuredGrid, "BeamIDs");
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
        [TestMethod]
        public void VTUtestFileShouldReturnThatBeamIDsAreInAcendingContinuesOrder()
        {
            Assert.IsTrue(isInAcentingContinuesOrder);
        }

    }
}
