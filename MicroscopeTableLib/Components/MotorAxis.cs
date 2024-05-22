using MicroscopeTableLib.Utilities;

namespace MicroscopeTableLib.Components
{
    public class MotorAxis(GearConfiguration gearConfiguration, double defaultPosition = 0)
    {
        internal double CurrentPosition { get; set; } = defaultPosition;
        private Gear MotorGear { get; set; } = new Gear(gearConfiguration);

        public double StepGear(uint numberOfSteps, bool increase)
        {
            double distanceMoved = MotorGear.GetEffectiveMovement(numberOfSteps) * numberOfSteps;
            if (increase)
            {
                MotorGear.CurrentStep += numberOfSteps;
            }
            else
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
            double delta = Math.Abs(targetPosition - CurrentPosition);
            bool increase = targetPosition > CurrentPosition;
            uint requiredSteps = (uint)Math.Round(delta / MotorGear.StepSize);
            if (increase)
            {
                MotorGear.CurrentStep += requiredSteps;
                CurrentPosition += requiredSteps * MotorGear.StepSize;
            }
            else
            {
                MotorGear.CurrentStep -= requiredSteps;
                CurrentPosition -= requiredSteps * MotorGear.StepSize;
            }
        }

    }

}
