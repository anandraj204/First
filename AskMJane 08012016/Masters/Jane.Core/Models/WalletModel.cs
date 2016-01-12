using System.Collections.Generic;

namespace Jane.Core.Models
{
    public class WalletModel : BaseModel
    {
        public decimal Credit { get; set; }

        public List<TransactionModel> Transactions { get; set; }
    }
}
