using System.Collections.Generic;
using UnityEngine;
using WFCourse.Modules;

namespace WFCourse.Generation
{
    public class CellController
    {
        private Transform _parent;
        private ModuleController[] _moduleModels;
        private ModuleController _errorModule;
        private List<ModuleController> _possibleModules;
        private int _totalWeight;
        private float _sumOfLogWeight;
        private float _entropyNoise;

        public delegate void CellPropagated(CellController cell);
        public event CellPropagated CellPropagatedEvent;
        
        public Vector3Int Position { get; }
        public FaceTypeSO TypeSo { get; private set; }
        public bool IsErroneus { get; private set; }
        public bool IsCollapsed { get; private set; }
        public bool OnlyOnePossibility => _possibleModules.Count == 1;
        public List<ModuleController> Possibilities => _possibleModules;
        

        public CellController(Transform parent, ModuleController[] moduleModels,  ModuleController errorModule,
            Vector3Int position)
        {
            _parent = parent;
            _moduleModels = moduleModels;
            _errorModule = errorModule;
            Position = position;

            _possibleModules = new List<ModuleController>(moduleModels);
        }

        public void Collapse()
        {
            ModuleController randomModule = GetWeightedRandomModule();
            ModuleController collapsedModule = InstantiateModule(randomModule);

            IsCollapsed = true;
        }

        private ModuleController InstantiateModule(ModuleController randomModule)
        {
            ModuleController collapsedModule = Object.Instantiate(randomModule, Position, Quaternion.identity, _parent);
            collapsedModule.transform.name += Position;
            return collapsedModule;
        }

        private ModuleController GetWeightedRandomModule()
        {
            int randomWeight = Random.Range(0, _totalWeight + 1);
            foreach (ModuleController possibleModule in _possibleModules)
            {
                randomWeight -= possibleModule.Frequency;
                if (randomWeight <= 0)
                {
                    return possibleModule;
                }
            }

            return _possibleModules[0];
        }

        public void Propagate(FaceTypeSO collapsedTypeSo)
        {
            Queue<ModuleController> impossibleModules = new Queue<ModuleController>();
            foreach (ModuleController possibleModule in _possibleModules)
            {
                /*if (!possibleModule.CanConnect(collapsedTypeSo))
                {
                    impossibleModules.Enqueue(possibleModule);
                }*/
            }
            
            foreach (ModuleController impossibleModule in impossibleModules)
            {
                RemoveImpossibleModule(impossibleModule);
            }

            if (_possibleModules.Count == 0)
            {
                IsErroneus = true;
                InstantiateModule(_errorModule);
            }
        }

        private void RemoveImpossibleModule(ModuleController impossibleModule)
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