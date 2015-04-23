namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class User
    {
        public string FullDislpayName
        {
            get
            {
                return string.Format("{0} - {1} {2}", this.NickName, this.FirstName, this.LastName);
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
            set { }
        }
    }
}