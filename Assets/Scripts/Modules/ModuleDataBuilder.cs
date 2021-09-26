using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WFCourse.Modules;
using WFCourse.ScriptableObjects;
using WFCourse.Utilities;

namespace WFCourse
{
    public class ModuleDataBuilder : MonoBehaviour
    {
        private const string MODULE_DATA_PATH = "ModuleData/pipesModuleData";
        
        [SerializeField] private ModuleController[] _moduleControllers;

        private readonly ModuleRotationChecker _moduleRotationChecker = new ModuleRotationChecker();
        private readonly ModuleConnectionChecker _moduleConnectionChecker = new ModuleConnectionChecker();

        public void CreateModulesData()
        {
            List<ModuleData> moduleDatas = ExtractDataFromModules();
            SetPossibleNeighbors(moduleDatas);
            SaveModuleDatas(moduleDatas);
            
            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
        }

        private List<ModuleData> ExtractDataFromModules()
        {
            int numberModuleData = 0;
            List<ModuleData> moduleDatas = new List<ModuleData>();
            foreach (ModuleController moduleController in _moduleControllers)
            {
                List<Rotation> differentRotations = _moduleRotationChecker.GetAllDifferentRotations(moduleController);
                foreach (Rotation rotation in differentRotations)
                {
                    ModuleData moduleData = new ModuleData(moduleController, rotation, numberModuleData);
                    moduleDatas.Add(moduleData);
                    numberModuleData++;
                }
            }

            return moduleDatas;
        }

        private void SetPossibleNeighbors(List<ModuleData> moduleDatas)
        {
            foreach (ModuleData moduleData in moduleDatas)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    foreach (ModuleData possibleNeighbor in moduleDatas)
                    {
                        if (_moduleConnectionChecker.CanConnect(moduleData, possibleNeighbor, direction))
                        {
                            moduleData.AddPossibleNeighbor(direction, possibleNeighbor.Number);
                        }
                    }
                }
            }
        }

        private void SaveModuleDatas(List<ModuleData> moduleDatas)
        {
            ModulesDataSO modulesDataSo = ScriptableObject.CreateInstance<ModulesDataSO>();
            modulesDataSo.SetData(moduleDatas.ToArray());
            ScriptableObjectGenerator.SaveAsset(MODULE_DATA_PATH , modulesDataSo);
        }
    }
}
