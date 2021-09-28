using System.Collections.Generic;
using WFCourse.Generation.Cells;
using WFCourse.Generation.Waves;

namespace WFCourse.Generation.Backtracking
{
    public class TrackingState
    {
        private WaveData _waveData;
        private List<CellController> _uncollapsedCells;
        private EntropyHeap _entropyHeap;

        public WaveData WaveData => _waveData;
        public List<CellController> UncollapsedCells => _uncollapsedCells;
        public EntropyHeap EntropyHeap => _entropyHeap;

        public TrackingState(Wave wave, List<CellController> uncollapsedCells, EntropyHeap entropyHeap)
        {
            _waveData = new WaveData(wave);
            _uncollapsedCells = new List<CellController>(uncollapsedCells);
            _entropyHeap = new EntropyHeap(entropyHeap);
        }
    }
}