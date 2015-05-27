using System;
using System.Linq;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.BusinessObjects.Data
{
    public class TransactionData
    {
        public static void Create(Transaction entity)
        {
            Create(eConnectionString.Online, entity);
        }

        private static void Create(eConnectionString connectionString, Transaction entity)
        {
            if (!TransactionData.HasAnyTrasaction(entity.UserId))
            {
                UserData.SetUserRefactoringType(entity.UserId, eAutoReturnState.Full);
            }

            if (entity.Amount < 0)
            {
                entity.Amount2 = entity.Amount;
            }

            entity.DateCreated = DateTime.Now;
            entity.DateAddedToDB = DateTime.Now;
            entity.TransactionId = Guid.NewGuid();
            entity.CratedByApplication = 1;

            using (var app = new BusinessDataContext(connectionString))
            {
                app.Transactions.InsertOnSubmit(entity);
                app.SubmitChanges();
            }
        }

        public static double GetBonus(Guid userId)
        {
            using (var app = new BusinessDataContext(eConnectionString.Online))
            {
                var result = app.Transactions.Where(t => t.UserId == userId && t.TransactionType == (int)eTransactionType.Bonus && !t.DateUsed.HasValue && !t.DatePayed.HasValue).ToList();

                if (result.IsNotNull() && result.Count > 0)
                {
                    return result.Sum(t => t.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static bool HasAnyTrasaction(Guid userId)
        {
            using (var app = new BusinessDataContext(eConnectionString.Online))
            {
                var transactions = app.Transactions.Where(t => t.UserId == userId && (eTransactionType)t.TransactionType != eTransactionType.Bonus && (eTransactionType)t.TransactionType != eTransactionType.BonusReturned);

                return transactions.Count() > 0;
            }
        }

        public static void HandleRefactoring(Guid userId, double refactorAmount, Guid loggedInUserId, string description = null)
        {
            if (description == null || description == string.Empty)
            {
                description = "Stiahnuté z výhry";
            }

            if (refactorAmount != 0)
            {
                using (var app = new BusinessDataContext(eConnectionString.Online))
                {
                    var tranList = app.Transactions.Where(t => t.UserId == userId && (t.TransactionType == (int)eTransactionType.Bar || t.TransactionType == (int)eTransactionType.CashGame || t.TransactionType == (int)eTransactionType.Tournament || t.TransactionType == (int)eTransactionType.NotSet) && !t.DateDeleted.HasValue && !t.DatePayed.HasValue).OrderByDescending(t => t.Amount2).ToList();

                    var availableAmount = refactorAmount;
                    foreach (var t in tranList)
                    {
                        if (availableAmount > 0 && t.Amount2.HasValue)
                        {
                            var originalAmount2 = t.Amount2;

                            if (t.Amount2.Value.ToAbs() > availableAmount.ToAbs())
                            {
                                t.Amount2 = t.Amount2 + availableAmount;
                                var transaction = new Transaction() { Amount = availableAmount, AttachedTransactionId = t.TransactionId, CratedByApplication = 1, UserId = t.UserId, CratedByUserId = loggedInUserId, TransactionType = (int)t.Type.GetOposite(), Description = description };
                                Create(transaction);
                                availableAmount = 0;
                                app.SubmitChanges();
                                CreateMail(transaction, UserData.GetUserBalance(transaction.UserId));
                            }
                            else
                            {
                                t.Amount2 = 0;
                                t.DatePayed = DateTime.Now;
                                var transaction = new Transaction() { Amount = originalAmount2.ToAbs(), AttachedTransactionId = t.TransactionId, CratedByApplication = 1, UserId = t.UserId, CratedByUserId = loggedInUserId, TransactionType = (int)t.Type.GetOposite(), Description = description };
                                Create(transaction);
                                availableAmount = availableAmount - originalAmount2.ToAbs();
                                app.SubmitChanges();
                                CreateMail(transaction, UserData.GetUserBalance(transaction.UserId));
                            }
                        }
                    }
                }
            }
        }

        public static void CreateMail(Transaction transaction, double balance)
        {
            var user = UserData.GetById(eConnectionString.Local, transaction.UserId);
            var admin = UserData.GetById(eConnectionString.Local, transaction.CratedByUserId);

            var subject = "";
            var body = "";

            if (transaction.Type == eTransactionType.Bonus || transaction.Type == eTransactionType.BonusReturned)
            {
                subject = Mailer.Constants.MailNewBonusSubject;
                body = string.Format(Mailer.Constants.MailNewBonusBody, user.NickName, user.FirstName, user.LastName, "Tournament", transaction.Amount, balance);
            }
            else
            {
                subject = transaction.Amount > 0 ? Mailer.Constants.MailNewReplySubject : Mailer.Constants.MailNewBorrowSubject;

                body = string.Format(
                Mailer.Constants.MailNewBorrowBody,
                user.NickName,
                user.FirstName,
                user.LastName,
                "Cash Game",
                transaction.Amount,
                balance,
                admin.FullName);
            }

            Mailer.Mailer.SendMail(subject, body);
        }
    }
}