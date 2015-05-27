using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class StructureDetail : INotifyPropertyChanged
    {
        public List<SelectableEnum<eStructureLineType>> LevelTypes
        {
            get
            {
                return Enum.GetValues(typeof(eStructureLineType)).Cast<eStructureLineType>().Select(t => new SelectableEnum<eStructureLineType>(t)).ToList();
            }
            private set
            { }
        }

        public string LevelDisplayName { get { return this.Level != 0 ? this.Level.ToString() : ""; } }

        partial void OnSmallBlindChanged()
        {
            BigBlind = 2 * SmallBlind;
            PropertyChanged.Raise(() => BigBlind);
        }

        partial void OnTypeChanged()
        {
            if (this.TypeEnum != eStructureLineType.Level)
            {
                this.Level = 0;
            }

            RefreshVisibility();
        }

        public eStructureLineType TypeEnum
        {
            get
            {
                return (eStructureLineType)this.Type;
            }

            set
            {
                this.Type = (int)value;
            }
        }

        public bool IsLevel
        {
            get
            {
                return TypeEnum == eStructureLineType.Level;
            }
        }

        public void RefreshVisibility()
        {
            PropertyChanged.Raise(() => LevelDisplayName);
            PropertyChanged.Raise(() => IsLevel);
            PropertyChanged.Raise(() => Level);
            PropertyChanged.Raise(() => Number);
            PropertyChanged.Raise(() => SmallBlind);
            PropertyChanged.Raise(() => BigBlind);
            PropertyChanged.Raise(() => Ante);
            PropertyChanged.Raise(() => Time);
        }

    }
}