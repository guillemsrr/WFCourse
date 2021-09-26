using UnityEngine;
using WFCourse.Modules;

namespace WFCourse.ScriptableObjects
{
    public class ModulesDataSO:ScriptableObject
    {
        public ModuleData[] ModuleDatas;

        public void SetData(ModuleData[] moduleData)
        {
            ModuleDatas = moduleData;
        }
    }
}