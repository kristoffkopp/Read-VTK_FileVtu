using Kitware.VTK;
using System;

namespace ReadVtkTEST
{
	public class RedirectVTKOutput
	{
		public RedirectVTKOutput()
		{
			vtkFileOutputWindow fileOutputWindow = vtkFileOutputWindow.New();
			if (vtkOutputWindow.GetInstance() != null)
			{
				vtkOutputWindow.SetInstance(fileOutputWindow);
				fileOutputWindow.ErrorEvt += new vtkObject.vtkObjectEventHandler(outputWindow_ErrorEvt);
			}
		}

		void outputWindow_ErrorEvt(vtkObject sender, vtkObjectEventArgs e)
		{
			string errorString = "ERROR: unknown";
				if (e.CallData != IntPtr.Zero)
					errorString = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(e.CallData);

				throw new Exception(errorString);
		}
	}
}