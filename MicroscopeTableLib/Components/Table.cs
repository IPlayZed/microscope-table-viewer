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
            UpdateTablePosition(MotorAxisX, position.X);   
            UpdateTablePosition(MotorAxisY, position.Y);   
            UpdateTablePosition(MotorAxisZ, position.Z);   
        }

        private void UpdateTablePosition(MotorAxis motorAxis, double position)
        {
            motorAxis.MoveToTargetPosition(position);
            TablePosition = new(MotorAxisX.CurrentPosition, MotorAxisY.CurrentPosition, MotorAxisZ.CurrentPosition);
        }
    }
}
