using Balances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BalanceManagement.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BalanceController : Controller
    {
        private readonly IBalanceManager _gameBalanceManager;
        private readonly IBalanceManager _casinoBalanceManager;
        public BalanceController()
        {
            _gameBalanceManager = new GameBalanceManager();
            _casinoBalanceManager = new CasinoBalanceManager();
        }
        [HttpGet("balance")]
        public decimal GetBalance()
        {
            return _casinoBalanceManager.GetBalance();
        }
        [HttpPost("withdraw/{transactionid}/{amount}")]
        public ErrorCode TransferMoneyToPlayer(string transactionid, decimal amount)
        {
            if (string.IsNullOrEmpty(transactionid) || amount <= 0)
                return ErrorCode.UnknownError;
            #region Casino
            var checkTransaction = _casinoBalanceManager.CheckTransaction(transactionid);
            if (!checkTransaction.Equals(ErrorCode.Success))
                return checkTransaction;
            var decreaseResult = _casinoBalanceManager.DecreaseBalance(amount, transactionid);
            if (!decreaseResult.Equals(ErrorCode.Success))
                return decreaseResult;
            #endregion
            #region Player
            var playerCheckTransaction = _gameBalanceManager.CheckTransaction(transactionid);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
            {
                _casinoBalanceManager.Rollback(transactionid);
                return ErrorCode.TransactionRejected;
            }
            var increaseResult = _gameBalanceManager.IncreaseBalance(amount, transactionid);
            if (!increaseResult.Equals(ErrorCode.Success))
            {
                _casinoBalanceManager.Rollback(transactionid);
                return ErrorCode.TransactionRejected;
            }
            #endregion
            return increaseResult;
        }
        [HttpPost("deposit/{transactionid}/{amount}")]
        public ErrorCode TransferMoneyToCasino(string transactionid, decimal amount)
        {
            if (string.IsNullOrEmpty(transactionid) || amount <= 0)
                return ErrorCode.UnknownError;
            #region Player
            var playerCheckTransaction = _gameBalanceManager.CheckTransaction(transactionid);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
                return playerCheckTransaction;
            var decreaseResult = _gameBalanceManager.DecreaseBalance(amount, transactionid);
            if (!decreaseResult.Equals(ErrorCode.Success))
                return decreaseResult;
            #endregion
            #region Casino
            var checkTransaction = _casinoBalanceManager.CheckTransaction(transactionid);
            if (!playerCheckTransaction.Equals(ErrorCode.Success))
            {
                _gameBalanceManager.Rollback(transactionid);
                return ErrorCode.TransactionRejected;
            }
            var increaseResult = _casinoBalanceManager.IncreaseBalance(amount, transactionid);
            if (!increaseResult.Equals(ErrorCode.Success))
            {
                _gameBalanceManager.Rollback(transactionid);
                return ErrorCode.TransactionRejected;
            }
            #endregion
            return increaseResult;
        }
    }
}
