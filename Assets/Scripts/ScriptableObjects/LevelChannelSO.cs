using UnityEngine;
using UnityEngine.Events;

namespace WFCourse.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Channel/Level")]
    public class LevelChannelSO: ScriptableObject
    {
        public UnityAction GenerationEvent;

        public void RaiseGenerationEvent()
        {
            GenerationEvent?.Invoke();
        }
    }
}