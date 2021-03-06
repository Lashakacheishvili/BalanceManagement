using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceModels.TransferModel
{
    public class TransferMoneyRequestModel:TransferBaseRequestModel
    {
        public decimal Amount { get; protected set; }
        public TransferMoneyRequestModel(string transactionId,decimal amount):base(transactionId)
        {
            Amount = amount;
        }
    }
}
