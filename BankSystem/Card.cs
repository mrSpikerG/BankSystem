using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Card
    {
        public string CardId { get; set; }
        public string Type { get; set; }

        public long Money { get; private set; }

        private DateTime EndTime { get; set; }
        private int CVV { get; set; }


        Random rand = new Random();
        public Card()
        {
            //card id genetation
            #region
            CardId = "516874" + rand.Next(1000000000).ToString().PadLeft(9);
            //Luna algorithm
            int sum = 0;
            for (int i = 0; i < CardId.Length; i++)
            {
                if (i + 1 % 2 != 0)
                {
                    sum+=Convert.ToInt32(CardId[i])*2;
                }
                else
                {
                    sum += Convert.ToInt32(CardId[i]);
                }
            }
            int lastNum = 10 - sum % 10;
            CardId +=lastNum;
            #endregion
            CVV = rand.Next(1000);
            EndTime = DateTime.Now.AddMonths(50);
            Type = "Debit";
            Money = 0;
        }
    }
}
