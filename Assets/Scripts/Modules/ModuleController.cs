using System.Linq;
using UnityEngine;

namespace WFCourse.Modules
{
    public class ModuleController : MonoBehaviour
    {
        [SerializeField] private ModuleType _type;
        [SerializeField] private ModuleType[] _neighbourTypes;
        
        public ModuleType Type => _type;

        public bool CanConnect(ModuleType collapsedType)
        {
            if (collapsedType == _type) return true;

            return _neighbourTypes.Contains(collapsedType);
        }
    }
}
