using Kitware.VTK;
using System.IO;

namespace ReadVtkTEST
{
	public class VTKreader
	{
		private string m_filePath;
		public VTKreader(string filePath)
		{
			m_filePath = filePath;
		}
		public vtkUnstructuredGrid readFile() {
			var reader = vtkXMLUnstructuredGridReader.New();
			reader.SetFileName(m_filePath);
			reader.Update();
			
			return reader.GetOutput();
		}
	}
}
