using MicroscopeTableLib.Exceptions;

namespace MicroscopeTableLib.Components
{
    public class Gear
    {
        public uint NumberOfSteps { get; }

        public double Diameter { get; }

        private double Circumference { get; }

        public double StepSize { get; }

        private uint _currentStep;
        public uint CurrentStep
        {
            get => _currentStep;
            set
            {
                if (value < 0 || value >= NumberOfSteps)
                {
                    throw new InvalidPositionException(string.Format(Resources.Exceptions.InvalidStepPosition, value, NumberOfSteps));
                }
                _currentStep = value;
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
