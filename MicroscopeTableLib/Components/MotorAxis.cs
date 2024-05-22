using MicroscopeTableLib.Exceptions;
using MicroscopeTableLib.Utilities;

namespace MicroscopeTableLib.Components
{
    public class MotorAxis(GearConfiguration gearConfiguration, double defaultPosition = 0)
    {
        internal double CurrentPosition { get; set; } = defaultPosition;
        private Gear MotorGear { get; set; } = new Gear(gearConfiguration);

        public double StepGear(uint numberOfSteps = 1, StepChange stepChange = StepChange.Increase)
        {
            double distanceMoved = MotorGear.GetEffectiveMovement(numberOfSteps);
            if (stepChange == StepChange.Increase)
            {
                MotorGear.CurrentStep += numberOfSteps;
            }
            else if (stepChange == StepChange.Decrease)
            {
                MotorGear.CurrentStep -= numberOfSteps;
                distanceMoved *= -1;
            }

            CurrentPosition += distanceMoved;

            return MotorGear.GetEffectiveMovement(numberOfSteps);
        }
       

        // TODO: Check if this calculation is really sensible.
        // TODO: Check if a controller (PID or something else) could be used here sensibly.
        public void MoveToTargetPosition(double targetPosition)
        {
            if (Math.Abs(targetPosition - CurrentPosition) < 0.0001) return;
            double delta = Math.Abs(targetPosition - CurrentPosition);
            bool increase = targetPosition > CurrentPosition;
            uint requiredSteps = (uint)Math.Round(delta / MotorGear.StepSize);
            if (requiredSteps == 0)
            {
                throw new DidNotStepException(
                    string.Format(Resources.Exceptions.DidNotStep, delta, MotorGear.StepSize)
                    );
            }
            if (increase)
            {
                MotorGear.CurrentStep += requiredSteps;
                CurrentPosition += MotorGear.GetEffectiveMovement(requiredSteps);
            }
            else
            {
                MotorGear.CurrentStep -= requiredSteps;
                CurrentPosition -= MotorGear.GetEffectiveMovement(requiredSteps);
            }
        }

    }

}
