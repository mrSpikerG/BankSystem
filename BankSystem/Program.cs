using System;
using System.Security.Cryptography;
using System.Text;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            System sys = new System();
            Action[] act = new Action[3];

            act[0] = sys.registerInSystem;
            act[1] = sys.loginInSystem;

      
            int check;
            do
            {
                Console.WriteLine("\tBankomat");
                Console.WriteLine("1 - Зарегестрировать банковский счет");
                Console.WriteLine("2 - Зайти в банковский счет");
                Console.WriteLine("3 - Пополнить банковский счет");
                Console.WriteLine("0 - Закончить работу");

                Console.Write("Ваш выбор: ");
                check = int.Parse(Console.ReadLine());
                Console.Clear();

                if (check != 0)
                {
                    act[check - 1]?.Invoke();
                }
            } while (check != 0);
        }
    }
}
