using System.Collections.Generic;
using UnityEngine;

namespace WFCourse.Generation.Constraints
{
    public interface ConstraintApplier
    {
        void ApplyConstraint(Dictionary<Vector3Int, CellController> cells);
    }
}