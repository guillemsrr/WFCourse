using WFCourse.Utilities;

namespace WFCourse.Modules
{
    [System.Serializable]
    public class ModuleData
    {
        public int Number;
        public int Frequency = 1;
        public Rotation Rotation;
        public ModuleController ModuleController;
        public PersistentPossibleNeighbors PersistentPossibleNeighbors;

        public ModuleData(ModuleController moduleController, Rotation rotation, int numberModuleData)
        {
            Number = numberModuleData;
            Rotation = rotation;
            ModuleController = moduleController;
            PersistentPossibleNeighbors = new PersistentPossibleNeighbors();
        }

        public void AddPossibleNeighbor(Direction direction, int possibleNeighbor)
        {
            PersistentPossibleNeighbors.PossibleNeighbors[direction].Add(possibleNeighbor);
        }
    }
}