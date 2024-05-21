namespace MicroscopeTableLib.Utilities
{
    public readonly struct GearConfiguration
    {
        public uint MaxNumberOfSteps { get; }
        public double StepSize { get; }

        public GearConfiguration(uint maxNumberOfSteps = 10, double stepSize = 1.0)
        {
            MaxNumberOfSteps = maxNumberOfSteps;
            StepSize = stepSize;
        }
    }
}
