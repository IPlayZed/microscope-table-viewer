namespace MicroscopeTableLib.Utilities
{
    public readonly struct StepperConfiguration
    {
        public GearConfiguration XGear { get; }
        public GearConfiguration YGear { get; }
        public GearConfiguration ZGear { get; }
    }
}
