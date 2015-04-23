using System;
using System.Collections.Generic;
using System.Linq;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.BusinessObjects.Data
{
    public class StructureData
    {
        public static List<Structure> GetList(eConnectionString cs)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.Structures.ToList();
            }
        }

        public static List<StructureDetail> GetDetailsByStrucureId(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.StructureDetails.Where(st => st.StructureId == id).OrderBy(s => s.Number).ToList();
            }
        }

        public static void Insert(eConnectionString cs, Structure entity, List<StructureDetail> details)
        {
            using (var app = new BusinessDataContext(cs))
            {
                if (!IsExist(cs, entity.StructureId))
                {
                    app.Structures.InsertOnSubmit(entity);
                    app.StructureDetails.InsertAllOnSubmit(details);
                    app.SubmitChanges();
                }
                else
                {
                    //TODO: UPDATE
                    //var det = app.TournamentDetails.SingleOrDefault(td => td.TournamentId == entity.TournamentId);
                    //det.AddOnPrize = details.AddOnPrize;
                    //det.AddOnStack = details.AddOnStack;
                    //det.BonusStack = details.BonusStack;
                    //det.BuyInPrize = details.BuyInPrize;
                    //det.BuyInStack = details.BuyInStack;
                    //det.GTD = details.GTD;
                    //det.IsFullPointed = details.IsFullPointed;
                    //det.IsLeague = details.IsLeague;
                    //det.ReBuyCount = details.ReBuyCount;
                    //det.ReBuyPrize = details.ReBuyPrize;
                    //det.ReBuyStack = details.ReBuyStack;
                    //det.ReEntryCount = details.ReEntryCount;
                    //det.StructureId = details.StructureId;

                    //app.SubmitChanges();
                }
            }
        }

        public static bool IsExist(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                var t = app.Structures.SingleOrDefault(u => u.StructureId == id);

                return t.IsNotNull();
            }
        }
    }
}