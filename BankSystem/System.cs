using BankSystem.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class System
    {
        private Card[] allCards;
        private Logger log = new Logger();

        public void registerInSystem()
        {
            log.printInLog("Неизвестный пользователь начал регистрацию", "INFO");
            string login, password;

            Console.Write("Введите логин: ");
            login = Console.ReadLine();
            log.printInLog("Неизвестный пользователь ввел логин в поле регистрации", "INFO");
            log.printInLog($"Неизвестный пользователь переименован в {login}", "INFO");

            bool haveDigital;
            bool haveUppercase;
            bool haveSymvol;
            do
            {
                haveDigital = false;
                haveUppercase = false;
                haveSymvol = false;
                try
                {
                    Console.Write("Введите пароль: ");
                    password = Console.ReadLine();
                    log.printInLog($"{login} ввёл пароль в поле регистрации", "INFO");

                    if (password == "Qwerty12345") { throw new Exception("Пожалуйста используйте СИЛЬНЫЙ пароль"); }
                    if (password.Length < 8 || password.Length > 16) { throw new Exception("Пароль должен быть размером 8-16 символов"); }
                    for (int i = 0; i < password.Length; i++)
                    {
                        if (Char.IsDigit(password[i]))
                        {
                            haveDigital = true;
                        }
                        if (!Char.IsLower(password[i]))
                        {
                            haveUppercase = true;
                        }
                        if (!Char.IsLetterOrDigit(password[i]))
                        {
                            haveUppercase = true;
                        }
                    }
                    if (haveDigital || haveUppercase || haveSymvol)
                    {
                        break;
                    }
                    else
                    {
                        throw new NotEnoughStrengthException(haveDigital, haveUppercase, haveSymvol);
                    }
                }
                catch (NotEnoughStrengthException e)
                {
                    log.printInLog($"{login} ввёл некорректный пароль: Символ, цифра, заглавная буква", "WARN");
                    Console.Clear();
                    Console.Write($"Введите логин: {login}");
                    e.getAdvMessege();

                }
                catch (Exception e)
                {
                    log.printInLog($"{login} ввёл некорректный пароль: {e.Message}", "WARN");
                    Console.Clear();
                    Console.Write($"Введите логин: {login}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (true);


            //Взял код с интернета :<
            byte[] data = Encoding.Default.GetBytes(password);
            var result = new SHA256Managed().ComputeHash(data);
            password = (BitConverter.ToString(result).Replace("-", "").ToLower());
            log.printInLog($"Пароль {login} успешно зашифрован", "INFO");


            Console.Clear();
            //имя фамилия и отчество раздельно специально чтобы не было потом фамилии "Вася"
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите ваше отчество: ");
            string patronymicon = Console.ReadLine();
            log.printInLog($"Пароль {login} успешно указал ФИО", "INFO");

            Console.Clear();
            Console.Write("Введите день месяц и год вашего рождения в формате dd:mm:yyyy : ");
            string strBirthday = Console.ReadLine();
            string[] temp = strBirthday.Split(":");

            ///////////////////////////////////////////////////
            //Не забудь сделать проверку на правильную дату!!//
            ///////////////////////////////////////////////////
            DateTime birthday = new DateTime(Convert.ToInt16(temp[0]), Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2]));
            log.printInLog($"Дата рождения {login} успешно указана", "INFO");


            BankAccount newUser = new BankAccount(surname, name, patronymicon, birthday, login, password);
            log.printInLog($"Банковский счёт {login} успешно зарегестрирован", "INFO");

        }
        public void loginInSystem()
        {

        }
        public void verifyCards()
        {

        }



    }
}
