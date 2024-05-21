namespace MicroscopeTableLib.Utilities
{
    public readonly struct GearConfiguration
    {
        public uint MaxNumberOfSteps { get; }
        public double Diameter { get; }

        public GearConfiguration(uint maxNumberOfSteps, double diameter)
        {
            MaxNumberOfSteps = maxNumberOfSteps;
            Diameter = diameter;
        }

        public GearConfiguration()
        {
            MaxNumberOfSteps = 10;
            Diameter = 1.0;
        }
    }
}
