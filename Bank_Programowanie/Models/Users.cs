using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Programowanie.Models
{
    class Users
    {
        public int ID;
        public string Name;
        public string Surname;
        public long Pesel;
        public string Address;

        public static Users FromCSV(string csvLine)
        {
            string[] values = csvLine.Split(';');
            Users user = new Users();
            user.ID = Int32.Parse(values[0]);
            user.Name = values[1];
            user.Surname = values[2];
            user.Pesel = Int64.Parse(values[3]);
            user.Address = values[4];
            return user;
        }
    }
}
