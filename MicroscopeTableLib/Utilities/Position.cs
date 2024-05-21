namespace MicroscopeTableLib.Utilities
{
    public struct Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Position(double x = 0, double y = 0, double z = 0) => (X, Y, Z) = (x, y, z);

        public override readonly string ToString() => $"({X}, {Y}, {Z})";
    }
}
