using UnityEditor;
using UnityEngine;

namespace WFCourse.Editor
{
    [CustomEditor(typeof(ModuleDataBuilder))]
    public class ModuleDataBuilderEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            ModuleDataBuilder moduleDataBuilder = (ModuleDataBuilder)target;

            if (GUILayout.Button("Extract Modules Data"))
            {
                moduleDataBuilder.CreateModulesData();
            }
        }
    }
}