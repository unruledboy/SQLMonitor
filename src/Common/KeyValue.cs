namespace Xnlab.SQLMon.Common
{
    public class KeyValue<TK, TV>
    {
        public TK Key { get; set; }
        public TV Value { get; set; }

        public KeyValue(TK key, TV value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
