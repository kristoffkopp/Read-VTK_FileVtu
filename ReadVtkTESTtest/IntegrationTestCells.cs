using Kitware.VTK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using System.Collections.Generic;
using System.IO;

namespace ReadVtkTESTtest
{
	[TestClass]
	public class IntegrationTestCells
	{
		[TestClass]
		public class IntegrationTestsCells
		{
			private vtkUnstructuredGrid unstructuredGrid;
			private VTKCellReader cellReader;
			private List<VTKCell> vtkCells;
			private List<double> allForcesBeams;
			private List<double> allFourcesShell;
			private double[] maxForcesBeam;
			private double[] minForcesBeam;
			private double[] maxForceShell;
			private double[] minForcesShell;

			[TestInitialize]
			public void init()
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTest.vtu");
				VTKreader reader = new VTKreader(@path);
				unstructuredGrid = reader.readFile();

				cellReader = new VTKCellReader();
				vtkCells = cellReader.readCells(unstructuredGrid);

				VTKCellDataReader cellDataReader = new VTKCellDataReader();
				allForcesBeams = cellDataReader.readAllForcesBeam(unstructuredGrid);
				allFourcesShell = cellDataReader.readAllForcesShell(unstructuredGrid);
				maxForcesBeam = cellDataReader.CalculateMaxForcesBeam;
				minForcesBeam = cellDataReader.CalculateMinForcesBeam;
				maxForceShell = cellDataReader.CalculateMaxForcesShell;
				minForcesShell = cellDataReader.CalculateMinForcesShell;
			}
			[TestMethod]
			public void Canary()
			{
				Assert.IsTrue(true);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnTwoOfLengthOfCells()
			{
				Assert.AreEqual(2, vtkCells.Count);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnCellTypeFiveOfFirstCell()
			{
				Assert.AreEqual(5, vtkCells[0].CellType);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnLengthThreeOfPointIdsOfFirstCell()
			{
				Assert.AreEqual(3, vtkCells[0].VtkPointIds.Count);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnTwoAtPointIDPositionThreeOfFirstCell()
			{
				Assert.AreEqual(2, vtkCells[0].VtkPointIds[2]);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnTwetyFourOfLengthOfAllFourcesBeam()
			{
				Assert.AreEqual(24, allForcesBeams.Count);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnFourtyEightOfLengthOfAllFourcesShell()
			{
				Assert.AreEqual(48, allFourcesShell.Count);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnSixOfLengthOfMaxFourcesBeam()
			{
				Assert.AreEqual(6, maxForcesBeam.Length);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnSixOfLengthOfMinFourcesBeam()
			{
				Assert.AreEqual(6, minForcesBeam.Length);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnSixOfLengthOfMaxFourcesShell()
			{
				Assert.AreEqual(8, maxForceShell.Length);
			}
			[TestMethod]
			public void VTUTestFileReadShouldReturnSixOfLengthOfMinFourcesShell()
			{
				Assert.AreEqual(8, minForcesShell.Length);
			}
			//Further: Check exact values of elements in the document
		}
	}
}
