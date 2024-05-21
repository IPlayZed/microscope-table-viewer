namespace MicroscopeTableLib.Utilities
{
    public readonly struct TableConfiguration(
        StepperConfiguration stepperConfiguration = new(),
        Position tablePosition = new())
    {
        public StepperConfiguration StepperConfiguration { get; } = stepperConfiguration;
        public Position TablePosition { get; } = tablePosition;
    }
}
