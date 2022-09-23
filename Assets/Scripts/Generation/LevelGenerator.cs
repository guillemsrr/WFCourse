using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WFCourse.Generation.Cells;
using WFCourse.Generation.Constraints;
using WFCourse.Generation.Waves;
using WFCourse.Modules;
using WFCourse.ScriptableObjects;
using Random = UnityEngine.Random;

namespace WFCourse.Generation
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private ModulesDataSO _modulesDataSo;
        [SerializeField] private Vector3Int _gridDimensions = new Vector3Int(5, 5, 1);
        [SerializeField] private LevelChannelSO _levelChannel;
        [SerializeField] private Transform _generationParent;
        [SerializeField] private float _moduleSize = 2f;
        [SerializeField] private int _seed = 0;
        [SerializeField] private bool _randomSeed = false;

        private Wave _wave;
        private WaveFunctionCollapse _waveFunctionCollapse;
        private FrequencyController _frequencyController;
        private List<ConstraintApplier> _constraints;
        private ModuleData[] _modulesDatas;

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
            SetRandom();
            CreateCells();
            InitializeSubClasses();
            ApplyConstraints();
            _waveFunctionCollapse.Observe();
            DrawCells();
            CenterPivot();
        }

        private void SetRandom()
        {
            if (_randomSeed)
            {
                _seed = Random.Range(0, int.MaxValue);
            }
            
            Random.InitState(_seed);
        }

        private void DrawCells()
        {
            foreach (CellController cell in _wave.Cells.Values)
            {
                if (!cell.IsCollapsed)
                {
                    Debug.LogError("Trying to draw uncollapsed cell");
                    continue;
                }

                cell.InstantiateModule();
            }
        }

        private void ApplyConstraints()
        {
            foreach (ConstraintApplier constraintApplier in _constraints)
            {
                constraintApplier.ApplyConstraint(_wave.Cells);
            }
        }

        private void InitializeSubClasses()
        {
            _constraints = new List<ConstraintApplier>()
            {
                new PerimeterConstraint(_modulesDataSo.PerimeterConstraintNumbers, _gridDimensions)
            };

            _frequencyController = new FrequencyController();
            _frequencyController.SetSpecificElementRandomFrequency(_modulesDatas, _modulesDataSo.AirIndex);
            _frequencyController.SetOneRandomElementHighFrequency(_modulesDatas);
            _frequencyController.CalculateInitialWeight(_wave.Cells.Values);
            
            _waveFunctionCollapse = new WaveFunctionCollapse(_wave);
        }

        private void CreateCells()
        {
            _wave = new Wave();
            _modulesDatas = new ModuleData[_modulesDataSo.ModuleDatas.Length];
            for(int i = 0; i< _modulesDataSo.ModuleDatas.Length; i++)
            {
                _modulesDatas[i] = new ModuleData(_modulesDataSo.ModuleDatas[i]);
            }

            //_modulesDatas = _modulesDataSo.ModuleDatas;

            for (int x = 0; x < _gridDimensions.x; x++)
            {
                for (int y = 0; y < _gridDimensions.y; y++)
                {
                    for (int z = 0; z < _gridDimensions.z; z++)
                    {
                        CreateCell(new Vector3Int(x, y, z));
                    }
                }
            }
        }

        private void CreateCell(Vector3Int position)
        {
            CellController cellController =
                new CellController(_generationParent, _modulesDatas, position, _moduleSize);
            _wave.Cells[position] = cellController;
        }

        private void Reset()
        {
            while (_generationParent.childCount != 0)
            {
                DestroyImmediate(_generationParent.GetChild(0).gameObject);
            }

            _wave.Cells.Clear();
        }

        private void CenterPivot()
        {
            _generationParent.position =
                -new Vector3(_gridDimensions.x / 2f, _gridDimensions.y / 2f, _gridDimensions.z / 2f) * _moduleSize;
            _generationParent.position += Vector3.one * _moduleSize / 2;
        }
    }
}