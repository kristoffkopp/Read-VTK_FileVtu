using Kitware.VTK;
using System;
using System.Collections.Generic;

namespace ReadVtkTEST
{
	public enum cellDefinitions
	{
		vtkVertex = 1,
		vtkLine = 2,
		vtkTriangle = 3
	}
	public class VTKCellReader
	{
		public List<VTKCell> readCells (vtkUnstructuredGrid unstructuredGrid)
		{
			var cellList = new List<VTKCell>();
			for (int i = 0; i < unstructuredGrid.GetNumberOfCells(); i++ )
			{
				var cell = unstructuredGrid.GetCell(i);
				var cellType = cell.GetCellType();
				if (cellType != 1 && cellType != 3 && cellType != 5) throw new Exception(String.Format("Focus Konstruksjon does not support this type of cell: {0}", cellType));
				foreach (var cellDefinition in Enum.GetValues(typeof(cellDefinitions)))
					if(cell.GetType().Name == cellDefinition.ToString() && cell.GetNumberOfPoints() != (int)cellDefinition)
						throw new Exception(String.Format("Imported vtkCell of cell type {0} expected number of points to be {1}, but number of points were {2}", cell.GetType().Name, (int)cellDefinition, cell.GetNumberOfPoints()));

				var cellPointIds = cell.GetPointIds();
				var vtkPointsIds = new List<int>();
				for (int j = 0; j < cellPointIds.GetNumberOfIds(); j++)
					vtkPointsIds.Add((int)cellPointIds.GetId(j));

				cellList.Add(new VTKCell(cellType, vtkPointsIds));
			}
			return cellList;
		}
	}
}