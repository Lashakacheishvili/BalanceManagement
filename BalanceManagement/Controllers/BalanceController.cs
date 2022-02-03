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
        public BalanceController()
        {
            _gameBalanceManager = new GameManagementService();
        }
        [HttpGet("balance")]
        public decimal GetBalance()=> _gameBalanceManager.GetBalanceCasino();
        [HttpPost("withdrawal/{transactionId}/{amount}")]
        public BaseResponseModel TransferMoneyToPlayer(string transactionId, decimal amount) => _gameBalanceManager.TransferMoneyToPlayer(new ServiceModels.TransferModel.TransferMoneyRequestModel(transactionId, amount));
        [HttpPost("deposit/{transactionId}/{amount}")]
        public BaseResponseModel TransferMoneyToCasino(string transactionId, decimal amount)=> _gameBalanceManager.TransferMoneyToCasino(new ServiceModels.TransferModel.TransferMoneyRequestModel(transactionId, amount));
    }
}
