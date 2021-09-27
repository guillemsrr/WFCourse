using System.Collections.Generic;
using UnityEngine;
using WFCourse.Modules;

namespace WFCourse.ScriptableObjects
{
    public class ModulesDataSO:ScriptableObject
    {
        public ModuleData[] ModuleDatas;

        public int[] PerimeterConstraintNumber;

        public void SetData(ModuleData[] moduleData)
        {
            ModuleDatas = moduleData;
        }

        public void SetPerimeterConstraints(List<int> perimeterModuleNumbers)
        {
            PerimeterConstraintNumber = perimeterModuleNumbers.ToArray();
        }
    }
}