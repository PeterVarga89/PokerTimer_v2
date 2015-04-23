using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTimer.BusinessObjects.DataClasses
{
    public class BusinessDataContext  : CADBDataContext
    {
        public BusinessDataContext(eConnectionString cs)
            : base(cs.GetEnumDescription())
        {
            DeferredLoadingEnabled = false;
        }
    }
}
