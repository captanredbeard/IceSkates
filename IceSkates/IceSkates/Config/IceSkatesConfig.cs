using Vintagestory.API.MathTools;

namespace IceSkates.Config
{
    public class IceSkatesConfig
    {
        public bool EnableSkateDrag { get; set; } = true;
        public float GlobalStakeDragModifier { get; set; } = 1;
        public float WalkSpeedReductionWearingSkates { get; set; } = 0.25f; 
        public bool DebugMode { get; set; } = false;
    }
}