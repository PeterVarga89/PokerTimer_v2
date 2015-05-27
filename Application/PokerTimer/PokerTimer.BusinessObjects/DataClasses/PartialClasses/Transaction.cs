namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class Transaction
    {
        public eTransactionType Type
        {
            get
            {
                return (eTransactionType)this.TransactionType;
            }
            set
            {
                this.TransactionType = (int)value;
            }
        }
    }
}