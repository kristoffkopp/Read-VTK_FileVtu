using Kitware.VTK;
using System;
using System.Runtime.InteropServices;

namespace ReadVtkTEST
{
    public class VTKFieldDataReader
    {
        private vtkUnstructuredGrid m_UnstructuredGrid;
        public VTKFieldDataReader(vtkUnstructuredGrid unstructuredGrid)
        {
            m_UnstructuredGrid = unstructuredGrid;
        }

        public double[,] readAllForcesBeam(bool hasCorrectIDMapping, int[] correctedElID)
        {
            CalculateMaxForcesBeam = new double[6];
            CalculateMinForcesBeam = new double[6];
            return readUnknownTuplesizeNameSpecificFieldDataArray(m_UnstructuredGrid, "BeamStressRes", hasCorrectIDMapping, correctedElID);
        }
        public double[][] readAllForcesShell(bool hasCorrectIDMapping, int[] correctedElID)
        {
            CalculateMaxForcesShell = new double[8];
            CalculateMinForcesShell = new double[8];
            var tempArray = readUnknownTuplesizeNameSpecificFieldDataArray(m_UnstructuredGrid, "AllForcesShell", hasCorrectIDMapping, correctedElID);
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

        private double[,] readUnknownTuplesizeNameSpecificFieldDataArray(vtkUnstructuredGrid unstructuredGrid, string arrayName, bool hasCorrectIDMapping, int[] correctedElID)
        {
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            var fieldDataArray = vtkSpecificDataArray.getNameSpecificDataArrayFieldData(unstructuredGrid, arrayName);
            if (fieldDataArray == null)
                return new double[0, 0];

            var numbComp = fieldDataArray.GetNumberOfComponents();
            var dataArray = new double[fieldDataArray.GetNumberOfTuples(), numbComp];
            for (int j = 0; j < fieldDataArray.GetNumberOfTuples(); j++)
            {
                double[] managedArray = new double[numbComp];
                IntPtr pointerArray = Marshal.AllocCoTaskMem(sizeof(double) * managedArray.Length);

                if (!hasCorrectIDMapping)
                    fieldDataArray.GetTuple(correctedElID[j], pointerArray);
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
        public double[] CalculateMaxForcesBeam { get; private set; }
        public double[] CalculateMinForcesBeam { get; private set; }
        public double[] CalculateMaxForcesShell { get; private set; }
        public double[] CalculateMinForcesShell { get; private set; }
    }
}