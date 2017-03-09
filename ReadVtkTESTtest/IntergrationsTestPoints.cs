using Kitware.VTK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;
using System.IO;

namespace ReadVtkTESTtest
{
	[TestClass]
	public class IntergrationsTestPoints
	{
		private vtkUnstructuredGrid unstructuredGrid;
		private VTKPointReader pointReader;
		private double[,] translation;
		private double[,] rotationVector;
		private double[] extremeDisplacement;

		[TestInitialize]
		public void init()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "VTUInputTest.vtu");
			VTKreader reader = new VTKreader(@path);
			unstructuredGrid = reader.readFile();
			pointReader = new VTKPointReader();

			VTKPointDataReader pointDataReader = new VTKPointDataReader();
			translation = pointDataReader.readTranslation(unstructuredGrid);
			rotationVector = pointDataReader.readRotationVectors(unstructuredGrid);
			extremeDisplacement = pointDataReader.ExtremeDisplacement;
		}
		[TestMethod]
		public void Canary()
		{
			Assert.IsTrue(true);
		}
		[TestMethod]
		public void VTUTestFileReadNumberOfPointsShouldBeFour()
		{
			var vectorPoints = pointReader.pointReader(unstructuredGrid);
			Assert.AreEqual(4, vectorPoints.Count);
		}
		[TestMethod]
		public void VTUTestFileReadXCoordinateShouldBeZeroOfFirstPoint()
		{
			var vectorPoints = pointReader.pointReader(unstructuredGrid);
			Assert.AreEqual(0, vectorPoints[0].X); ;
		}
		[TestMethod]
		public void VTUTestFileReadYCoordinateShouldBeOneOfThirdPoint()
		{
			var vectorPoints = pointReader.pointReader(unstructuredGrid);
			Assert.AreEqual(1, vectorPoints[2].Y); ;
		}
		[TestMethod]
		public void VTUTestFileReadZCoordinateShouldBeOneOfForthPoint()
		{
			var vectorPoints = pointReader.pointReader(unstructuredGrid);
			Assert.AreEqual(1, vectorPoints[3].Z); ;
		}
		[TestMethod]
		public void VTUTestFileReadTranslationShouldBeOfLenghtTwelve()
		{
			Assert.AreEqual(12, translation.Length);
		}
		[TestMethod]
		public void VTUTestFileReadTranslationShouldBeGivenDoubleAtPointZeroZero()
		{
			Assert.AreEqual(0.10000000149, translation[0,0]);
		}
		[TestMethod]
		public void VTUTestFileReadRotaionShouldBeOfLenghtTwelve()
		{
			Assert.AreEqual(12, rotationVector.Length);
		}
		[TestMethod]
		public void VTUTestFileReadRotationShouldBeGivenDoubleAtPointZeroTwo()
		{
			Assert.AreEqual(0.30000001192, translation[0, 2]);
		}
		[TestMethod]
		public void VTUTestFileReadExtremeDisplacementShouldBeOfLenghtThree()
		{
			Assert.AreEqual(3, extremeDisplacement.Length);
		}

	}
}
