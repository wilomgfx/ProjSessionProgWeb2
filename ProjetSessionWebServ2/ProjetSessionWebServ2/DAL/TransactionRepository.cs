using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class TransactionRepository: GenericRepository<Transaction>
    {
        public TransactionRepository(ApplicationDbContext context) : base(context) { }
        public Transaction ObtenirTransactionParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Transaction> ObtenirTransactions()
            {
                return Get();
            }
        public void InsertTransaction(Transaction Transaction) { Insert(Transaction); }
        public void DeleteTransaction(Transaction Transaction) { Delete(Transaction); }
        public void UpdateTransaction(Transaction Transaction) { Update(Transaction); }
    }
}
