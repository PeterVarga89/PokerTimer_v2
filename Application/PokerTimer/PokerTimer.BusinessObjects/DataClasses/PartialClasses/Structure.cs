using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTimer.BusinessObjects.Data;

namespace PokerTimer.BusinessObjects.DataClasses
{
    public partial class Structure
    {
        public List<StructureDetail> Details
        {
            get;
            set;
        }

        public void LoadDetails(eConnectionString cs)
        {
            this.Details = StructureData.GetDetailsByStrucureId(cs, this.StructureId);
        }

    }
}
