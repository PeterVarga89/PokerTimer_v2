using System;
using System.ComponentModel;
using PokerTimer.BusinessObjects.Data;

namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class Tournament : INotifyPropertyChanged
    {
        # region PropertyChanged

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            Name
        }

        private void Refresh()
        {
            PropertyChange(Property.Name);
        }

        # endregion

        public TournamentDetail TournamentDetail { get; set; }

        public Structure Structure { get; set; }

        public void LoadTournamentDetails()
        {
            this.TournamentDetail = TournamentData.GetDetailById(eConnectionString.Online, this.TournamentId);
            this.TournamentDetail.Refresh();
        }

        public eGameType GameTypeEnum
        {
            get
            {
                return this.GameType.ToGameType();
            }
        }

        public string DisplayDateName
        {
            get
            {
                if (this.Date.Date == DateTime.Today.Date)
                {
                    return string.Format("Dnes - {0}", this.Name);
                }
                else
                {
                    return string.Format("{0} - {1}", this.Date.ToString("dd.MM.yyyy"), this.Name);
                }
            }
        }
    }
}