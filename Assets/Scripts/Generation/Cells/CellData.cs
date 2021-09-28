using System.Collections.Generic;
using WFCourse.Modules;

namespace WFCourse.Generation.Cells
{
    public class CellData
    {
        private List<ModuleData> _possibleModules;
        public int TotalWeight { get; set; }
        public float SumOfLogWeight { get; set; }

        public CellData(ModuleData[] moduleDatas)
        {
            _possibleModules = new List<ModuleData>(moduleDatas);
        }

        public CellData(CellData cellData)
        {
            _possibleModules = new List<ModuleData>(cellData.PossibleModules);
            TotalWeight = cellData.TotalWeight;
            SumOfLogWeight = cellData.SumOfLogWeight;
        }

        public ModuleData CollapsedModuleData { get; set; }
        public List<ModuleData> PossibleModules => _possibleModules;
    }
}