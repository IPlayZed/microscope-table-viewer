using MicroscopeTableLib.Utilities;

namespace MicroscopeTableLib.Components
{
    public class Table(TableConfiguration tableConfiguration)
    {
        public MotorAxis MotorAxisX { get; } = new MotorAxis(tableConfiguration.StepperConfiguration.XGear);

        public MotorAxis MotorAxisY { get; } = new MotorAxis(tableConfiguration.StepperConfiguration.YGear);

        public MotorAxis MotorAxisZ { get; } = new MotorAxis(tableConfiguration.StepperConfiguration.ZGear);

        public Position TablePosition { get; set; } = tableConfiguration.TablePosition;

        public Table() : this(new TableConfiguration()) { }

        public void MoveTableTo(Position position)
        {
            MotorAxisX.MoveTo(position.X);
            MotorAxisY.MoveTo(position.Y);
            MotorAxisZ.MoveTo(position.Z);
        }
    }
}
