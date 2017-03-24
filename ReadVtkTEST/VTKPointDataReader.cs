using Kitware.VTK;
using System;

namespace ReadVtkTEST
{
	public class VTKPointDataReader
	{
        private vtkUnstructuredGrid m_UnstructuredGrid;
        public VTKPointDataReader(vtkUnstructuredGrid unstructuredGrid)
        {
            m_UnstructuredGrid = unstructuredGrid;
        }
		public double[,] readTranslation(bool hasCorrectIDMapping, int[] correctedNodeID)
		{
			return readTuple3NameSpecificPointDataArray(m_UnstructuredGrid, "Translation", true, hasCorrectIDMapping, correctedNodeID);
		}
		public double[,] readRotationVectors(bool hasCorrectIDMapping, int[] correctedNodeID)
		{
			return readTuple3NameSpecificPointDataArray(m_UnstructuredGrid, "RotationVector", false, hasCorrectIDMapping, correctedNodeID);
		}
		private double[,] readTuple3NameSpecificPointDataArray(vtkUnstructuredGrid unstructuredGrid, string dataArrayName, bool readExtremeForces, bool hasCorrectIDMapping, int[] correctedNodeID)
		{
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            var pointDataArray = vtkSpecificDataArray.getNameSpecificDataArrayPointData(unstructuredGrid, dataArrayName);
            double[,] dataArray = new double[,] { };
			ExtremeDisplacement = new double[3];
			
			dataArray = new double[pointDataArray.GetNumberOfTuples(), 3];
			for (int j = 0; j < pointDataArray.GetNumberOfTuples(); j++)
			{
                double[] tuple;
                if (!hasCorrectIDMapping)
                    tuple = pointDataArray.GetTuple3(correctedNodeID[j]);
                else
                    tuple = pointDataArray.GetTuple3(j);

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