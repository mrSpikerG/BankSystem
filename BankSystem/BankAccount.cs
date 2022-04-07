using System;
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
        private int Money;
        private Card[] Cards;
        public string login { get; set; }
        //cards

        public BankAccount(string surname,string name,string patronymicon,DateTime birthday)
        {
            UserID = Statistic.countUsers;
            Statistic.addUser();

            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = DateTime.Now;
            Money = 0;
        }

        public void createCard()
        {
            if (Cards == null)
            {
                Cards = new Card[1];
                Cards[0] = new Card();
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

    }
}
