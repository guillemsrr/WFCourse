using System;
using System.Collections.Generic;

namespace WFCourse.Generation
{
    public class EntropyHeap
    {
        private SortedList<float, CellController> _sortedCells = new SortedList<float, CellController>();
        public EntropyHeap(ICollection<CellController> cells)
        {
            foreach (CellController cell in cells)
            {
                cell.CellPropagatedEvent += AddCell;
            }

            AddLowestEntropyCell(cells);
        }

        private void AddCell(CellController cell)
        {
            if (_sortedCells.ContainsValue(cell)) return;
            
            _sortedCells.Add(cell.GetEntropy(), cell);
        }

        public void AddLowestEntropyCell(ICollection<CellController> cells)
        {
            CellController lowestEntropyCell = null;
            float lowestEntropy = Single.MaxValue;
            
            foreach (CellController cell in cells)
            {
                float entropy = cell.GetEntropy();
                if (entropy < lowestEntropy)
                {
                    lowestEntropyCell = cell;
                    lowestEntropy = entropy;
                }
            }
            
            AddCell(lowestEntropyCell);
        }

        public CellController GetCell()
        {
            CellController cell = null;
            while (_sortedCells.Count != 0)
            {
                do
                {
                    cell = _sortedCells.Values[0];
                    _sortedCells.RemoveAt(0);
                }
                while (cell.IsCollapsed || cell.IsErroneus);
            }

            return cell;
        }
    }
}