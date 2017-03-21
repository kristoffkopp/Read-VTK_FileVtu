using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using Kitware.VTK;
using System.IO;

namespace ReadVtkTESTtest
{
	[TestClass]
	public class VTKCellDataReaderTramsformationUnitTest
	{
		private vtkUnstructuredGrid unstructuredGrid;
		private VTKCellDataReader cellDataReader;

		[TestInitialize]
		public void init()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTest.vtu");
			VTKreader reader = new VTKreader(@path);
			unstructuredGrid = reader.readFile();

			cellDataReader = new VTKCellDataReader();
		}

		[TestMethod]
		public void Canary()
		{
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void readTransformationBeamShouldReturnListOfLengthTwo()
		{
			var lists = cellDataReader.readTransformationBeam(unstructuredGrid);
			Assert.AreEqual(2, lists.Length);
		}

		[TestMethod]
		public void readTransformationBeamShouldReturnListWithNineElementsInFirstRow()
		{
			var lists = cellDataReader.readTransformationBeam(unstructuredGrid);
			Assert.AreEqual(9, lists[0].Length);
		}

		[TestMethod]
		public void readTransformationBeamShouldReturnFirstListWithFirstElementIsNegative()
		{
			var lists = cellDataReader.readTransformationBeam(unstructuredGrid);
			bool negative = lists[0][0] < 0;
			Assert.IsTrue(negative);
		}

		[TestMethod]
		public void readTransformationBeamShouldReturnFirstListWithLastElementNegative()
		{
			var lists = cellDataReader.readTransformationBeam(unstructuredGrid);
			bool negative = lists[0][8] < 0;
			Assert.IsTrue(negative);
		}

		[TestMethod]
		public void readTransformationBeamShouldReturnSpecifiedElementOfSecoundListThirdElement()
		{
			//Reads all doubles - but CHANGES decimals (not only adding)
			var lists = cellDataReader.readTransformationBeam(unstructuredGrid);
			Assert.AreEqual(0.99994751209229038, lists[1][2]);
		}

		[TestMethod]
		public void readTransformationShellShouldReturnListOfLengthTwo()
		{
			var lists = cellDataReader.readTransformationShell(unstructuredGrid);
			Assert.AreEqual(2, lists.Length);
		}

		[TestMethod]
		public void readTransformationShellShouldReturnListWithNineElementsInFirstRow()
		{
			var lists = cellDataReader.readTransformationShell(unstructuredGrid);
			Assert.AreEqual(9, lists[0].Length);
		}

		[TestMethod]
		public void readTransformationShellShouldReturnSecondListWithFifthElementIsNegative()
		{
			var lists = cellDataReader.readTransformationShell(unstructuredGrid);
			bool negative = lists[1][4] < 0;
			Assert.IsTrue(negative);
		}

		[TestMethod]
		public void readTransformationShellShouldReturnFirstListWithSecondElementPositive()
		{
			var lists = cellDataReader.readTransformationShell(unstructuredGrid);
			bool positve = lists[0][1] > 0;
			Assert.IsTrue(positve);
		}

		[TestMethod]
		public void readTransformationShellShouldReturnSpecifiedElementOfSecoundListLastElement()
		{
			//Does not read full double, as other integrationstests fail.
			var lists = cellDataReader.readTransformationShell(unstructuredGrid);
			Assert.AreEqual(0.99994751209229038, lists[1][8]);
		}
	}
}
