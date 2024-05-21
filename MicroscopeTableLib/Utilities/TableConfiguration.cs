namespace MicroscopeTableLib.Utilities
{
    public readonly struct TableConfiguration
    {
        public StepperConfiguration StepperConfiguration { get; }
        public Position TablePosition { get; }

        public TableConfiguration(StepperConfiguration stepperConfiguration, Position position)
        {
            StepperConfiguration = stepperConfiguration;
            TablePosition = position;
        }

        public TableConfiguration()
        {
            StepperConfiguration = new StepperConfiguration();
            TablePosition = new Position();
        }
    }
}
