using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class BankAccount
    {
        Logger log = new Logger();
        private DateTime Birthday;
        private DateTime DayOfCreation;
        private string UserID;
        private string FIO;
        private Card[] Cards;
        public string Login { get; set; }
        public string Password { get; set; }
        //cards


        //for registration
        public BankAccount(string surname, string name, string patronymicon, DateTime birthday, string login, string password)
        {
            Login = login;
            Password = password;


            Statistic.addUser();
            UserID = Statistic.countUsers;

            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = DateTime.Now;


            //.txt это кстати топовая база данных 2022 
            File.AppendAllText("Accounts.txt", String.Format("{0} {1} \n{2} \n{3} {4} {5}\n", login, password, UserID, FIO, birthday.ToShortDateString(), DayOfCreation.ToShortDateString()));
            log.printInLog($"Банковский счёт {login} успешно записан в базу данных", "INFO");
        }
        

        //for verify
        public BankAccount(string surname, string name, string patronymicon, DateTime birthday, DateTime dayOfCreation, string login, string password, string userID)
        {
            Login = login;
            Password = password;
            UserID = userID;

            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = dayOfCreation;
        }

        //USD,UAH,EUR
        public void createCard(ushort type)
        {
            if (Cards == null)
            {
                Cards = new Card[1];
                Cards[0] = new Card(type);
                log.printInLog($"{Login} завел свою первую карту", "INFO");
            }
            else
            {
                Card[] temp = new Card[Cards.Length + 1];
                for (int i = 0; i < Cards.Length; i++)
                {
                    temp[i] = Cards[i];
                }
                Cards = temp;
                log.printInLog($"{Login} завел новую карту", "INFO");
            }
        }


        private double[] transfer = new double[3];
        public double getAllMoney()
        {
            double sum = 0;
            transfer[0] = 1;//UAH to UAH
            transfer[1] = Statistic.USDtransferUAH;//USD to UAH
            transfer[2] = Statistic.EURtransferUAH;//EUR to UAH

            for (int i = 0; i < Cards.Length; i++)
            {
                sum += Cards[i].Money * transfer[Cards[i].MoneyType - 1]; 
            }
            return sum;
        }
    }
}
