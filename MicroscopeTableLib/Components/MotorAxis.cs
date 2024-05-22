using MicroscopeTableLib.Utilities;

namespace MicroscopeTableLib.Components
{
    public class MotorAxis(GearConfiguration gearConfiguration, double defaultPosition = 0)
    {
        private double CurrentPosition { get; set; } = defaultPosition;
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
            double delta = targetPosition - CurrentPosition;
            bool increase = delta > 0;
            uint requiredSteps = (uint)Math.Round(delta / MotorGear.StepSize);
            if (increase)
            {
                MotorGear.CurrentStep += requiredSteps;
                CurrentPosition += requiredSteps * MotorGear.StepSize;
                int a = 1;
            }
            else
            {
                MotorGear.CurrentStep -= requiredSteps;
                CurrentPosition -= requiredSteps * MotorGear.StepSize;
                int a = 1;
            }
        }

    }

}
