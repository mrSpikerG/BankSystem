using BankSystem.Exceptions;
using System;
using System.IO;
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
        private BankAccount account;





        private string SHA256(string pass)
        {
            //Взял код с интернета :<
            byte[] data = Encoding.Default.GetBytes(pass);
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        public void registerInSystem()
        {
            log.printInLog("Неизвестный пользователь начал регистрацию", "INFO");
            string login, password;


            bool uniq = false;
            do
            {

                Console.Write("Введите логин: ");
                login = Console.ReadLine();
                if (File.Exists("Accounts.txt"))
                {
                    try
                    {
                        uniq = true;
                        string database = File.ReadAllText("Accounts.txt");
                        string[] databaseInfo = database.Split("\n");
                        for (int i = 0; i < databaseInfo.Length; i += 3)
                        {
                            string[] infoLine = databaseInfo[i].Split(" ");
                            if (infoLine[0] == login)
                            {
                                uniq = false;
                                Console.Clear();
                                throw new Exception("Данный логин уже существует");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Тут нет смысла от логов но если кто-то захочет можно оставить
                        //log.printInLog(e.Message, "WARN");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                }
                else
                {
                    uniq = true;
                }
            }
            while (!uniq);
            log.printInLog("Неизвестный пользователь ввел логин в поле регистрации", "INFO");
            log.printInLog($"Неизвестный пользователь переименован в {login}", "INFO");

            bool haveDigital;
            bool haveUppercase;
            bool haveSymvol;

            //Пароль
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
                            haveSymvol = true;
                        }
                    }
                    if (haveDigital && haveUppercase && haveSymvol)
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
                    Console.WriteLine($"Введите логин: {login}");
                    e.getAdvMessege();

                }
                catch (Exception e)
                {
                    log.printInLog($"{login} ввёл некорректный пароль: {e.Message}", "WARN");
                    Console.Clear();
                    Console.WriteLine($"Введите логин: {login}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (true);


            password = SHA256(password);
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
            DateTime birthday = new DateTime(Convert.ToInt16(temp[2]), Convert.ToInt16(temp[1]), Convert.ToInt16(temp[1]));
            log.printInLog($"Дата рождения {login} успешно указана", "INFO");


            BankAccount newUser = new BankAccount(surname, name, patronymicon, birthday, login, password);
            log.printInLog($"Банковский счёт {login} успешно зарегестрирован", "INFO");

            Console.WriteLine("Банковский счёт успешно зарегестрирован");

        }
        public void loginInSystem()
        {
            try
            {
                Console.Write("Введите логин: ");
                string login = Console.ReadLine();


                if (File.Exists("Accounts.txt"))
                {
                    throw new FileNotFoundException("У нашего банка еще нет пользователей", "Account.txt");
                }
                string DB = File.ReadAllText("Accounts.txt");


                string[] DBINFO = DB.Split("\n");
                for (int i = 0; i < DBINFO.Length; i += 3)
                {
                    string[] infoLine = DBINFO[i].Split(" ");
                    if (infoLine[0] == login)
                    {
                        Console.Write("Введите пароль: ");
                        string password = SHA256(Console.ReadLine());
                        if (infoLine[1] == password)
                        {
                            string[] AdditionalInfo = DBINFO[i + 2].Split(" ");
                            string[] birth = AdditionalInfo[3].Split(".");
                            string[] create = AdditionalInfo[4].Split(".");

                            DateTime birthday = new DateTime(Convert.ToInt16(birth[0]), Convert.ToInt16(birth[1]), Convert.ToInt16(birth[2]));
                            DateTime DayOfCreation = new DateTime(Convert.ToInt16(create[0]), Convert.ToInt16(create[1]), Convert.ToInt16(create[2]));


                            account = new BankAccount(AdditionalInfo[0], AdditionalInfo[1], AdditionalInfo[2], birthday, DayOfCreation, login, password, DBINFO[i + 1]);

                            Console.WriteLine("Ok");
                            // verifyCards()
                        }
                        else
                        {
                            throw new Exception("Некорректный пароль");
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                log.printInLog($"({e.FileName}) {e.Message}", "WARN");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                log.printInLog(e.Message, "WARN");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        public void logOut()
        {

        }
        public void verifyCards(BankAccount bk)
        {

        }



    }
}
