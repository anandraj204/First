using System.Collections.Generic;

namespace Jane.Data.EntityFramework.Entities
{
    public class Wallet :BaseEntity 
    {
        public  Wallet()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        public decimal Credit { get; set; }

        public HashSet<Transaction> Transactions { get; set; }
    }
}
