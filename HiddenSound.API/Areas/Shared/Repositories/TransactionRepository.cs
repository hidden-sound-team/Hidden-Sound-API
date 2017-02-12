using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public HiddenSoundDbContext DbContext { get; set; }

        public Transaction GetTransaction(string authorizationCode)
        {
            return new Transaction();
            //return DbContext.Transactions.FirstOrDefault(a => a.AuthorizationCode == authorizationCode);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void CreateTransaction(Transaction transaction)
        {
            // DbContext.Transactions.Add(transaction);
            // DbContext.SaveChanges();
        }

        public List<Transaction> GetAllTransactions()
        {
            return new List<Transaction>();
            //return DbContext.Transactions.ToList();
        }
    }
}
