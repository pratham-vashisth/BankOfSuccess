using BankOfSuccess.BuisnessLogicLayer;
using BankOfSuccess.EntityLayer;

namespace BankOfSuccessUnitTesting
{
    [TestClass]
    public class AccountManagerUnitTesting
    {
        [TestMethod]
        public void OpenAccount_WithPositiveInput_ShouldActivateAcc()
        {
            //DebitCard card = new DebitCard();
            //DebitCardStatus cardS= new DebitCardStatus();
            AccountManager manager = new AccountManager();
            Account acc = new Account { Name="Name",PinNumber=1111,Balance=100.0,
                Privilge=Privilge.GOLD,IsActive=false,ActivatedDate=DateTime.Now,
                DebitCard=null,DebitCardStatus=null};
            bool accOpen = manager.OpenAccount(acc, "Savings");
            Assert.AreEqual(true, acc.IsActive);
        }

        //[TestMethod]
        //[ExpectedException(typeof(AccountIsAlreadyClosedException))]

    }
}