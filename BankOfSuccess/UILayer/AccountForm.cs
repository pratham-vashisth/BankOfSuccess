using BankOfSuccess.BuisnessLogicLayer;
using BankOfSuccess.EntityLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BankOfSuccess.UILayer
{/// <summary>
/// Boundary class - used to represent and manage the interactions with the user
/// </summary>
///
    class AccountForm
    {
        //creating object of AccountManager class
        AccountManager manager = new AccountManager();
        //Taking data from user to open account
        public void OpenAccount()
        {
            bool checkAccountOpened = false;
            
            Console.WriteLine("Account Opening");
            Console.WriteLine("Do you want to open Savings or Current:");
            string accountType = Console.ReadLine();

            if (accountType.Equals("Savings"))
            {
                Savings savings = new Savings();

                Console.Write("Enter Your Name:");
                savings.Name = Console.ReadLine();

                Console.Write("Enter Your Pin Number:");
                savings.PinNumber = int.Parse(Console.ReadLine());

                Console.Write("Enter Your Date of Birth:");
                savings.DateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Your Gender(F/M/T):");
                savings.Gender = char.Parse(Console.ReadLine());

                Console.Write("Enter Your Phone Number:");
                savings.PhoneNo = Console.ReadLine();

                Console.WriteLine("Select the Privilge: 1.PRIMIUM \t 2.GOLD \t 3.SILVER");
                string privilge = Convert.ToString(Console.ReadLine());
                if (privilge.Equals("1"))
                    savings.Privilge = Privilge.PREMIUM;
                else if (privilge.Equals("2"))
                    savings.Privilge = Privilge.GOLD;
                else if (privilge.Equals("3"))
                    savings.Privilge = Privilge.SILVER;

                try
                {
                    //Calling account manager by passing savings object
                    checkAccountOpened = manager.OpenAccount(savings, accountType);
                    if (checkAccountOpened)
                    {
                        Console.WriteLine("Account Opened Successfully.");
                        Console.WriteLine($"Account type:{accountType}");
                        Console.WriteLine($"Account number: {savings.AccountNumber}");
                        Console.WriteLine($"Account holder name: {savings.Name}");
                        Console.WriteLine($"Balance: {savings.Balance}");
                        Console.WriteLine($"Date of birth: {savings.DateOfBirth}");
                        Console.WriteLine($"Gender: {savings.Gender}\n");
                        ApplyDebitCard();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            else if (accountType.Equals("Current"))
            {
                Current current = new Current();
                Console.Write("Name:");
                current.Name = Console.ReadLine();

                Console.Write("Pin number:");
                current.PinNumber = int.Parse(Console.ReadLine());

                Console.Write("Company Name:");
                current.CompanyName = Console.ReadLine();

                Console.Write("Website:");
                current.Website = Console.ReadLine();

                Console.Write("Registration Number:");
                current.RegistrationNo = Console.ReadLine();

                Console.WriteLine("Select the Privilge: 1.PRIMIUM /t 2.GOLD /t 3.SILVER");
                string privilge = Convert.ToString(Console.ReadLine());
                if (privilge.Equals("1"))
                    current.Privilge = Privilge.PREMIUM;
                else if (privilge.Equals("2"))
                    current.Privilge = Privilge.GOLD;
                else if (privilge.Equals("3"))
                    current.Privilge = Privilge.SILVER;

                try
                {
                    //Calling account manager by passing Current object
                    checkAccountOpened = manager.OpenAccount(current, accountType);
                    if (checkAccountOpened)
                    {
                        Console.WriteLine("Account Opened Successfully.");
                        Console.WriteLine($"Account type:{accountType}");
                        Console.WriteLine($"Account number: {current.AccountNumber}");
                        Console.WriteLine($"Account holder name: {current.Name}");
                        Console.WriteLine($"Registration number: {current.RegistrationNo}");
                        Console.WriteLine($"Company name: {current.CompanyName}");
                        Console.WriteLine($"Website: {current.Website}");
                        Console.WriteLine($"Balance: {current.Balance}\n");
                        ApplyDebitCard();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        //Method for taking inputs from user to close opened Account
        public void CloseAccount()
        {
            bool isClosed = false;
            Console.Write("Do you want to close your account: (Y/N)");
            char choice = char.Parse(Console.ReadLine());

            try
            {
                if (choice == 'Y')
                {
                    Console.Write("Account number:");
                    int accNo = int.Parse(Console.ReadLine());

                    Console.Write("Pin number:");
                    int pinNo = int.Parse(Console.ReadLine());

                    //Get account from accountRepository by passing account number as parameter
                    Account accountTobeClosed = AccountRepository.GetAccount(accNo);

                    //Calling account manager by passing account object to validate details inserted by user
                    isClosed = manager.CloseAccount(accountTobeClosed);
                    if (isClosed)
                        Console.WriteLine($"Account closed successfully.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }

        }
        //Method for taking input form user to withdraw money
        public void Withdraw()
        {
            double accountBalance = 0;

            Console.Write("Account Number: ");
            int accNo = int.Parse(Console.ReadLine());

            Console.Write("Account Pin:");
            int pin = int.Parse(Console.ReadLine());

            Console.Write("Amount which you want to withdraw:");
            double withdrawAmt = double.Parse(Console.ReadLine());

            try
            {
                //Get account from accountRepository by passing account number as parameter
                Account accountWithdrawn = AccountRepository.GetAccount(accNo);

                //Calling account manager by passing account object,pin, withdraw amount
                accountBalance = manager.Withdraw(pin, withdrawAmt, accountWithdrawn);

                Console.WriteLine($"{withdrawAmt} is debited from account:{accNo}.Total balance is {accountBalance}.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        //Method for taking inputs from user to deposit money account
        public void Deposit()
        {
            double accountBalance = 0;

            Console.Write("Account Number: ");
            int accNo = int.Parse(Console.ReadLine());
            Console.Write("Amount which you want to deposit:");
            double depositAmt = double.Parse(Console.ReadLine());

            try
            {
                //Get account from accountRepository by passing account number as parameter
                Account accountDeposit = AccountRepository.GetAccount(accNo);

                //Calling account manager by passing account object,amount
                accountBalance = manager.Deposit(accountDeposit, depositAmt);
                Console.WriteLine($"{depositAmt} is deposited in account:{accNo}.Total balance:{accountBalance}.\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        //Method for taking inputs from user to transfer money from one account to another
        public void Transfer()
        {
            double accountBalance = 0;
            Transfer transfer = new Transfer();

            Console.Write("Enter Account Number from which you Want to transfer:");
            int fromAccNo = int.Parse(Console.ReadLine());

            Console.Write("Enter pin:");
            transfer.PinNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Account Number in which you Want to transfer:");
            int toAccNo = int.Parse(Console.ReadLine());

            //Asking for transfer mode to user
            Console.WriteLine("Transfer Mode: 1.IMPS\t       2.NEFT\t        3.RTGS");
            int mode = int.Parse(Console.ReadLine());
            if (mode.Equals("1"))
                transfer.Mode = TransferMode.IMPS;
            else if (mode.Equals("2"))
                transfer.Mode = TransferMode.NEFT;
            else if (mode.Equals("3"))
                transfer.Mode = TransferMode.RTGS;

            Console.WriteLine("Enter amount:");
            transfer.Amount = Convert.ToDouble(Console.ReadLine());
            //transfer.PinNumber= pin;
            try
            {
                //Get account from accountRepository by passing account number as parameter
                transfer.FromAccount = AccountRepository.GetAccount(fromAccNo);
                transfer.ToAccount = AccountRepository.GetAccount(toAccNo);
                //Calling account manager by passing transfer object
                accountBalance = manager.Transfer(transfer);
                Console.WriteLine($"{transfer.Amount} is debited from account:{transfer.FromAccount.AccountNumber}.Total balance is {accountBalance}.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
       
        //Method for taking inputs from user to apply debit card service
        public void ApplyDebitCard()
        {
            try
            {

                Console.WriteLine("Do you want debit card?(Y/N):");
                char choice = char.Parse(Console.ReadLine());
                if (choice == 'Y')
                {
                    bool isApplied = false;
                    Console.WriteLine("Account number:");
                    int accNo = int.Parse(Console.ReadLine());

                
                    //Get account from accountRepository by passing account number as parameter
                    Account accObj = AccountRepository.GetAccount(accNo);

                    //Calling account manager by passing account object
                    isApplied = manager.ApplyDebitCard(accObj);
                    if (isApplied)
                    {
                        Console.WriteLine("Applied for card Successfully.\n");
                        Console.WriteLine($"pin:{accObj.DebitCard.Pin}");
                    }
                }
            }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
        }
        //Method for Taking input from user to change debit card pin
        public void SetPin()
        {
            Console.WriteLine("Do you want to change pin of debit card?(Y/N):");
            char choice = char.Parse(Console.ReadLine());
            if (choice == 'Y')
            {
                bool pinChanged = false;
                Console.WriteLine("Account number:");
                int accNo = int.Parse(Console.ReadLine());
                try
                {
                    //Get account from accountRepository by passing account number as parameter
                    Account accObj = AccountRepository.GetAccount(accNo);
                    Console.WriteLine("Enter debit card pin:");
                    int pin = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter new pin:");
                    int newPin = int.Parse(Console.ReadLine());

                    Console.WriteLine("Confirm pin:");
                    int confirmPin = int.Parse(Console.ReadLine());


                    //Calling account manager by passing account object,Debitcard
                    pinChanged = manager.SetPin(accObj, pin, newPin, confirmPin);
                    if (pinChanged)
                    {
                        Console.WriteLine("Pin changed successfully!\n");
                        Console.WriteLine($"Debit card number:{accObj.DebitCard.CardNumber}");
                        Console.WriteLine($"Debit card Activated date:{accObj.DebitCard.ActivationDate}");
                        Console.WriteLine($"Debit card expiry date:{accObj.DebitCard.ExpiryDate}");
                        Console.WriteLine($"pin:{accObj.DebitCard.Pin}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        //Method for taking inputs from user to cancel debit card service
        public void CancelDebitCard()
        {

            Console.WriteLine("Do you want cancel debit card service?(Y/N):");
            char choice = char.Parse(Console.ReadLine());
            if (choice == 'Y')
            {
                bool isCanceled = false;
                Console.WriteLine("Account number:");
                int accNo = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter account Pin:");
                int pin = int.Parse(Console.ReadLine());

                try
                {
                    //Get account from accountRepository by passing account number as parameter
                    Account accObj = AccountRepository.GetAccount(accNo);

                    //Calling manager to verify details given by user
                    isCanceled = manager.CancelDebitCard(accObj, pin);
                    if (isCanceled)
                    {
                        Console.WriteLine("Debit card canceled Successfully.\n");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void TransactionDetail()
        {
            Transaction transaction = new Transaction();
            Console.Write("Enter Account Number:");
            transaction.AccountNo = int.Parse(Console.ReadLine());

            manager.GetTransactionDetail(transaction);


        }
    }

}
