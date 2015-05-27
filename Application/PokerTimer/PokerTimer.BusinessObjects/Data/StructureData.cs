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
                return app.Structures.Where(s => !s.DateDeleted.HasValue).ToList();
            }
        }

        public static List<StructureDetail> GetDetailsByStrucureId(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.StructureDetails.Where(st => st.StructureId == id).OrderBy(s => s.Number).ToList();
            }
        }

        public static void InsertOrUpdate(eConnectionString cs, Structure entity, List<StructureDetail> details, bool asNew = false)
        {
            if (asNew)
            {
                entity.StructureId = Guid.NewGuid();

                foreach (var d in details)
                {
                    d.StructureId = entity.StructureId;
                    d.StructureDetailId = Guid.NewGuid();
                }
            }

            using (var app = new BusinessDataContext(cs))
            {
                if (!IsExist(cs, entity.StructureId))
                {
                    entity.StructureId = Guid.NewGuid();
                    entity.DateCreated = DateTime.Now;
                    details.ForEach(d => d.StructureId = entity.StructureId);

                    app.Structures.InsertOnSubmit(entity);
                    app.StructureDetails.InsertAllOnSubmit(details);
                    app.SubmitChanges();
                }
                else
                {
                    var dbList = app.StructureDetails.Where(sd => sd.StructureId == entity.StructureId).ToList();

                    foreach (var sd in details)
                    {
                        var detail = app.StructureDetails.SingleOrDefault(d => d.StructureDetailId == sd.StructureDetailId);

                        if (detail != null)
                        {
                            detail.Number = sd.Number;
                            detail.Ante = sd.Ante;
                            detail.BigBlind = sd.BigBlind;
                            detail.Level = sd.Level;
                            detail.SmallBlind = sd.SmallBlind;
                            detail.Time = sd.Time;
                            detail.Type = sd.Type;

                            app.SubmitChanges();
                        }
                        else
                        {
                            app.StructureDetails.InsertOnSubmit(sd);
                            app.SubmitChanges();
                        }
                    }

                    foreach (var dbItem in dbList)
                    {
                        if(!details.Select(d => d.StructureDetailId).Contains(dbItem.StructureDetailId))
                        {
                            app.StructureDetails.DeleteOnSubmit(dbItem);
                            app.SubmitChanges();
                        }
                    }
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

        public static void Delete(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                var toDel = app.Structures.SingleOrDefault(s => s.StructureId == id);
                toDel.DateDeleted = DateTime.Now;
                app.SubmitChanges();
            }
        }
    }
}