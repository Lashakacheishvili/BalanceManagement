using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceModels.TransferModel
{
    public class TransferBaseRequestModel
    {
        public string TransactionId { get; protected set; }
        public TransferBaseRequestModel(string transactionId)
        {
            TransactionId = transactionId;
        }

    }
}
