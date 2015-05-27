using System;

namespace PokerTimer.BusinessObjects.DataClasses
{
    public class SelectableEnum<T> where T : struct, IConvertible
    {
        public T Enumerator { get; set; }

        public SelectableEnum(T enumerator)
        {
            Enumerator = enumerator;
        }

        public string DisplayName { get { return Enumerator.GetEnumDescription(); } }

        public int Value
        {
            get
            {
                return Convert.ToInt32(Enum.Parse(typeof(T), Enumerator.ToString()) as Enum);
            }
        }
    }
}