using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Bank_Programowanie.Models;

namespace Bank_Programowanie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Users> users = ImportUsers(@"Assets\..\..\..\..\Data\osoby.csv");
            List<Accounts> accounts = ImportAccounts(@"Assets\..\..\..\..\Data\konta.csv");
            Users CurrentUser = LoginMenu(users);
            while (true) {
                Console.Clear();
                PrintMainMenu(CurrentUser);
                TakeInputMenu(Console.ReadLine(),CurrentUser, accounts, users);
            }
        }
        static List<Users> ImportUsers(string sciezka) 
        {
            List<Users> values = File.ReadAllLines(sciezka).Skip(1).Select(v => Users.FromCSV(v)).ToList();
            return values;
        }
        static List<Accounts> ImportAccounts(string sciezka)
        {
            List<Accounts> values = File.ReadAllLines(sciezka).Skip(1).Select(v => Accounts.FromCSV(v)).ToList();
            return values;
        }
        static Users LoginMenu(List<Users> users)
        {

            Console.WriteLine("Podaj ID Logowania");
            Console.Write("ID: ");
            Users verified = CheckID(Console.ReadLine(),users);
            return verified;
        }
        static void PrintMainMenu(Users currentUser)
        {
            Console.WriteLine($"Witaj {currentUser.Name} {currentUser.Surname}");
            Console.WriteLine($"Aktualny adres to {currentUser.Address}");
            Console.WriteLine("1.Wypisz liste Kont");
            Console.WriteLine("2.Wplata lub wyplata");
            Console.WriteLine("3.Konta Zablokowane");
            Console.WriteLine("4.Generuj Raport");
            Console.WriteLine("5.Wyjdz");
            Console.Write("Wybor: ");
        }
        static void PrintTransferMenu()
        {
            Console.Clear();
            Console.WriteLine("1.Wplata");
            Console.WriteLine("2.Wyplata");
            Console.Write("Wybor: ");
        }
        static void TakeInputMenu(string input,Users currentUser, List<Accounts> accounts, List<Users> allUsers)
        {
            List<Accounts> userAccounts = new List<Accounts>();
            userAccounts.AddRange(accounts.Where(i => i.UserID == currentUser.ID));
            Console.Clear();
            switch (Int32.Parse(input))
            {
                case 1:
                    ListAccounts(userAccounts,2);
                    break;
                case 2:
                    long currentAccount = SelectAccount(userAccounts);
                    PrintTransferMenu();
                    TakeInputTransfer(currentAccount, accounts);
                    break;
                case 3:
                    ListBlockedAccounts(userAccounts);
                    break;
                case 4:
                    GenerateInfo(allUsers,accounts);
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        static void GenerateInfo(List<Users> allUsers, List<Accounts> allAccounts)
        {
            foreach (Users user in allUsers)
            {
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine($"{user.ID} {user.Name} {user.Surname} {user.Pesel} {user.Address}");
                Console.WriteLine("----------------------------------------------------");
                ListAccounts(allAccounts.Where(nr => nr.UserID == user.ID).ToList(), 3);
            }
        }
        static void ListAccounts(List<Accounts> userAccounts,int toNumber)
        {
            int i = 1;
            if (toNumber == 1)
            {
                Console.WriteLine("L.P. | Numer Konta | Waluta | Saldo");
                foreach (Accounts userAccount in userAccounts)
                {
                    Console.WriteLine($"  {i} | {userAccount.AccountNumber} |  {userAccount.Currency}  | {userAccount.Balance}");
                    i++;
                }
            }
            else if (toNumber == 2)
            {
                Console.WriteLine("Numer Konta | Waluta | Saldo");
                foreach (Accounts userAccount in userAccounts)
                {

                    Console.WriteLine($"{userAccount.AccountNumber} |  {userAccount.Currency}  | {userAccount.Balance}");
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Numer Konta | Waluta | Saldo | Blokada");
                foreach (Accounts userAccount in userAccounts)
                {

                    Console.WriteLine($"{userAccount.AccountNumber} |  {userAccount.Currency}  | {userAccount.Balance} | {userAccount.Blockade}");
                }
                Console.ReadKey();
            }
        }
        static long SelectAccount(List<Accounts> userAccounts)
        {
            Console.WriteLine("Wybierz Konto do wplaty lub wyplaty");
            ListAccounts(userAccounts, 1);
            Console.Write("Numer: ");
            string input = Console.ReadLine();
            int accountNr = Int32.Parse(input)-1;
            return userAccounts[accountNr].AccountNumber;
        }
        static void TakeInputTransfer(long accountNumber, List<Accounts> Accounts)
        {
            switch (Int32.Parse(Console.ReadLine()))
            {
                case 1:
                    Deposit(Amount(),accountNumber,Accounts);
                    break;
                case 2:
                    Withdraw(Amount(), accountNumber, Accounts);
                    break;
                default:
                    break;
            }
        }
        static void Deposit(int Amount,long accountNumber, List<Accounts> Accounts)
        {
            foreach (Accounts account in Accounts.Where(accountsNumber => accountsNumber.AccountNumber == accountNumber))
            {
                account.Balance += Amount;
            }
        }
        static void Withdraw(int Amount, long accountNumber, List<Accounts> Accounts)
        {
            foreach (Accounts account in Accounts.Where(accountsNumber => accountsNumber.AccountNumber == accountNumber))
            {
                account.Balance -= Amount;
            }
        }
        static int Amount()
        {
            Console.Clear();
            Console.WriteLine("Prosze podac kwote: ");
            return Int32.Parse(Console.ReadLine());
        }
        static void ListBlockedAccounts(List<Accounts> userAccounts)
        {
            Console.WriteLine("Konta Zablokowane");
            Console.WriteLine("Numer Konta | Waluta | Saldo");
            foreach (Accounts userAccount in userAccounts.Where(UA => UA.Blockade == true))
            {
                Console.WriteLine($"{userAccount.AccountNumber} |  {userAccount.Currency}  | {userAccount.Balance}");
            }
            Console.ReadKey();
        }
        static Users CheckID(string input,List<Users> users)
        {
            if(int.TryParse(input, out int id))
            {
                return users.Single(x => x.ID == id);
            }
            else
            {
                throw new Exception("Brak ID");
            }
        }
    }
}