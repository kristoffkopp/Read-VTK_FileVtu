using Kitware.VTK;
using System.Collections.Generic;

namespace ReadVtkTEST
{
	public class VTKFieldDataReader
	{		
		public List<double> readFieldData(vtkUnstructuredGrid unstructuredGrid)
		{
			List<double> fieldDataList = new List<double>();
			var fieldData = unstructuredGrid.GetFieldData();
			for (int i = 0; i < fieldData.GetNumberOfArrays(); i++)
			{
				if (fieldData.GetArrayName(i) != "ExtremeDisplacement")
					continue;

				for (int j = 0; j < fieldData.GetArray(i).GetNumberOfTuples(); j++)
					fieldDataList.AddRange(fieldData.GetArray(i).GetTuple3(j));
			}

			return fieldDataList;
		}
	}
}