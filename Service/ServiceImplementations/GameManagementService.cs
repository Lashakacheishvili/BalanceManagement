using Balances;
using Service.ServiceInterfaces;
using ServiceModels;
using ServiceModels.TransferModel;

namespace Service.ServiceImplementations
{
    public class GameManagementService : IBaseManagementService
    {
        private readonly IBalanceManager _gameBalanceManager;
        private readonly IBalanceManager _casinoBalanceManager;
        public GameManagementService()
        {
            _gameBalanceManager = new GameBalanceManager();
            _casinoBalanceManager = new CasinoBalanceManager();
        }
        public decimal GetBalancePlayer()
        {
            return _gameBalanceManager.GetBalance();
        }
        public decimal GetBalanceCasino()
        {
            return _casinoBalanceManager.GetBalance();
        }
        public BaseResponseModel TransferMoneyToPlayer(TransferMoneyRequestModel request)
        {
            var deduction = DeductionMoneyCasino(request);
            if (deduction.HttpStatusCode == 0)
            {
                var enrollment = EnrollmentMoneyPlayer(request);
                return enrollment.HttpStatusCode == 0 ? enrollment : RollbackCasino(new ServiceModels.TransferModel.TransferBaseRequestModel(request.TransactionId));
            }
            return deduction;
        }
        public BaseResponseModel TransferMoneyToCasino(TransferMoneyRequestModel request)
        {
            var deduction = DeductionMoneyPlayer(request);
            if (deduction.HttpStatusCode == 0)
            {
                var enrollment = EnrollmentMoneyCasino(request);
                return enrollment.HttpStatusCode == 0 ? enrollment : RollbackPlayer(new ServiceModels.TransferModel.TransferBaseRequestModel(request.TransactionId));
            }
            return deduction;
        }
        #region Player
        BaseResponseModel EnrollmentMoneyPlayer(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel((int)ErrorCode.UnknownError, ErrorCode.UnknownError.ToString());
            var playerCheckTransaction = _gameBalanceManager.CheckTransaction(request.TransactionId);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel((int)playerCheckTransaction, playerCheckTransaction.ToString());
            var increaseResult = _gameBalanceManager.IncreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel((int)increaseResult, increaseResult.ToString());
        }
        BaseResponseModel DeductionMoneyPlayer(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel((int)ErrorCode.UnknownError, ErrorCode.UnknownError.ToString());
            var playerCheckTransaction = _gameBalanceManager.CheckTransaction(request.TransactionId);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel((int)playerCheckTransaction, playerCheckTransaction.ToString());
            var decreaseResult = _gameBalanceManager.DecreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel((int)decreaseResult, decreaseResult.ToString());
        }
        BaseResponseModel RollbackPlayer(TransferBaseRequestModel request)
        {
            var result = _gameBalanceManager.Rollback(request.TransactionId);
            if (result.Equals(ErrorCode.Success))
                return new BaseResponseModel((int)ErrorCode.TransactionRejected, ErrorCode.TransactionRejected.ToString());
            return new BaseResponseModel((int)result, result.ToString());
        }
        #endregion
        #region Casino
        BaseResponseModel EnrollmentMoneyCasino(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel((int)ErrorCode.UnknownError, ErrorCode.UnknownError.ToString());
            var checkTransaction = _casinoBalanceManager.CheckTransaction(request.TransactionId);
            if (!checkTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel((int)checkTransaction, checkTransaction.ToString());
            var increaseResult = _casinoBalanceManager.IncreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel((int)increaseResult, increaseResult.ToString());
        }
        BaseResponseModel DeductionMoneyCasino(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return new BaseResponseModel((int)ErrorCode.UnknownError, ErrorCode.UnknownError.ToString());
            var checkTransaction = _casinoBalanceManager.CheckTransaction(request.TransactionId);
            if (!checkTransaction.Equals(ErrorCode.Success))
                return new BaseResponseModel((int)checkTransaction, checkTransaction.ToString());
            var decreaseResult = _casinoBalanceManager.DecreaseBalance(request.Amount, request.TransactionId);
            return new BaseResponseModel((int)decreaseResult, decreaseResult.ToString());
        }
        BaseResponseModel RollbackCasino(TransferBaseRequestModel request)
        {
            var result = _casinoBalanceManager.Rollback(request.TransactionId);
            if (result.Equals(ErrorCode.Success))
                return new BaseResponseModel((int)ErrorCode.TransactionRejected, ErrorCode.TransactionRejected.ToString());
            return new BaseResponseModel((int)result, result.ToString());
        }
        #endregion
    }
}
