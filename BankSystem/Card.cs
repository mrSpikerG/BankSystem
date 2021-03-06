using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Card
    {
        //cart type
        public string Type { get; private set; }

        //Verification info
        public string CardId { get; private set; }
        public DateTime EndTime { get; private set; }
        public int CVV { get; private set; }

        //Money
        public ushort MoneyType { get; private set; }
        public double Money { get; set; }


        private Random rand = new Random();
        
        
        //1 - UAH
        //2 - USD
        //3 - EUR

        //for creation
        public Card(ushort type)
        {
            MoneyType = type;
            //card id genetation
            #region
            CardId = "516874" +Statistic.countCards.ToString().PadLeft(9,'0');
            Statistic.addCard();
            //Luna algorithm
            int sum = 0;
            for (int i = 0; i < CardId.Length; i++)
            {
                if (i + 1 % 2 != 0)
                {
                    sum += Convert.ToInt32(CardId[i]) * 2;
                }
                else
                {
                    sum += Convert.ToInt32(CardId[i]);
                }
            }
            int lastNum = 10 - sum % 10;
            CardId += lastNum;
            #endregion
            CVV = rand.Next(1000);
            EndTime = DateTime.Now.AddMonths(50);
            Type = "Debit";
            Money = 0;
        }
        //for verify
        public Card(int _CVV,double money, ushort typeMoney,string typeCard, string cardId,DateTime endTime)
        {
            Type = typeCard;

            CardId = cardId;
            CVV = _CVV;
            EndTime = endTime;
            
            MoneyType = typeMoney;
            Money = money;
        }
    }
}
