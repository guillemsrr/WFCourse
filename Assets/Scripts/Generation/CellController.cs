using System.Collections.Generic;
using UnityEngine;
using WFCourse.Modules;
using WFCourse.Utilities;

namespace WFCourse.Generation
{
    public class CellController
    {
        private Transform _parent;
        private ModuleController _errorModule;
        private List<ModuleData> _possibleModules;
        private int _totalWeight;
        private float _sumOfLogWeight;
        private float _entropyNoise;

        public delegate void CellPropagated(CellController cell);
        public event CellPropagated CellPropagatedEvent;
        
        public Vector3Int Position { get; }
        public bool IsErroneus { get; private set; }
        public bool IsCollapsed { get; private set; }
        public bool OnlyOnePossibility => _possibleModules.Count == 1;
        public List<ModuleData> Possibilities => _possibleModules;
        public int CollapsedModuleNumber { get; private set; }
        

        public CellController(Transform parent, ModuleData[] moduleDatas,  ModuleController errorModule,
            Vector3Int position)
        {
            _parent = parent;
            _errorModule = errorModule;
            Position = position;

            _possibleModules = new List<ModuleData>(moduleDatas);
        }

        public void Collapse()
        {
            ModuleData moduleData = GetWeightedRandomModule();
            CollapsedModuleNumber = moduleData.Number;
            ModuleController randomModule = moduleData.ModuleController;
            InstantiateModule(randomModule, moduleData.Rotation);

            IsCollapsed = true;
        }

        private void InstantiateModule(ModuleController randomModule, Rotation rotation)
        {
            ModuleController collapsedModule = Object.Instantiate(randomModule, Position*Vector3Int.one*2, Rotations.QuaternionByRotation[rotation], _parent);
            collapsedModule.transform.name += Position;
        }

        private ModuleData GetWeightedRandomModule()
        {
            int randomWeight = Random.Range(0, _totalWeight + 1);
            foreach (ModuleData possibleModule in _possibleModules)
            {
                randomWeight -= possibleModule.Frequency;
                if (randomWeight <= 0)
                {
                    return possibleModule;
                }
            }

            return _possibleModules[0];
        }

        public void Propagate(Direction direction, int collapsedModuleDataNumber)
        {
            Queue<ModuleData> impossibleModules = new Queue<ModuleData>();
            foreach (ModuleData possibleModule in _possibleModules)
            {
                if (!possibleModule.PersistentPossibleNeighbors.PossibleNeighbors[Directions.FlipDirection(direction)].Contains(collapsedModuleDataNumber))
                {
                    impossibleModules.Enqueue(possibleModule);
                }
            }
            
            foreach (ModuleData impossibleModule in impossibleModules)
            {
                RemoveImpossibleModule(impossibleModule);
            }

            if (_possibleModules.Count == 0)
            {
                IsErroneus = true;
                InstantiateModule(_errorModule, Rotation.Rot0);
            }
            
            CellPropagatedEvent?.Invoke(this);
        }

        private void RemoveImpossibleModule(ModuleData impossibleModule)
        {
            _possibleModules.Remove(impossibleModule);
            _totalWeight -= impossibleModule.Frequency;
            _sumOfLogWeight -= Mathf.Log(impossibleModule.Frequency);
        }

        public float GetEntropy()
        {
            return Mathf.Log(_totalWeight) - _sumOfLogWeight / _totalWeight + _entropyNoise;
        }

        public void SetWeightData(int totalWeight, float logWeight, float noise)
        {
            _totalWeight = totalWeight;
            _sumOfLogWeight = logWeight;
            _entropyNoise = noise;
        }
    }
}