using Balances;
using Service.ServiceInterfaces;
using ServiceModels;
using ServiceModels.TransferModel;
using System;

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
            if (deduction.Equals(ErrorCode.Success))
            {
                var enrollment = EnrollmentMoneyPlayer(request);
                return enrollment.Equals(ErrorCode.Success) ? new BaseResponseModel((int)enrollment, enrollment.ToString()) : RollbackCasino(new ServiceModels.TransferModel.TransferBaseRequestModel(request.TransactionId));
            }
            return new BaseResponseModel((int)deduction, deduction.ToString());
        }
        public BaseResponseModel TransferMoneyToCasino(TransferMoneyRequestModel request)
        {
            var deduction = DeductionMoneyPlayer(request);
            if (deduction.Equals(ErrorCode.Success))
            {
                var enrollment = EnrollmentMoneyCasino(request);
                return enrollment.Equals(ErrorCode.Success) ? new BaseResponseModel((int)enrollment, enrollment.ToString()) : RollbackPlayer(new ServiceModels.TransferModel.TransferBaseRequestModel(request.TransactionId));
            }
            return new  BaseResponseModel((int)deduction, deduction.ToString());
        }
        #region Player
        ErrorCode EnrollmentMoneyPlayer(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return ErrorCode.UnknownError;

            var playerCheckTransaction = _casinoBalanceManager.CheckTransaction(request.TransactionId);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
                return playerCheckTransaction;

            var increaseResult = _gameBalanceManager.IncreaseBalance(request.Amount, request.TransactionId);
            return increaseResult;
        }
        ErrorCode DeductionMoneyPlayer(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return ErrorCode.UnknownError;

            var decreaseResult = _gameBalanceManager.DecreaseBalance(request.Amount, request.TransactionId);
            return decreaseResult;
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
        ErrorCode EnrollmentMoneyCasino(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return ErrorCode.UnknownError;

            var checkTransaction = _gameBalanceManager.CheckTransaction(request.TransactionId);
            if (!checkTransaction.Equals(ErrorCode.Success))
                return checkTransaction;

            var increaseResult = _casinoBalanceManager.IncreaseBalance(request.Amount, request.TransactionId);
            return increaseResult;
        }
        ErrorCode DeductionMoneyCasino(TransferMoneyRequestModel request)
        {
            if (string.IsNullOrEmpty(request.TransactionId) || request.Amount <= 0)
                return ErrorCode.UnknownError;

            var decreaseResult = _casinoBalanceManager.DecreaseBalance(request.Amount, request.TransactionId);
            return decreaseResult;
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
