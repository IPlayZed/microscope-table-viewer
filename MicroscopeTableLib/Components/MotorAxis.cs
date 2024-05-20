using MicroscopeTableLib.Utilities;

namespace MicroscopeTableLib.Components
{
    public class MotorAxis(GearConfiguration gearConfiguration, double defaultPosition = 0)
    {
        private double CurrentPosition { get; set; } = defaultPosition;
        private Gear Gear { get; set; } = new Gear(gearConfiguration.MaxNumberOfSteps, gearConfiguration.StepSize);

        public double StepGear(uint numberOfSteps, bool increase)
        {
            double distanceMoved = Gear.GetEffectiveMovement(numberOfSteps) * numberOfSteps;
            if (increase)
            {
                Gear.CurrentStep += numberOfSteps;
            }
            else
            {
                Gear.CurrentStep -= numberOfSteps;
                distanceMoved *= -1;
            }

            CurrentPosition += distanceMoved;

            return Gear.GetEffectiveMovement(numberOfSteps);
        }

        // TODO: Check if this calculation is really sensible.
        // TODO: Check if a controller could be used here sensibly.
        public uint MoveTo(double targetPosition)
        {
            double distancePerStep = Gear.StepSize;
            uint closestStep = (uint)Math.Round(targetPosition / distancePerStep);

            closestStep = Math.Max(0, Math.Min(closestStep, Gear.NumberOfSteps));

            double currentEffectiveMovement = Gear.GetEffectiveMovement(Gear.CurrentStep);

            Gear.CurrentStep = closestStep;

            CurrentPosition += currentEffectiveMovement - Gear.GetEffectiveMovement(Gear.CurrentStep);

            return closestStep;
        }

    }

}
