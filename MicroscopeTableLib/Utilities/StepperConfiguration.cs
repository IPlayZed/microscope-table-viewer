namespace MicroscopeTableLib.Utilities
{
    public readonly struct StepperConfiguration
    {
        public GearConfiguration XGear { get; }
        public GearConfiguration YGear { get; }
        public GearConfiguration ZGear { get; }

        public StepperConfiguration(GearConfiguration xGear, GearConfiguration yGear,
            GearConfiguration zGear)
        {
            XGear = xGear;
            YGear = yGear;
            ZGear = zGear;
        }

        public StepperConfiguration() 
        {
            XGear = new GearConfiguration();
            YGear = new GearConfiguration();
            ZGear = new GearConfiguration();
        }
    }
}
