﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ReadVtkTEST
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				RedirectVTKOutput errorObserver = new RedirectVTKOutput();

				var path = Path.Combine(Directory.GetCurrentDirectory(), "FocusResults.vtu");
				VTKreader reader = new VTKreader(@path);
				var unstructuredGrid = reader.readFile();

				VTKPointReader pointReader = new VTKPointReader();
				var vectorPoints = pointReader.pointReader(unstructuredGrid);

				VTKPointDataReader pointDataReader = new VTKPointDataReader(unstructuredGrid);
				//translation and rotationVector can be written as Vector3D - since cfemWrapper is converting double[,] to Vector3D 
				var translation = pointDataReader.readTranslation(true, null);
				var rotationVector = pointDataReader.readRotationVectors(true, null);
				var extremeDisplacement = pointDataReader.ExtremeDisplacement;

				VTKCellReader cellReader = new VTKCellReader();
				var vtkCells = cellReader.readCells(unstructuredGrid);

				VTKCellDataReader cellDataReader = new VTKCellDataReader();
				var allForcesBeams = cellDataReader.readAllForcesBeam(unstructuredGrid);
				var allFourcesShell = cellDataReader.readAllForcesShell(unstructuredGrid);
				var maxForcesBeam = cellDataReader.CalculateMaxForcesBeam;
				var minForcesBeam = cellDataReader.CalculateMinForcesBeam;
				var maxForceShell = cellDataReader.CalculateMaxForcesShell;
				var minForcesShell = cellDataReader.CalculateMinForcesShell;
				var translationBeam = cellDataReader.readTransformationBeam(unstructuredGrid);
				var translationShell = cellDataReader.readTransformationShell(unstructuredGrid);

				VTKFieldDataReader fieldDataReader = new VTKFieldDataReader(unstructuredGrid);

				ElementReader elementReader = new ElementReader();
				var elements = elementReader.readCellsAndPoints(vtkCells, vectorPoints);

				foreach (Element element in elements)
					printToTextBox(element);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void printToTextBox(Element element)
		{
			richTextBox.AppendText("Element type: " + element.CellType.ToString());
			foreach (Vector3D vector in element.VectorList)
			{
				richTextBox.AppendText(System.Environment.NewLine);
				richTextBox.AppendText("Coordinates: " + vector.ToString());
			}
			richTextBox.AppendText(System.Environment.NewLine);
			richTextBox.AppendText(System.Environment.NewLine);
		}
	}
}
