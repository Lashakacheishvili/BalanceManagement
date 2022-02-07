using ServiceModels;
using ServiceModels.TransferModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.ServiceInterfaces
{
    public interface IGameManagementService
    {
        BaseResponseModel TransferMoneyToPlayer(TransferMoneyRequestModel request);
        BaseResponseModel TransferMoneyToCasino(TransferMoneyRequestModel request);
        decimal GetBalancePlayer();
        decimal GetBalanceCasino();
    }
}
