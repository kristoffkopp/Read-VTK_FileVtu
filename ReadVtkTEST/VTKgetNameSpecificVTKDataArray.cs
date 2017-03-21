using Kitware.VTK;

namespace ReadVtkTEST
{
    class VTKgetNameSpecificVTKDataArray
    {
        public vtkDataArray getNameSpecificDataArrayCellData(vtkUnstructuredGrid unstructuredGrid, string arrayName)
        {
            vtkDataArray dataArray = null;
            var cellData = unstructuredGrid.GetCellData();
            for (int i = 0; i < cellData.GetNumberOfArrays(); i++)
            {
                if (cellData.GetArrayName(i) != arrayName)
                    continue;

                dataArray = cellData.GetArray(i);
            }
            return dataArray;
        }
        public vtkDataArray getNameSpecificDataArrayFieldData(vtkUnstructuredGrid unstructuredGrid, string arrayName)
        {
            vtkDataArray dataArray = null;
            var fieldData = unstructuredGrid.GetFieldData();
            for (int i = 0; i < fieldData.GetNumberOfArrays(); i++)
            {
                if (fieldData.GetArrayName(i) != arrayName)
                    continue;

                dataArray = fieldData.GetArray(i);
            }
            return dataArray;
        }
    }
}
