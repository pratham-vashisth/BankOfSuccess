using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOfSuccess.EntityLayer
{
    //Entity class for defining structure of Transfer
    public class Transfer
    {
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
        public double Amount { get; set; }
        public int PinNumber { get; set; }
        public TransferMode Mode { get; set; }
    }

    //Enum class for storing constants
    public enum TransferMode
    {
        none = 0,
        IMPS = 1,
        NEFT = 2,
        RTGS = 3
    }
}
