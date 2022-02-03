using Balances;
using Service.ServiceInterfaces;
using ServiceModels;
using ServiceModels.TransferModel;

namespace Service.ServiceImplementations
{
    public class GameManagementService : IBaseManagementService
    {
        private readonly IBalanceManager _gameBalanceManager;
        public GameManagementService()
        {
            _gameBalanceManager = new GameBalanceManager();
        }
        public BaseResponseModel EnrollmentMoney(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel { HttpStatusCode = (int)ErrorCode.UnknownError, Message = ErrorCode.UnknownError.ToString() };
            var playerCheckTransaction = _gameBalanceManager.CheckTransaction(request.TransactionId);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel { HttpStatusCode = (int)playerCheckTransaction, Message = playerCheckTransaction.ToString() };
            var increaseResult = _gameBalanceManager.IncreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel { HttpStatusCode = (int)increaseResult, Message = increaseResult.ToString() }; 
        }
        public BaseResponseModel DeductionMoney(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel { HttpStatusCode = (int)ErrorCode.UnknownError, Message = ErrorCode.UnknownError.ToString() };
            var playerCheckTransaction = _gameBalanceManager.CheckTransaction(request.TransactionId);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel { HttpStatusCode = (int)playerCheckTransaction, Message = playerCheckTransaction.ToString() };
            var decreaseResult = _gameBalanceManager.DecreaseBalance(request.Amount, request.TransactionId);
                return new BaseResponseModel { HttpStatusCode = (int)decreaseResult, Message = decreaseResult.ToString() };
        }
        public BaseResponseModel Rollback(TransferBaseRequestModel request)
        {
            var result = _gameBalanceManager.Rollback(request.TransactionId);
            return new BaseResponseModel { HttpStatusCode = (int)result, Message = result.ToString() };
        }
        public decimal GetBalance()
        {
            return _gameBalanceManager.GetBalance();
        }
    }
}
