using System.Collections.Generic;
using UnityEngine;

namespace WFCourse.Generation
{
    public class WaveFunctionCollapse
    {
        private readonly Dictionary<Vector3Int, CellController> _waveCells;
        private List<CellController> _uncollapsedCells;

        private int NumberCells => _uncollapsedCells.Count;

        public WaveFunctionCollapse(Dictionary<Vector3Int, CellController> waveCells)
        {
            _waveCells = waveCells;
            _uncollapsedCells = new List<CellController>(waveCells.Values);
        }

        public void Observe()
        {
            while (NumberCells != 0)
            {
                CellController randomCell = GetRandomCell();
                Collapse(randomCell);
                Propagate(randomCell);
            }
        }

        private CellController GetRandomCell()
        {
            int randomNumber = Random.Range(0, NumberCells);
            return _uncollapsedCells[randomNumber];
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
            foreach (Vector3Int directionVector in Utilities.Directions.DirectionVectors)
            {
                Vector3Int offset = cell.Position + directionVector;
                if (!_waveCells.ContainsKey(offset))
                {
                    continue;
                }

                CellController propagatedCell = _waveCells[offset];
                if (propagatedCell.IsErroneus || propagatedCell.IsCollapsed)
                {
                    continue;
                }
                
                propagatedCell.Propagate(cell.Type);

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