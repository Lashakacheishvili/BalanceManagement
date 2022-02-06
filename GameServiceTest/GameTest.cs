using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.ServiceImplementations;
using Service.ServiceInterfaces;
using System;

namespace GameServiceTest
{
    [TestClass]
    public class GameTest
    {
        private readonly IBaseManagementService _managementService;
        public GameTest()
        {   
            _managementService = new GameManagementService();
        }
        [TestMethod]
        public void PlayerWin()
        {
            var transferMoneyToPlayer = _managementService.TransferMoneyToPlayer(new ServiceModels.TransferModel.TransferMoneyRequestModel("test",100));
            Console.WriteLine(transferMoneyToPlayer.Message);
            Assert.AreEqual(transferMoneyToPlayer.HttpStatusCode, 10);
        }
        [TestMethod]
        public void CasinoWin()
        {
            var transferMoneyToCasino = _managementService.TransferMoneyToCasino(new ServiceModels.TransferModel.TransferMoneyRequestModel("test1", 200));
            Console.WriteLine(transferMoneyToCasino.Message);
            Assert.AreEqual(transferMoneyToCasino.HttpStatusCode, 10);
        }
        [TestMethod]
        public void GetCasinoBalance()
        {
            var casinoBalance = _managementService.GetBalanceCasino();
            Console.WriteLine(casinoBalance.ToString());
            Assert.AreNotEqual(casinoBalance, 0);
        }
    }
}
