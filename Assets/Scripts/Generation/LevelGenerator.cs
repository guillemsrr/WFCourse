using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WFCourse.Modules;
using WFCourse.ScriptableObjects;

namespace WFCourse.Generation
{
    
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private ModuleController[] _moduleModels;
        [SerializeField] private ModuleController _errorModule;
        [SerializeField] private Vector3Int _gridDimensions = new Vector3Int(5, 5, 1);
        [SerializeField] private LevelChannelSO _levelChannel;

        private Dictionary<Vector3Int, CellController> _cells;
        private WaveFunctionCollapse _waveFunctionCollapse;

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

            _waveFunctionCollapse = new WaveFunctionCollapse(_cells);
            _waveFunctionCollapse.Observe();
        }
        
        private void CreateCells()
        {
            _cells = new Dictionary<Vector3Int, CellController>();
            
            for (int x = 0; x < _gridDimensions.x; x++)
            {
                for (int y = 0; y < _gridDimensions.y; y++)
                {
                    CreateCell(new Vector3Int(x,y,0));
                }
            }
        }

        private void CreateCell(Vector3Int position)
        {
            CellController cellController = new CellController(transform, _moduleModels,  _errorModule, position);
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
