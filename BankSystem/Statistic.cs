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

        public static string countUsers { get; set; }
      
            
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
