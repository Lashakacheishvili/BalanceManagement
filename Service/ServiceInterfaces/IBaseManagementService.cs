using ServiceModels;
using ServiceModels.TransferModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.ServiceInterfaces
{
    public interface IBaseManagementService
    {
        BaseResponseModel EnrollmentMoney(TransferMoneyRequestModel request);
        BaseResponseModel DeductionMoney(TransferMoneyRequestModel request);
        BaseResponseModel Rollback(TransferBaseRequestModel request);
        decimal GetBalance();
    }
}
