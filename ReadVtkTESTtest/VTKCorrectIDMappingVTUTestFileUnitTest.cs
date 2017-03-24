using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using Kitware.VTK;
using System.IO;

namespace ReadVtkTESTtest
{
    [TestClass]
    public class VTKCorrectIDMappingVTUTestFileUnitTest
    {
        private VTKCorrectIDMapping correctIDMapping;
        private vtkUnstructuredGrid unstructuredGrid;
        private int[] elID;

        [TestInitialize]
        public void init()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTest.vtu");
            VTKreader reader = new VTKreader(@path);
            unstructuredGrid = reader.readFile();
            correctIDMapping = new VTKCorrectIDMapping(unstructuredGrid);
            elID = new int[20];
            for (int i = 0; i < 20; i++)
                elID[i] = i;
        }

        [TestMethod]
        public void Canary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldReturnTrueWhenLenghtOfArraysAreEqual()
        {
            Assert.IsTrue(correctIDMapping.hasCorrectIDMapping("BeamIDs", elID));
        }
        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldReturnTrueWhenArraysAreEqual()
        {
            Assert.IsTrue(correctIDMapping.hasCorrectIDMapping("BeamIDs", elID));
        }
        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldReturnFalseWhenLengthOfArraysAreUnequal()
        {
            elID = new int[19];
            for (int i = 0; i < 19; i++)
                elID[i] = i;
            Assert.IsFalse(correctIDMapping.hasCorrectIDMapping("BeamIDs", elID));
        }
        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldReturnFalseWhenIDArrayIsNull()
        {
            Assert.IsFalse(correctIDMapping.hasCorrectIDMapping("BeamIDs", null));
        }
        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldReturnFalseWhenNoArrayInFile()
        {
            Assert.IsFalse(correctIDMapping.hasCorrectIDMapping("WrongIDs", elID));
        }
        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldReturnFalseWhenIDsAreDifferentInSamePosition()
        {
            elID[5] = 5;
            elID[6] = 4;
            Assert.IsFalse(correctIDMapping.hasCorrectIDMapping("BeamIDs", elID));
        }
        [TestMethod]
        public void hasCorrectIDMappingFieldDataShouldWritePropertyWtihSpecifiedArrayIfIDsNotMatch()
        {
            elID[5] = 5;
            elID[6] = 4;
            correctIDMapping.hasCorrectIDMapping("BeamIDs", elID);
            Assert.AreEqual(20, correctIDMapping.BeamIDsNonContiniusAcending.Length);
        }

        [TestMethod]
        public void hasCorrectIDMappingFieldDataShoudlWritePropertyWithEigtheenAtSecondPosition()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTestWithDifferentelID.vtu");
            VTKreader reader = new VTKreader(@path);
            unstructuredGrid = reader.readFile();
            correctIDMapping = new VTKCorrectIDMapping(unstructuredGrid);
            elID = new int[20];
            for (int i = 0; i < 20; i++)
                elID[i] = i;

            correctIDMapping.hasCorrectIDMapping("BeamIDs", elID);
            Assert.AreEqual(18, correctIDMapping.BeamIDsNonContiniusAcending[1]);
        }
    }
}
