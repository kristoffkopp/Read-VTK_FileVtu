using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ReadVtkTEST
{
	public class VTKCellDataReader
	{
		public List<double> readAllForcesBeam(vtkUnstructuredGrid unstructuredGrid) {
			CalculateMaxForcesBeam = new double[6];
			CalculateMinForcesBeam = new double[6];
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesBeam");
		}
		public List<double> readAllForcesShell(vtkUnstructuredGrid unstructuredGrid)
		{
			CalculateMaxForcesShell = new double[8];
			CalculateMinForcesShell = new double[8];
			return readUnknownTuplesizeNameSpecificCellDataArray(unstructuredGrid, "AllForcesShell");
		}
        public double[][] readTransformationBeam(vtkUnstructuredGrid unstructuredGrid)
        {
            return readTransformation(unstructuredGrid, "TransformationBeam");
        }
        public double[][] readTransformationShell(vtkUnstructuredGrid unstructuredGrid)
        {
            return readTransformation(unstructuredGrid, "TransformationShell");
        }
        private List<double> readUnknownTuplesizeNameSpecificCellDataArray(vtkUnstructuredGrid unstructuredGrid, string arrayName)
		{
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            var cellDataArray = vtkSpecificDataArray.getNameSpecificDataArrayCellData(unstructuredGrid, arrayName);
            var dataArray = new List<double>();
			var numbComp = cellDataArray.GetNumberOfComponents();
			for (int j = 0; j < cellDataArray.GetNumberOfTuples(); j++)
			{	
				double[] managedArray = new double[numbComp];
				IntPtr pointerArray = Marshal.AllocCoTaskMem(sizeof(double) * managedArray.Length);
				cellDataArray.GetTuple(j, pointerArray);
                Marshal.Copy(pointerArray, managedArray, 0, numbComp);
                //Did not work to use vktFloatArray, instead can use this:
                //for (int k = 0; k<numbComp; k++)
                //		cellDataArray.GetComponent(j, k);

                dataArray.AddRange(managedArray);

				if (arrayName == "AllForcesBeam") //Modelo 6 iterates from 0-5 in all cell_vertexes to find the max and min forces (X,Y,Z,Mx,My,Mz)
					calculateExtremeForces(managedArray, 6, CalculateMinForcesBeam, CalculateMaxForcesBeam);

				if (arrayName == "AllForcesShell") //Modelo 8 iterates from 0-7 in all cell_vertexes to find the max and min value (Nx,Ny,Nz,Mx,My,Mz,Vzx,Vzy)
					calculateExtremeForces(managedArray, 8, CalculateMinForcesShell, CalculateMaxForcesShell);
			}	
			return dataArray;
		}

		public double[][] readTransformation(vtkUnstructuredGrid unstructuredGrid, string arrayName)
		{
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
			double[][] TransformationLists = null;
            var cellDataArray = vtkSpecificDataArray.getNameSpecificDataArrayCellData(unstructuredGrid, arrayName);
            var numbComp = cellDataArray.GetNumberOfComponents();
			var numbTuples = cellDataArray.GetNumberOfTuples();
			TransformationLists = new double[numbTuples][];
			for (int j = 0; j < numbTuples; j++)
			{
				double[] managedArray = new double[numbComp];
				IntPtr pointerArray = Marshal.AllocCoTaskMem(sizeof(double) * managedArray.Length);
				cellDataArray.GetTuple(j, pointerArray);
				Marshal.Copy(pointerArray, managedArray, 0, numbComp);

				TransformationLists[j] = managedArray;
			}
			return TransformationLists;
		}

		private void calculateExtremeForces(double[] managedArray, int numberOfForces, double[] calculateMin, double[] calculateMax)
		{
			for (int k = 0; k < managedArray.Length; k++)
			{
				if (Math.Abs(managedArray[k]) > Math.Abs(calculateMax[k % numberOfForces]))
					calculateMax[k % numberOfForces] = managedArray[k];

				if (Math.Abs(managedArray[k]) < Math.Abs(calculateMin[k % numberOfForces]) || Math.Abs(calculateMin[k % numberOfForces]) == 0)
					calculateMin[k % numberOfForces] = managedArray[k];
			}
		}

		public double[] CalculateMaxForcesBeam { get; private set; }
		public double[] CalculateMinForcesBeam { get; private set; }
		public double[] CalculateMaxForcesShell { get; private set; }
		public double[] CalculateMinForcesShell { get; private set; }
	}
}