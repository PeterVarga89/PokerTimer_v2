using System;
using System.Collections.Generic;
using System.Linq;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.BusinessObjects.Data
{
    public class TournamentData
    {
        public static List<Tournament> GetList(eConnectionString cs, int take = 5)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.Tournaments.Where(t => t.GameType != 'C' && !t.DateDeleted.HasValue).OrderByDescending(t => t.Date).Take(take).ToList();
            }
        }

        public static List<Tournament> GetFutureList(eConnectionString cs, int take = 5)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.Tournaments.Where(t => t.GameType != 'C' && t.Date > DateTime.Now.AddDays(-2) && !t.DateDeleted.HasValue).OrderByDescending(t => t.Date).Take(take).ToList();
            }
        }

        public static Tournament GetById(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.Tournaments.SingleOrDefault(u => u.TournamentId == id);
            }
        }

        public static TournamentDetail GetDetailById(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.TournamentDetails.SingleOrDefault(u => u.TournamentId == id);
            }
        }

        public static void Insert(eConnectionString cs, Tournament entity, TournamentDetail details)
        {
            using (var app = new BusinessDataContext(cs))
            {
                if (!IsExist(cs, entity.TournamentId))
                {
                    entity.Description = entity.Description ?? string.Empty;
                    app.Tournaments.InsertOnSubmit(entity);
                    app.TournamentDetails.InsertOnSubmit(details);
                    app.SubmitChanges();
                }
                else
                {
                    var det = app.TournamentDetails.SingleOrDefault(td => td.TournamentId == entity.TournamentId);
                    det.AddOnPrize = details.AddOnPrize;
                    det.AddOnStack = details.AddOnStack;
                    det.BonusStack = details.BonusStack;
                    det.BuyInPrize = details.BuyInPrize;
                    det.BuyInStack = details.BuyInStack;
                    det.GTD = details.GTD;
                    det.IsFullPointed = details.IsFullPointed;
                    det.IsLeague = details.IsLeague;
                    det.ReBuyCount = details.ReBuyCount;
                    det.ReBuyPrize = details.ReBuyPrize;
                    det.ReBuyStack = details.ReBuyStack;
                    det.ReEntryCount = details.ReEntryCount;
                    det.StructureId = details.StructureId;

                    app.SubmitChanges();
                }
            }
        }

        public static bool IsExist(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                var t = app.Tournaments.SingleOrDefault(u => u.TournamentId == id);
                return t.IsNotNull();
            }
        }
    }
}