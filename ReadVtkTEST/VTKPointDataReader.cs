using Kitware.VTK;
using System;

namespace ReadVtkTEST
{
	public class VTKPointDataReader
	{
		public double[,] readTranslation(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "Translation", true);
		}
		public double[,] readRotationVectors(vtkUnstructuredGrid unstructuredGrid)
		{
			return readTuple3NameSpecificPointDataArray(unstructuredGrid, "RotationVector", false);
		}

		private double[,] readTuple3NameSpecificPointDataArray(vtkUnstructuredGrid unstructuredGrid, string dataArrayName, bool readExtremeForces)
		{
			double[,] dataArray = new double[,] { };
			var pointData = unstructuredGrid.GetPointData();
			ExtremeDisplacement = new double[3];
			for (int i = 0; i < pointData.GetNumberOfArrays(); i++)
			{
				if (pointData.GetArrayName(i) != dataArrayName)
				continue;

				dataArray = new double[pointData.GetArray(i).GetNumberOfTuples(), 3];
				for (int j = 0; j < pointData.GetArray(i).GetNumberOfTuples(); j++)
				{
					var tuple = pointData.GetArray(i).GetTuple3(j);
					dataArray[j, 0] = tuple[0]; dataArray[j, 1] = tuple[1]; dataArray[j, 2] = tuple[2];
					if (!readExtremeForces)
						continue;

					for (int k = 0; k < 3; k++)
						if (Math.Abs(tuple[k]) > Math.Abs(ExtremeDisplacement[k]))
							ExtremeDisplacement[k] = tuple[k];
				}
			}
			return dataArray;
		}

		public double[] ExtremeDisplacement { get; private set; }
	}
}