using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Exceptions
{
    class NotEnoughStrengthException : Exception
    {
        bool HaveDigital;
        bool HaveUppercase;
        bool HaveSymvol;
        public NotEnoughStrengthException(bool digits, bool upper, bool symvol) : base()
        {
            HaveDigital = digits;
            HaveUppercase = upper;
            HaveSymvol = symvol;
        }

        public void getAdvMessege()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("В пароле должны быть:");

            if (HaveUppercase) { Console.ForegroundColor = ConsoleColor.Green; }
            Console.WriteLine(" - Буква верхнего регистра");


            if (HaveUppercase) { Console.ForegroundColor = ConsoleColor.Green; }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(" - Цифра");


            if (HaveUppercase) { Console.ForegroundColor = ConsoleColor.Green; }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" - Символ");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
