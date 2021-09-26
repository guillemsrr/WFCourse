using System.Collections.Generic;
using UnityEngine;
using WFCourse.Modules;

namespace WFCourse.Generation
{
    public class FrequencyController
    {
        public FrequencyController(ICollection<CellController> cells)
        {
            foreach (CellController cell in cells)
            {
                CalculateInitialWeight(cell);
            }
        }

        private void CalculateInitialWeight(CellController cellController)
        {
            int totalWeight = 0;
            float logWeight = 0;
            
            foreach (ModuleData possibleModule in cellController.Possibilities)
            {
                totalWeight += possibleModule.Frequency;
                logWeight += Mathf.Log(possibleModule.Frequency);
            }

            float noise = Random.value / 100f;
            
            cellController.SetWeightData(totalWeight, logWeight, noise);
        }
    }
}