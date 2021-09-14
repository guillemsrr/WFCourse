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
        
        public Vector3Int Position { get; }
        public ModuleType Type { get; private set; }
        public bool IsErroneus { get; private set; }
        public bool IsCollapsed { get; private set; }
        public bool OnlyOnePossibility => _possibleModules.Count == 1;

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
            ModuleController randomModule = GetRandomModule();
            ModuleController collapsedModule = InstantiateModule(randomModule);
            Type = collapsedModule.Type;

            IsCollapsed = true;
        }

        private ModuleController InstantiateModule(ModuleController randomModule)
        {
            ModuleController collapsedModule = Object.Instantiate(randomModule, Position, Quaternion.identity, _parent);
            collapsedModule.transform.name += Position;
            return collapsedModule;
        }

        private ModuleController GetRandomModule()
        {
            int randomNumber = Random.Range(0, _possibleModules.Count);
            return _possibleModules[randomNumber];
        }

        public void Propagate(ModuleType collapsedType)
        {
            Queue<ModuleController> impossibleModules = new Queue<ModuleController>();
            foreach (ModuleController possibleModule in _possibleModules)
            {
                if (!possibleModule.CanConnect(collapsedType))
                {
                    impossibleModules.Enqueue(possibleModule);
                }
            }
            
            foreach (ModuleController impossibleModule in impossibleModules)
            {
                _possibleModules.Remove(impossibleModule);
            }

            if (_possibleModules.Count == 0)
            {
                IsErroneus = true;
                InstantiateModule(_errorModule);
            }
        }
    }
}