using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankOfSuccess.EntityLayer;

namespace BankOfSuccess.BuisnessLogicLayer
{
    //Creating Interface which has bool type method
    internal interface IAccountImpl
    {
        bool Open(Account account);
    }
}
