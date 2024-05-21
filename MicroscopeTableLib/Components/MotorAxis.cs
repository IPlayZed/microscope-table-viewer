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
        // TODO: Check if a controller could be used here sensibly.
        public uint MoveTo(double targetPosition)
        {
            double distancePerStep = MotorGear.StepSize;
            uint closestStep = (uint)Math.Round(targetPosition / distancePerStep);

            closestStep = Math.Max(0, Math.Min(closestStep, MotorGear.NumberOfSteps));

            double currentEffectiveMovement = MotorGear.GetEffectiveMovement(MotorGear.CurrentStep);

            MotorGear.CurrentStep = closestStep;

            CurrentPosition += currentEffectiveMovement - MotorGear.GetEffectiveMovement(MotorGear.CurrentStep);

            return closestStep;
        }

    }

}
