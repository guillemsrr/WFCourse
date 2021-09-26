using UnityEngine;

namespace WFCourse.Modules
{
    public class ModuleController : MonoBehaviour
    {
        [SerializeField] private Face[] _faces;
        [SerializeField] private int _frequency = 1;


        public int Frequency => _frequency;
        
    }
}
