using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WFCourse.Generation.Constraints;
using WFCourse.Modules;
using WFCourse.ScriptableObjects;
using Random = UnityEngine.Random;

namespace WFCourse.Generation
{
    
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private ModulesDataSO _modulesDataSo;
        [SerializeField] private ModuleController _errorModule;
        [SerializeField] private Vector3Int _gridDimensions = new Vector3Int(5, 5, 1);
        [SerializeField] private LevelChannelSO _levelChannel;

        private Dictionary<Vector3Int, CellController> _cells;
        private WaveFunctionCollapse _waveFunctionCollapse;
        private FrequencyController _frequencyController;
        private List<ConstraintApplier> _constraints;

        private void Awake()
        {
            _levelChannel.GenerationEvent += Generate;
        }

        private void Start()
        {
            GenerateLevel();
        }

        private void Generate()
        {
            Reset();
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            CreateCells();
            InitializeConstraints();
            ApplyConstraints();
            _frequencyController = new FrequencyController(_cells.Values);
            _waveFunctionCollapse = new WaveFunctionCollapse(_cells);
            
            
            _waveFunctionCollapse.Observe();
        }

        private void ApplyConstraints()
        {
            foreach (ConstraintApplier constraintApplier in _constraints)
            {
                constraintApplier.ApplyConstraint(_cells);
            }
        }

        private void InitializeConstraints()
        {
            _constraints = new List<ConstraintApplier>()
            {
                new PerimeterConstraint(_modulesDataSo.PerimeterConstraintNumber, _gridDimensions)
            };
        }

        private void CreateCells()
        {
            _cells = new Dictionary<Vector3Int, CellController>();
            
            for (int x = 0; x < _gridDimensions.x; x++)
            {
                for (int y = 0; y < _gridDimensions.y; y++)
                {
                    for (int z = 0; z < _gridDimensions.z; z++)
                    {
                        CreateCell(new Vector3Int(x,y,z));
                    }
                }
            }
        }

        private void CreateCell(Vector3Int position)
        {
            CellController cellController = new CellController(transform, _modulesDataSo.ModuleDatas,  _errorModule, position);
            _cells[position] = cellController;
        }

        private void Reset()
        {
            while (transform.childCount != 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            
            _cells.Clear();
        }
    }
}
