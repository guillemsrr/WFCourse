using System.Linq;
using UnityEngine;

namespace WFCourse.Modules
{
    public class ModuleController : MonoBehaviour
    {
        [SerializeField] private ModuleType _type;
        [SerializeField] private ModuleType[] _neighbourTypes;
        [SerializeField] private int _frequency = 1;


        public int Frequency => _frequency;
        public ModuleType Type => _type;

        public bool CanConnect(ModuleType collapsedType)
        {
            if (collapsedType == _type) return true;

            return _neighbourTypes.Contains(collapsedType);
        }
    }
}
