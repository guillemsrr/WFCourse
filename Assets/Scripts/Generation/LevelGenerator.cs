using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WFCourse.Generation.Cells;
using WFCourse.Generation.Constraints;
using WFCourse.Generation.Waves;
using WFCourse.ScriptableObjects;

namespace WFCourse.Generation
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private ModulesDataSO _modulesDataSo;
        [SerializeField] private Vector3Int _gridDimensions = new Vector3Int(5, 5, 1);
        [SerializeField] private LevelChannelSO _levelChannel;
        [SerializeField] private Transform _generationParent;
        [SerializeField] private float _moduleSize = 2f;

        private Wave _wave;
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
            InitializeSubClasses();
            ApplyConstraints();
            _waveFunctionCollapse.Observe();
            DrawCells();
            CenterPivot();
        }

        private void DrawCells()
        {
            foreach (CellController cell in _wave.Cells.Values)
            {
                if (!cell.IsCollapsed) continue;

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
                new PerimeterConstraint(_modulesDataSo.PerimeterConstraintNumber, _gridDimensions)
            };

            _frequencyController = new FrequencyController(_wave.Cells.Values);
            _waveFunctionCollapse = new WaveFunctionCollapse(_wave);
        }

        private void CreateCells()
        {
            _wave = new Wave();

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
                new CellController(_generationParent, _modulesDataSo.ModuleDatas, position, _moduleSize);
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