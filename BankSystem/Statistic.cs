using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public static class Statistic
    {
        private static long right = 0;
        private static long left = 0;


        ///////////////  Unique Id  /////////////////
        public static string countUsers { get; private set; }
        public static long countCards { get; private set; }

        ////////////////////////////////////////////
      

        ///////////  Money transfering  /////////////
        public static double USDtransferUAH { get; private set; } = 29.41;
        public static double EURtransferUAH { get; private set; } = 31.98;
        ////////////////////////////////////////////

        public static void addCard()
        {
            try
            {
                if (countCards == 999999999)
                {
                    throw (new Exception("unique id for card ended"));
                }
                else
                {
                    countCards++;
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void addUser()
        {
            //1234567890 1234567890
            if (right != 9999999999)
            {
                right++;
            }
            else
            {
                right = 0;
                left++;
            }
            countUsers += left.ToString().PadLeft(10);
            countUsers += right.ToString().PadLeft(10);
        }

    }
}
