using System.Collections.Generic;
using UnityEngine;
using WFCourse.Generation.Cells;

namespace WFCourse.Generation.Constraints
{
    public interface ConstraintApplier
    {
        void ApplyConstraint(Dictionary<Vector3Int, CellController> cells);
    }
}