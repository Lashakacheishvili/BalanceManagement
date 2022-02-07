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
        private readonly IGameManagementService _baseManagementService;
        public BalanceController(IGameManagementService baseManagementService)
        {
            _baseManagementService = baseManagementService;
        }
        [HttpGet("balance")]
        public decimal GetBalance() => _baseManagementService.GetBalanceCasino();
        [HttpPost("withdrawal/{transactionId}/{amount}")]
        public BaseResponseModel TransferMoneyToPlayer(string transactionId, decimal amount) => _baseManagementService.TransferMoneyToPlayer(new ServiceModels.TransferModel.TransferMoneyRequestModel(transactionId, amount));
        [HttpPost("deposit/{transactionId}/{amount}")]
        public BaseResponseModel TransferMoneyToCasino(string transactionId, decimal amount) => _baseManagementService.TransferMoneyToCasino(new ServiceModels.TransferModel.TransferMoneyRequestModel(transactionId, amount));
    }
}
