namespace VSW.Lib.Global.ListItem
{
    public class Item
    {
        public Item(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}