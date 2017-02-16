using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadVtkTEST;

namespace ReadVtkTESTtest
{
	[TestClass]
	public class IntergrationsTestPoints
	{
		[TestMethod]
		public void Canary()
		{
			Assert.IsTrue(true);
		}
		[TestMethod]
		public void VTUTestFileReadNumberOfPointsShouldBeFour()
		{
			VTKreader reader = new VTKreader(@"C:\Users\Kristoffer\Dropbox\Dokumenter\Masteroppgave - Spring 2017\TDD vtk-files\TESTTESTTESTTEST.vtu");
			var unstructuredGrid = reader.readFile();

			VTKPointReader pointReader = new VTKPointReader();
			var vectorPoints = pointReader.pointReader(unstructuredGrid);

			Assert.AreEqual(4, vectorPoints.Count);
		}
		[TestMethod]
		public void Canary2()
		{
			Assert.IsTrue(true);
		}
	}
}
