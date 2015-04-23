namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class TournamentResult
    {
        public User User { get; set; }

        public string FullDislpayName
        {
            get
            {
                return string.Format("{0} - {1} {2}", this.User.NickName, this.User.FirstName, this.User.LastName);
            }
        }

        public int BonusStackTotal { get; set; }
    }
}