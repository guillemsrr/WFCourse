using System.Collections.Generic;
using UnityEngine;
using WFCourse.Utilities;

namespace WFCourse.Generation
{
    public class WaveFunctionCollapse
    {
        private readonly Dictionary<Vector3Int, CellController> _waveCells;
        private List<CellController> _uncollapsedCells;
        private EntropyHeap _entropyHeap;


        private int NumberCells => _uncollapsedCells.Count;

        public WaveFunctionCollapse(Dictionary<Vector3Int, CellController> waveCells)
        {
            _waveCells = waveCells;
            _uncollapsedCells = new List<CellController>(waveCells.Values);
            _entropyHeap = new EntropyHeap(waveCells.Values);
        }

        public void Observe()
        {
            while (NumberCells != 0)    
            {
                CellController randomCell = _entropyHeap.GetCell();
                if (randomCell == null)
                {
                    _entropyHeap.AddLowestEntropyCell(_uncollapsedCells);
                    randomCell = _entropyHeap.GetCell();
                }
                Collapse(randomCell);
                Propagate(randomCell);
            }
        }

        private void Collapse(CellController randomCell)
        {
            randomCell.Collapse();
            RemoveCollapsedCell(randomCell);
        }

        private void RemoveCollapsedCell(CellController randomCell)
        {
            _uncollapsedCells.Remove(randomCell);
        }

        private void Propagate(CellController cell)
        {
            foreach (KeyValuePair<Direction, Vector3Int> directionVector in Utilities.Directions.DirectionsByVectors)
            {
                Vector3Int offset = cell.Position + directionVector.Value;
                if (!_waveCells.ContainsKey(offset))
                {
                    continue;
                }

                CellController propagatedCell = _waveCells[offset];
                if (propagatedCell.IsErroneus || propagatedCell.IsCollapsed)
                {
                    continue;
                }
                
                propagatedCell.Propagate(directionVector.Key, cell.CollapsedModuleNumber);

                if (propagatedCell.OnlyOnePossibility)
                {
                    Collapse(propagatedCell);
                    Propagate(propagatedCell);
                }

                if (propagatedCell.IsErroneus)
                {
                    RemoveCollapsedCell(propagatedCell);
                }
            }
        }
    }
}