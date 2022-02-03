using Balances;
using Service.ServiceInterfaces;
using ServiceModels;
using ServiceModels.TransferModel;
using System;

namespace Service.ServiceImplementations
{
    public class CasinoManagementService : IBaseManagementService
    {
        private readonly IBalanceManager _casinoBalanceManager;
        public CasinoManagementService()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
        }
        public BaseResponseModel EnrollmentMoney(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel { HttpStatusCode = (int)ErrorCode.UnknownError, Message = ErrorCode.UnknownError.ToString() };
            var checkTransaction = _casinoBalanceManager.CheckTransaction(request.TransactionId);
            if (!checkTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel { HttpStatusCode = (int)checkTransaction, Message = checkTransaction.ToString() };
            var increaseResult = _casinoBalanceManager.IncreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel { HttpStatusCode = (int)increaseResult, Message = increaseResult.ToString() };
        }
        public BaseResponseModel DeductionMoney(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel { HttpStatusCode = (int)ErrorCode.UnknownError, Message = ErrorCode.UnknownError.ToString() };
            var checkTransaction = _casinoBalanceManager.CheckTransaction(request.TransactionId);
            if (!checkTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel { HttpStatusCode = (int)checkTransaction, Message = checkTransaction.ToString() };
            var decreaseResult = _casinoBalanceManager.DecreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel { HttpStatusCode = (int)decreaseResult, Message = decreaseResult.ToString() };
        }
        public BaseResponseModel Rollback(TransferBaseRequestModel request)
        {
            var result = _casinoBalanceManager.Rollback(request.TransactionId);
            return new BaseResponseModel { HttpStatusCode = (int)result, Message = ErrorCode.TransactionRejected.ToString() };
        }
        public decimal GetBalance()
        {
            return _casinoBalanceManager.GetBalance();
        }
    }
}
