using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace ReadVtkTEST
{
	public class ElementReader
	{
		public List<Element> readCellsAndPoints(List<VTKCell> cellList, List<Vector3D> vectorList)
		{
			var elementList = new List<Element>();
			foreach(VTKCell cell in cellList)
			{
				var cellType = cell.CellType;
				var cellPointIdList = cell.VtkPointIds;
				var elementVectors = new List<Vector3D>();
				foreach(int cellPointId in cellPointIdList)
					elementVectors.Add(vectorList[cellPointId]);

				elementList.Add(new Element(cellType, elementVectors));
			}
			return elementList;
		}
	}
}