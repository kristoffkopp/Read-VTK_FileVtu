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
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            var pointDataArray = vtkSpecificDataArray.getNameSpecificDataArrayPointData(unstructuredGrid, dataArrayName);
            double[,] dataArray = new double[,] { };
			ExtremeDisplacement = new double[3];
			
			dataArray = new double[pointDataArray.GetNumberOfTuples(), 3];
			for (int j = 0; j < pointDataArray.GetNumberOfTuples(); j++)
			{
				var tuple = pointDataArray.GetTuple3(j);
				dataArray[j, 0] = tuple[0]; dataArray[j, 1] = tuple[1]; dataArray[j, 2] = tuple[2];
				if (!readExtremeForces)
					continue;

				for (int k = 0; k < 3; k++)
					if (Math.Abs(tuple[k]) > Math.Abs(ExtremeDisplacement[k]))
						ExtremeDisplacement[k] = tuple[k];
			}
			return dataArray;
		}

		public double[] ExtremeDisplacement { get; private set; }
	}
}