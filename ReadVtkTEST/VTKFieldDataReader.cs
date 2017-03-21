using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ReadVtkTEST
{
	public class VTKFieldDataReader
	{
        public double[,] readAllForcesBeam(vtkUnstructuredGrid unstructuredGrid, bool needsMapping)
        {
            CalculateMaxForcesBeam = new double[6];
            CalculateMinForcesBeam = new double[6];
            return readUnknownTuplesizeNameSpecificFieldDataArray(unstructuredGrid, "BeamStressRes", needsMapping);
        }
        public double[][] readAllForcesShell(vtkUnstructuredGrid unstructuredGrid, bool needsMapping)
        {
            CalculateMaxForcesShell = new double[8];
            CalculateMinForcesShell = new double[8];
            var tempArray = readUnknownTuplesizeNameSpecificFieldDataArray(unstructuredGrid, "AllForcesShell", needsMapping);
            double[][] shellStressArray = new double[tempArray.Length][];
            for (int i = 0; i < tempArray.Length; i++)
            {
                double[] stresses = new double[8];
                for (int j = 0; j < 8; j++)
                    stresses[j] = tempArray[i, j];

                shellStressArray[i] = stresses;
            }
            return shellStressArray;
        }

        public double[,] readUnknownTuplesizeNameSpecificFieldDataArray(vtkUnstructuredGrid unstructuredGrid, string arrayName, bool needsMapping)
        {
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            var fieldDataArray = vtkSpecificDataArray.getNameSpecificDataArrayFieldData(unstructuredGrid, arrayName);
            var numbComp = fieldDataArray.GetNumberOfComponents();            
            var dataArray = new double[fieldDataArray.GetNumberOfTuples(), numbComp];
            for (int j = 0; j < fieldDataArray.GetNumberOfTuples(); j++)
            {
                double[] managedArray = new double[numbComp];
                IntPtr pointerArray = Marshal.AllocCoTaskMem(sizeof(double) * managedArray.Length);

                if (needsMapping)
                    fieldDataArray.GetTuple((BeamIDsNonContiniusAcending[j] - 1), pointerArray);
                else
                    fieldDataArray.GetTuple(j, pointerArray);

                Marshal.Copy(pointerArray, managedArray, 0, numbComp);
                //Did not work to use vktFloatArray, instead can use this, gives same decimalError:
                //for (int k = 0; k<numbComp; k++)
                //		cellData.GetArray(i).GetComponent(j, k);

                for (int k = 0; k < 12; k++)
                    dataArray[j, k] = managedArray[k];

                if (arrayName == "BeamStressRes") //Modelo 6 iterates from 0-5 in all cell_vertexes to find the max and min forces (X,Y,Z,Mx,My,Mz)
                    calculateExtremeForces(managedArray, 6, CalculateMinForcesBeam, CalculateMaxForcesBeam);

                if (arrayName == "AllForcesShell") //Modelo 8 iterates from 0-7 in all cell_vertexes to find the max and min value (Nx,Ny,Nz,Mx,My,Mz,Vzx,Vzy)
                    calculateExtremeForces(managedArray, 8, CalculateMinForcesShell, CalculateMaxForcesShell);
            }
            return dataArray;
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

        public bool isIDsContinuesAcendingOrder(vtkUnstructuredGrid unstructuredGrid, string idType)
        {
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            var fieldDataArray = vtkSpecificDataArray.getNameSpecificDataArrayFieldData(unstructuredGrid, idType);
            var ascending = true;
            int[] dataArray = new int[fieldDataArray.GetNumberOfTuples()];
            for (int j = 0; j < fieldDataArray.GetNumberOfTuples(); j++)
            {
                if (j != 0 && (dataArray[j - 1] + 1 != fieldDataArray.GetTuple1(j)))
                    ascending = false;

                dataArray[j] = (int)fieldDataArray.GetTuple1(j);
            }
            if (!ascending && idType == "BeamIDs")
                BeamIDsNonContiniusAcending = dataArray;
            else if (!ascending && idType == "ShellIDs")
                ShellIDsNonContiniusAcending = dataArray;
            
            return ascending;
        }

        public double[] CalculateMaxForcesBeam { get; private set; }
        public double[] CalculateMinForcesBeam { get; private set; }
        public double[] CalculateMaxForcesShell { get; private set; }
        public double[] CalculateMinForcesShell { get; private set; }
        public int[] BeamIDsNonContiniusAcending { get; private set; }
        public int[] ShellIDsNonContiniusAcending { get; private set; }
    }
}