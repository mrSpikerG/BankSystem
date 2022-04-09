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

        private DateTime Birthday;
        private DateTime DayOfCreation;
        private string UserID;
        private string FIO;
        private Card[] Cards;
        public string Login { get; set; }
        public string Password { get; set; }
        //cards


        //for registration
        public BankAccount(string surname,string name,string patronymicon,DateTime birthday,string login,string password)
        {
            Login = login;
            Password = password;


            UserID = Statistic.countUsers;
            Statistic.addUser();

            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = DateTime.Now;

            File.AppendAllText("Accounts.txt", String.Format("{0} {1} \n{2} \n{3} {4} {5}",login,password,UserID,FIO,birthday.ToShortDateString(),DayOfCreation.ToShortDateString()));
        }

        //for verify
        public BankAccount(string surname, string name, string patronymicon, DateTime birthday,DateTime dayOfCreation, string login, string password,string userID)
        {
            Login = login;
            Password = password;
            UserID = userID;
            
            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = dayOfCreation;
        }

        //USD,UAH,EUR
        public void createCard(string type)
        {
            if (Cards == null)
            {
                Cards = new Card[1];
                Cards[0] = new Card(type);
            }
            else
            {
                Card[] temp = new Card[Cards.Length + 1];
                for(int i = 0; i < Cards.Length; i++)
                {
                    temp[i] = Cards[i];
                }
                Cards = temp;
            }
        }
        public double getAllMoney()
        {
            double sum = 0;

            for(int i = 0; i < Cards.Length; i++)
            {
                //Я понимаю что свитч - один из 8 смертных грехов, но лишний раз парится из-за этого не хочется :<
                switch (Cards[i].Type)
                {
                    case "USD":
                        sum+=Cards[i].Money * Statistic.USDtransferUAH;
                        break;
                    case "EUR":
                        sum+=Cards[i].Money * Statistic.EURtransferUAH;
                        break;
                    case "UAH":
                        sum += Cards[i].Money;
                        break;
                }
            }
            return sum;
        }
    }
}
