using System;

namespace Jane.Core.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public TransactionTypeModelEnum TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal TransactionCharge { get; set; }
        public DateTimeOffset TransactedAt { get; set; }
        public int WalletId { get; set; }
        public int? OrderId { get; set; }
        public virtual OrderModel Order { get; set; }
        public virtual WalletModel Wallet
        {
            get;
            set;
        }
        public TransactionStatusModelEnum TransactionStatus { get; set; }
    }

    public enum TransactionStatusModelEnum
    {
        PENDING = 1,
        CLEARED = 2,
        DENIED = 3,
        REVERSED = 4,
        OTHER = 5
    }
    public enum TransactionTypeModelEnum
    {
        DEPOSIT = 1,
        PURCHASE = 2,
        WITHDRAWL = 3,
        TRANSFER = 4,
        OTHER = 5
    }
 }

