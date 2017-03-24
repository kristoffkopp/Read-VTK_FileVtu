using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using Kitware.VTK;
using System.IO;

namespace ReadVtkTESTtest
{
    [TestClass]
    public class VTKCorrectIDMappingFocusResultFileUnitTest
    {
        private VTKCorrectIDMapping correctIDMapping;
        private vtkUnstructuredGrid unstructuredGrid;
        private int[] nodeID;

        [TestInitialize]
        public void init()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "FocusResults.vtu");
            VTKreader reader = new VTKreader(@path);
            unstructuredGrid = reader.readFile();
            correctIDMapping = new VTKCorrectIDMapping(unstructuredGrid);
            nodeID = new int[21];
            for (int i = 0; i < 20; i++)
                nodeID[i] = i;

            nodeID[20] = 20;
        }

        [TestMethod]
        public void Canary()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void hasCorrectIDMappingPointDataShouldReturnTrueWhenLengthOfArraysAreEqual()
        {
            Assert.IsTrue(correctIDMapping.hasCorrectIDMapping("NodeIDs", nodeID));
        }
        [TestMethod]
        public void hasCorrectIDMappingPointDataShouldReturnFalseWhenIDArrayIsNull()
        {
            Assert.IsFalse(correctIDMapping.hasCorrectIDMapping("NodeIDs", null));
        }

        [TestMethod]
        public void hasCorrectIDMappingPointDataShouldReturnFalseWhenNoArrayInFile()
        {
            Assert.IsFalse(correctIDMapping.hasCorrectIDMapping("WrongIDs", nodeID));
        }
        [TestMethod]
        public void hasCorrectIDMappingPointDataShouldWritePropertyWtihSpecifiedArrayIfIDsNotMatch()
        {
            nodeID[5] = 5;
            nodeID[6] = 4;
            correctIDMapping.hasCorrectIDMapping("NodeIDs", nodeID);
            Assert.AreEqual(21, correctIDMapping.NodeIDsNonContiniusAcending.Length);
        }
        [TestMethod]
        public void hasCorrectIDMappingPointDataShoudlWritePropertyWithEigtheenAtSecondPosition()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "FocusResultsDifferentIDs.vtu");
            VTKreader reader = new VTKreader(@path);
            unstructuredGrid = reader.readFile();
            correctIDMapping = new VTKCorrectIDMapping(unstructuredGrid);
            nodeID = new int[21];
            for (int i = 0; i < 21; i++)
                nodeID[i] = i;

            correctIDMapping.hasCorrectIDMapping("NodeIDs", nodeID);
            Assert.AreEqual(19, correctIDMapping.NodeIDsNonContiniusAcending[1]);
        }
    }
}
