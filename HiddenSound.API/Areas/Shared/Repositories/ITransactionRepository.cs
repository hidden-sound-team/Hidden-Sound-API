using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAllTransactions();

        Transaction GetTransaction(string code);

        void UpdateTransaction(Transaction authorization);

        void CreateTransaction(Transaction authorization);
    }
}
