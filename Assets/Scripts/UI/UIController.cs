using System;
using UnityEngine;
using UnityEngine.UI;
using WFCourse.ScriptableObjects;

namespace WFCourse.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button _generationButton;
        [SerializeField] private LevelChannelSO _levelChannel;

        private void Awake()
        {
            _generationButton.onClick.AddListener(Generate);
        }

        private void Generate()
        {
            _levelChannel.RaiseGenerationEvent();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Generate();
            }
        }
    }
}