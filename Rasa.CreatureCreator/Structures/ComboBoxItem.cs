namespace Rasa.Structures
{
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public uint Value { get; set; }

        public ComboBoxItem(string text, uint value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
