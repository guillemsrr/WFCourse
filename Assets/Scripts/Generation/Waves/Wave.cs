using System.Collections.Generic;
using UnityEngine;
using WFCourse.Generation.Cells;

namespace WFCourse.Generation.Waves
{
    public class Wave
    {
        public Dictionary<Vector3Int, CellController> Cells = new Dictionary<Vector3Int, CellController>();

        public void SetCellsData(WaveData trackingStateWaveData)
        {
            foreach (CellController cellController in Cells.Values)
            {
                cellController.CellData = new CellData(trackingStateWaveData.CellDatas[cellController.Position]);
            }
        }
    }
}