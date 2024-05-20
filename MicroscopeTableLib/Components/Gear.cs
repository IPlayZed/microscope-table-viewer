using MicroscopeTableLib.Exceptions;

namespace MicroscopeTableLib.Components
{
    internal class Gear
    {
        public uint NumberOfSteps { get; }

        public double Diameter { get; }

        private double Circumference { get; }

        public double StepSize { get; }

        public uint CurrentStep
        {
            get => CurrentStep;
            set
            {
                if (value < 0 || value >= NumberOfSteps)
                {
                    throw new InvalidPositionException(string.Format(Resources.Exceptions.InvalidStepPosition, value, NumberOfSteps));
                }
            }
        }

        public Gear(uint numberOfSteps, double diameter)
        {
            (NumberOfSteps, Diameter) = (numberOfSteps, diameter);

            Circumference = double.Pi * Diameter;

            StepSize = Circumference / NumberOfSteps;
        }


        public double GetEffectiveMovement(uint steps) => StepSize * steps;
    }
}
