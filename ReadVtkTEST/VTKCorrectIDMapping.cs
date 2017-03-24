using Kitware.VTK;

namespace ReadVtkTEST
{
    public class VTKCorrectIDMapping
    {
        private vtkUnstructuredGrid m_UnstructuredGrid;
        public VTKCorrectIDMapping(vtkUnstructuredGrid unstructuredGrid)
        {
            m_UnstructuredGrid = unstructuredGrid;
        }
        public bool hasCorrectIDMapping(string idType, int[] idList)
        {
            VTKgetNameSpecificVTKDataArray vtkSpecificDataArray = new VTKgetNameSpecificVTKDataArray();
            vtkDataArray idData = null;
            if(idType == "BeamIDs" || idType == "ShellIDs")
                idData = vtkSpecificDataArray.getNameSpecificDataArrayFieldData(m_UnstructuredGrid, idType);
            else if(idType == "NodeIDs")
                idData = vtkSpecificDataArray.getNameSpecificDataArrayPointData(m_UnstructuredGrid, idType);

            if (idData == null || idList == null)
                return false;

            var hasCorrectIDMapping = true;
            int[] dataArray = new int[idData.GetNumberOfTuples()];
            if (idData.GetNumberOfTuples() != idList.Length)
                return false;

            for (int i = 0; i < idData.GetNumberOfTuples(); i++)
            {
                if (idList[i] != (idData.GetTuple1(i) - 1))
                    hasCorrectIDMapping = false;

                dataArray[i] = (int)(idData.GetTuple1(i) - 1);
            }
            if (!hasCorrectIDMapping && idType == "BeamIDs")
                BeamIDsNonContiniusAcending = dataArray;
            else if (!hasCorrectIDMapping && idType == "ShellIDs")
                ShellIDsNonContiniusAcending = dataArray;
            else if (!hasCorrectIDMapping && idType == "NodeIDs")
                NodeIDsNonContiniusAcending = dataArray;

            return hasCorrectIDMapping;
        }
        public int[] BeamIDsNonContiniusAcending { get; private set; }
        public int[] ShellIDsNonContiniusAcending { get; private set; }
        public int[] NodeIDsNonContiniusAcending { get; private set; }
    }
}
