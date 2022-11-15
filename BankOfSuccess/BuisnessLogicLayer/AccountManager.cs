using BankOfSuccess.EntityLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankOfSuccess.BuisnessLogicLayer
{/// <summary>
/// Class that manages the activities of  an account
/// </summary>

    //delegate void DTransfer(Transfer transfer);

    // Control class-recieve the data from the user and validate the rules and apply the same
    public class AccountManager
    {
        //delegate
        //DTransfer dTransfer = new DTransfer(TranferLog.AddToLog);

        //Method for opening account
        public bool OpenAccount(Account account, string accType)
        {
            //Declaration..
            bool isAccountOpened = false;

            //Check wether account is saving or not(Dynamic Polymorphism)
            IAccountImpl accountImpl = AccountImplFactory.Create(accType);
            isAccountOpened = accountImpl.Open(account);

            //Return status
            return isAccountOpened;
        }

        //Close account which are Opened
        public bool CloseAccount(Account account)
        {
            bool isAccountClosed = false;
            try
            {
                //Check if account already Closed
                CheckIsAccountClosed(account);
                CheckIsAccountActive(account.IsActive);
                DiscloseAccount(account);

            }
            catch (AccountIsAlreadyClosedException ex)
            {
                throw ex;
            }
            catch (AccountIsNotActiveException ex)
            {
                throw ex;
            }
            isAccountClosed = true;
            return isAccountClosed;
        }

        //Method for withdraw money from account
        public double Withdraw(int pin, double withdrawAmt, Account account)
        {
            bool isWithdrawn = false;
            try
            {
                if (CheckIsAccountActive(account.IsActive))
                {
                    if (CheckIfPinIsValid(pin, account.PinNumber))
                    {
                        if (IsSufficientBalance(account.Balance, withdrawAmt))
                        {
                            isWithdrawn = WithdrawnFunds(account, withdrawAmt);
                            // Transaction transaction = new Transaction(account, null, withdrawAmt, TransferMode.none, DateTime.Now, "Withdraw");
                            //TransactionLog.AddToLog(transaction);

                        }
                    }
                }
            }
            catch (AccountIsNotActiveException ex)
            {
                throw ex;
            }
            catch (InvalidPinNumberException ex)
            {
                throw ex;
            }
            catch (InSufficientBalanceException ex)
            {
                throw ex;
            }

            return account.Balance;
        }

        //Method for deposit money in account
        public double Deposit(Account account, double depositAmt)
        {
            bool isDeposited = false;
            try
            {
                if (CheckIsAccountActive(account.IsActive))
                {
                    isDeposited = DepositFunds(account, depositAmt);
                    //Transaction transaction = new Transaction(null, account, depositAmt, TransferMode.none, DateTime.Now, "Deposit");
                    //TransactionLog.AddToLog(transaction);
                }
            }
            catch (AccountIsNotActiveException ex)
            {
                throw ex;
            }
            return account.Balance;
        }

        //Transfer money from one account to another
        public double Transfer(Transfer transfer)
        {
            bool isTransferred = false;

            try
            {
                if (CheckIsAccountActive(transfer.FromAccount.IsActive) && CheckIsAccountActive(transfer.ToAccount.IsActive))
                {
                    if (CheckIfPinIsValid(transfer.FromAccount.PinNumber, transfer.PinNumber))
                    {
                        if (IsSufficientBalance(transfer.FromAccount.Balance, transfer.Amount))
                        {
                            //get daily transfer limit
                            double dailyTransferLimit = TransferLimitManager.GetTransferLimit(transfer.FromAccount.Privilge);

                            CheckIfTransferLimitExceeded(transfer, dailyTransferLimit);

                            if (WithdrawnFunds(transfer.FromAccount, transfer.Amount))
                            {
                                if (DepositFunds(transfer.ToAccount, transfer.Amount))
                                {
                                    TranferLog.AddToLog(transfer);
                                    //dTransfer.Invoke(transfer);

                                    isTransferred = true;
                                    //Transaction transaction = new Transaction(transfer.FromAccount, transfer.ToAccount, transfer.Amount, transfer.Mode, DateTime.Now, "Transfer");
                                    //TransactionLog.AddToLog(transaction);
                                }
                            }
                        }
                    }
                }
            }
            catch (AccountIsNotActiveException ex)
            {
                throw ex;
            }
            catch (InvalidPinNumberException ex)
            {
                throw ex;
            }
            catch (InSufficientBalanceException ex)
            {
                throw ex;
            }
            catch (TransferLimitExceededException ex)
            {
                throw ex;
            }

            return transfer.FromAccount.Balance;
        }

        //you need to create one more class to store each incominng requests for the debit card

        //Method for applying debit card
        public bool ApplyDebitCard(Account accObj)
        {
            bool orderPlaced = false;
            try
            {
                if (CheckIsAccountActive(accObj.IsActive))
                {

                    if (IfAccountHasAlreadyCard(accObj))
                    {
                        throw new AccountHasAlreadyDebitCard("Debit card has been already issued for this account .");
                    }

                    else if (CheckIfAppliedForCard(accObj))
                    {
                        throw new DebitCardHasBeenAlreadyRequestedException("Debit card has been already requested for this account and status is " + accObj.DebitCardStatus.Status);
                    }
                    else
                    {
                        //accObj.DebitCardRequest = new DebitCardRequest("requested");
                        DebitCardStatus cardReq = new DebitCardStatus("requested");
                        accObj.DebitCardStatus = cardReq;
                        //DebitCardRequestedRepository.AddToRepository(cardReq);
                        accObj.DebitCard = new DebitCard();
                        //DebitCardRepository.AddToRepository(accObj.DebitCard);
                    }
                }

            }
            catch (Exception ex) { throw; }
            orderPlaced = true;
            return orderPlaced;
        }

        //Method for change pin of debit card
        public bool SetPin(Account accObj, int pin, int newPin, int confirmPin)
        {

            if (CheckStatusOfDebitCardReq(accObj))
            {
                if (CheckIfCardIsNotExpired(accObj))
                {
                    if (CheckIfPinIsValid(pin, accObj.DebitCard.Pin))
                    {
                        if (ChangePin(accObj, newPin, confirmPin))
                        {
                            if (accObj.DebitCardStatus.Status.Equals("requested"))
                            {
                                accObj.DebitCardStatus.Status = "completed";
                                accObj.DebitCard.ActivationDate = DateTime.Now;
                                accObj.DebitCard.IsCardActive = true;
                            }
                        }
                    }
                }
            }
            return true;
        }

        //Method for Cancel service of debit card
        public bool CancelDebitCard(Account accObj, int pin)
        {
            bool orderCancel = false;
            try
            {
                if (CheckIsAccountActive(accObj.IsActive))
                {
                    if (CheckIfPinIsValid(pin, accObj.PinNumber))
                    {
                        if (CheckIfAppliedForCard(accObj))
                        {
                            accObj.DebitCardStatus.Status = "cancel";
                            //throw new DebitCardHasBeenAlreadyRequestedException("Debit card has been already requested for this account and status is " + accObj.DebitCardRequest.Status);
                        }
                        else if (!IfAccountHasAlreadyCard(accObj))
                        {
                            throw new AccountHasAlreadyDebitCard("Debit card has not issued for this account .");
                        }
                        else
                        {
                            //accObj.DebitCardRequest = new DebitCardRequest("requested");

                            accObj.DebitCardStatus.Status = "cancel";
                            accObj.DebitCard.IsCardActive = false;

                        }
                    }
                }

            }
            catch (Exception ex) { throw; }
            orderCancel = true;
            return orderCancel;
        }

        //Helping methods to build unit testable code-------------------------------------------------------------
        private bool IfAccountHasAlreadyCard(Account account)
        {
            bool temp = false;
            if (account.DebitCard == null)
            {
                return temp;
            }
            else if (account.DebitCard.IsCardActive)
            {
                temp = true;
            }
            return temp;
        }

        private bool CheckIfAppliedForCard(Account acc)
        {
            if (acc.DebitCardStatus == null || acc.DebitCardStatus.Status.Equals("expired"))
                return false;
            return true;
        }
   
        private bool CheckStatusOfDebitCardReq(Account accObj)
        {
            if (accObj.DebitCardStatus == null)
            {
                throw new AccountHadNoDebitCards("Your Account has no debit card service. Apply for debit card.");
            }
            return true;
        }

        private bool ChangePin(Account accObj, int newPin, int confirmPin)
        {
            if (newPin == confirmPin)
                accObj.DebitCard.Pin = newPin;
            else
                throw new BothPinNotSameException("Both pin should be same. You entered wrong pin.");
            return true;
        }

        private bool CheckIfCardIsNotExpired(Account accObj)
        {
            if (accObj.DebitCard != null && accObj.DebitCard.ExpiryDate < DateTime.Now)
            {
                accObj.DebitCard.IsCardActive = false;
                accObj.DebitCardStatus.Status = "expired";
                throw new DebitCardExpiredException("Your Debit card is expired.");
            }
            return true;
        }
       



        private bool CheckIsAccountClosed(Account account)
        {
            if (!account.IsActive)
                throw new AccountIsAlreadyClosedException("This account is already closed.");
            return true;
        }
        private bool DiscloseAccount(Account account)
        {
            account.IsActive = false;
            account.ClosedDate = DateTime.Now;
            return true;
        }

        private bool CheckIsAccountActive(bool isActive)
        {
            if (!isActive)
                throw new AccountIsNotActiveException("Account is InActive.");
            return true;
        }
        private bool CheckIfPinIsValid(int enteredPinNumber, int accountPinNumber)
        {
            if (enteredPinNumber != accountPinNumber)
                throw new InvalidPinNumberException("You entered wrong pin number.");
            return true;
        }
        private bool IsSufficientBalance(double balance, double withdrawAmt)
        {
            if (balance < withdrawAmt)
                throw new InSufficientBalanceException("Insufficient  balance in account.");
            return true;
        }

        private bool WithdrawnFunds(Account account, double withdrawAmt)
        {
            account.Balance -= withdrawAmt;
            return true;
        }
        private bool DepositFunds(Account account, double depositAmt)
        {
            account.Balance += depositAmt;
            return true;
        }

        private bool CheckIfTransferLimitExceeded(Transfer transfer, double dailyTransferLimit)
        {
            //get transfers done by FromAccount for a day 
            List<Transfer> transfers = TranferLog.GetTransfersFromLog(transfer.FromAccount);
            double amountTransferred = 0.00;
            foreach (Transfer t in transfers)
            {
                amountTransferred += transfer.Amount;
            }
            //check if transfer done for the day already exceeded the limit. 
            if (amountTransferred + transfer.Amount >= dailyTransferLimit)
            {
                throw new TransferLimitExceededException("Daily Transfer Limit Exceeded");
            }
            return true;
        }
        
        public void GetTransactionDetail(Transaction transaction)
        {
            if (transaction.AccountNo == transaction.FromAccount.AccountNumber)
            {

            }
        }
    }
}
