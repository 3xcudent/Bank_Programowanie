using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Programowanie.Models
{
    class Accounts
    {
        public long AccountNumber;
        public int UserID;
        public string Currency;
        public int Balance;
        public bool Blockade;

        public static Accounts FromCSV(string csvLine)
        {
            string[] values = csvLine.Split(';');
            Accounts account = new Accounts();
            account.AccountNumber = Int64.Parse(values[0]);
            account.UserID = Int32.Parse(values[1]);
            account.Currency = values[2];
            account.Balance = Int32.Parse(values[3]);
            if (values[4] == "TAK") {
                account.Blockade = true; 
            } else {
                account.Blockade = false; 
            }
            return account;
        }
    }
}
