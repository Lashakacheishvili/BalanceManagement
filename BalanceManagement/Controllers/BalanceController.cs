using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.ServiceImplementations;
using Service.ServiceInterfaces;
using ServiceModels;

namespace BalanceManagement.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BalanceController : BaseController
    {
        private readonly IBaseManagementService _gameBalanceManager;
        private readonly IBaseManagementService _casinoBalanceManager;
        public BalanceController()
        {
            _gameBalanceManager = new GameManagementService();
            _casinoBalanceManager = new CasinoManagementService();
        }
        [HttpGet("balance")]
        public decimal GetBalance()
        {
            return _casinoBalanceManager.GetBalance();
        }
        [HttpPost("withdrawal/{transactionid}/{amount}")]
        public BaseResponseModel TransferMoneyToPlayer(string transactionId, decimal amount)
        {
            var deduction = _casinoBalanceManager.DeductionMoney(new ServiceModels.TransferModel.TransferMoneyRequestModel { Amount = amount, TransactionId = transactionId });
            if (deduction.HttpStatusCode == 0)
            {
                var enrollment = _gameBalanceManager.EnrollmentMoney(new ServiceModels.TransferModel.TransferMoneyRequestModel { Amount = amount, TransactionId = transactionId });
                return enrollment.HttpStatusCode == 0 ? enrollment : _casinoBalanceManager.Rollback(new ServiceModels.TransferModel.TransferBaseRequestModel { TransactionId = transactionId });
            }
            return deduction;
        }
        [HttpPost("deposit/{transactionid}/{amount}")]
        public BaseResponseModel TransferMoneyToCasino(string transactionId, decimal amount)
        {
            var deduction = _gameBalanceManager.DeductionMoney(new ServiceModels.TransferModel.TransferMoneyRequestModel { Amount = amount, TransactionId = transactionId });
            if (deduction.HttpStatusCode == 0)
            {
                var enrollment = _casinoBalanceManager.EnrollmentMoney(new ServiceModels.TransferModel.TransferMoneyRequestModel { Amount = amount, TransactionId = transactionId });
                return enrollment.HttpStatusCode == 0 ? enrollment : _gameBalanceManager.Rollback(new ServiceModels.TransferModel.TransferBaseRequestModel { TransactionId = transactionId });
            }
            return deduction;
        }
    }
}
