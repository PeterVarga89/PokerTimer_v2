using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class TournamentDetail : INotifyPropertyChanged
    {
        # region PropertyChanged

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            GTD,
            GTDDisplayName
        }
        
        public void Refresh()
        {
            PropertyChange(Property.GTD);
            PropertyChange(Property.GTDDisplayName);
        }

        # endregion

        public string GTDDisplayName { get { return this.GTD + "€"; } private set { } }
    }
}
