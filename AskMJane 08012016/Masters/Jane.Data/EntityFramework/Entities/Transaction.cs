using System;

namespace Jane.Data.EntityFramework.Entities
{
    public class Transaction
    {
        public Transaction()
        {
            if (TransactedAt == DateTimeOffset.MinValue)
            {
                TransactedAt = DateTimeOffset.UtcNow;
            }
        }
        public int Id { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal TransactionCharge { get; set; }
        public DateTimeOffset TransactedAt { get; set; }
        public int WalletId { get; set; }
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Wallet Wallet
        {
            get;
            set;
        }
        public TransactionStatusEnum TransactionStatus { get; set; }
    }

    public enum TransactionStatusEnum
    {
        PENDING = 1,
        CLEARED = 2,
        DENIED = 3,
        REVERSED = 4,
        OTHER = 5
    }
    public enum TransactionTypeEnum
    {
        DEPOSIT = 1,
        PURCHASE = 2,
        WITHDRAWL = 3,
        TRANSFER = 4,
        OTHER = 5
    }
}
