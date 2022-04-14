using System;
using System.IO;
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
        private static Logger log = new Logger();
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
                string text;
                string[] stats;
                //Синхронизация данных
                if (!File.Exists("Statistic.txt"))
                {
                    File.WriteAllText("Statistic.txt", $"{countCards}\n{left.ToString().PadLeft(10, '0')} {right.ToString().PadLeft(10, '0')}");
                }
                else
                {
                    text = File.ReadAllText("Statistic.txt");
                    stats = text.Split("\n");
                    countCards = Convert.ToInt64(stats[0]);

                    stats = stats[1].Split(" ");
                    left = Convert.ToInt64(stats[0]);
                    right = Convert.ToInt64(stats[1]);
                }

                if (countCards == 999999999)
                {
                    throw (new Exception("unique id for card ended"));
                    //К тому времени как закончаться айдишки скорее всего меня не будет в компании которая закажет банкомат
                    //Поэтому оставлю эту проблему на будущее поколение :)
                }
                else
                {
                    countCards++;
                }

                File.WriteAllText("Statistic.txt", $"{countCards}\n{left.ToString().PadLeft(10, '0')} {right.ToString().PadLeft(10, '0')}");
            }
            catch (Exception e)
            {
                log.printInLog(e.Message, "ERROR");
            }
        }
        public static void addUser()
        {
            try
            {
                string text;
                string[] stats;
                //Синхронизация данных
                if (!File.Exists("Statistic.txt"))
                {
                    File.WriteAllText("Statistic.txt", $"{countCards}\n{left.ToString().PadLeft(10, '0')} {right.ToString().PadLeft(10, '0')}");
                }
                else
                {
                    text = File.ReadAllText("Statistic.txt");
                    stats = text.Split("\n");
                    countCards = Convert.ToInt64(stats[0]);

                    stats = stats[1].Split(" ");
                    left = Convert.ToInt64(stats[0]);
                    right = Convert.ToInt64(stats[1]);
                }

                //1234567890 1234567890
                if (right == 9999999999)
                {
                    right = 0;

                    if (left == 9999999999)
                    {
                        throw (new Exception("unique id for accounts ended"));
                        //К тому времени как закончаться айдишки скорее всего меня не будет в компании которая закажет банкомат
                        //Поэтому оставлю эту проблему на будущее поколение :)
                    }
                    else
                    {
                        left++;
                    }
                }
                else
                {
                    right++;
                }
                countUsers += left.ToString().PadLeft(10, '0');
                countUsers += right.ToString().PadLeft(10, '0');

                File.WriteAllText("Statistic.txt", $"{countCards}\n{left.ToString().PadLeft(10, '0')} {right.ToString().PadLeft(10, '0')}");
            }
            catch(Exception e)
            {
                log.printInLog(e.Message, "ERROR");
            }
        }

    }
}
