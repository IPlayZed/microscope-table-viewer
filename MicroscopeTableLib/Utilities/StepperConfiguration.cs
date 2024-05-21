namespace MicroscopeTableLib.Utilities
{
    public readonly struct StepperConfiguration(
        GearConfiguration xGear = new(),
        GearConfiguration yGear = new(),
        GearConfiguration zGear = new())
    {
        public GearConfiguration XGear { get; } = xGear;
        public GearConfiguration YGear { get; } = yGear;
        public GearConfiguration ZGear { get; } = zGear;
    }
}
