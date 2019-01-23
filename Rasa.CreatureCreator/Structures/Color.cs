namespace Rasa.Structures
{
    public class Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }

        public Color(System.Drawing.Color color)
        {
            Red = color.R;
            Green = color.G;
            Blue = color.B;
            Alpha = color.A;
        }

        public Color(byte red, byte green, byte blue, byte alpha = 0xFF)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public uint Hue => (uint)((Alpha << 24) | (Blue << 16) | (Green << 8) | Red);

        public Color(uint hue)
        : this((byte)(hue & 0xFF), (byte)((hue >> 8) & 0xFF), (byte)((hue >> 16) & 0xFF), (byte)((hue >> 24) & 0xFF))
        {
        }
    }
}
