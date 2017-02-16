using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace ReadVtkTEST
{
	public class VTKPointReader
	{
		public List<Vector3D> pointReader(vtkUnstructuredGrid unstructuredGrid)
		{
			var pointsList = new List<Vector3D>();
			var points = unstructuredGrid.GetPoints();
			for (int i=0; i< points.GetNumberOfPoints(); i++) {
				if (points.GetPoint(i).Length < 3) throw new Exception(String.Format("Point at position {0} does not contain a 3(xyz)-coordinate point", i));
				var vector = new Vector3D(points.GetPoint(i)[0], points.GetPoint(i)[1], points.GetPoint(i)[2]);
				pointsList.Add(vector);
			}
			return pointsList;
		}
	}
}
