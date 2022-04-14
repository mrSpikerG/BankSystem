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
        private BankAccount Account;
        private double[] transfer = new double[3];

        public System()
        {
            transfer[0] = 1;//UAH to UAH
            transfer[1] = Statistic.USDtransferUAH;//USD to UAH
            transfer[2] = Statistic.EURtransferUAH;//EUR to UAH
        }



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
                log.printInLog("Неизвестный пользователь ввел логин в поле регистрации", "INFO");
                log.printInLog($"Неизвестный пользователь переименован в {login}", "INFO");


                if (!File.Exists("Accounts.txt"))
                {
                    throw new FileNotFoundException("У нашего банка еще нет пользователей", "Account.txt");
                }

                string DB = File.ReadAllText("Accounts.txt");
                log.printInLog($"{login} запросил аккаунты для входа в систему", "INFO");


                string[] DBINFO = DB.Split("\n");
                for (int i = 0; i < DBINFO.Length; i += 3)
                {
                    string[] infoLine = DBINFO[i].Split(" ");
                    if (infoLine[0] == login)
                    {
                        log.printInLog($"Аккаунт {login} найден успешно", "INFO");

                        Console.Write("Введите пароль: ");
                        string password = SHA256(Console.ReadLine());
                        log.printInLog($"{login} Ввел пароль", "INFO");
                        log.printInLog($"Пароль {login} успешно зашифрован", "INFO");


                        if (infoLine[1] == password)
                        {
                            log.printInLog($"{login} успешно зашёл в систему", "INFO");

                            string[] AdditionalInfo = DBINFO[i + 2].Split(" ");
                            string[] birth = AdditionalInfo[3].Split(".");
                            string[] create = AdditionalInfo[4].Split(".");

                            DateTime birthday = new DateTime(Convert.ToInt16(birth[2]), Convert.ToInt16(birth[1]), Convert.ToInt16(birth[0]));
                            DateTime DayOfCreation = new DateTime(Convert.ToInt16(create[2]), Convert.ToInt16(create[1]), Convert.ToInt16(create[0]));
                            log.printInLog($"Информация о {login} успешно считана", "INFO");

                            Account = new BankAccount(AdditionalInfo[0], AdditionalInfo[1], AdditionalInfo[2], birthday, DayOfCreation, login, password, DBINFO[i + 1]);
                            log.printInLog($"Аккаунт {login} готов к использованию", "INFO");

                            Action[] act = new Action[4];
                            act[0] = Account.createCard;
                            act[1] = Account.showCards;
                            act[2] = Account.getAllMoney;
                            act[3] = Account.payMoneyTo;

                            ushort chose;
                            do
                            {
                                Console.WriteLine($"{Account.FIO}");
                                    
                                Console.WriteLine("\n\t Меню");
                                Console.WriteLine("1 - Создать новую карту");
                                Console.WriteLine("2 - Получить информкцию о картах");
                                Console.WriteLine("3 - Получить информкцию о всех деньгах");
                                Console.WriteLine("4 - Передать деньги на другую карту");
                                Console.WriteLine("0 - Выйти из аккаунта\n");

                                Console.Write("Ваш выбор: ");
                                chose = ushort.Parse(Console.ReadLine());

                                if (chose == 0)
                                {
                                    log.printInLog($"{login} вышел из системы", "INFO");
                                    return;
                                }
                                act[chose - 1]?.Invoke();

                            } while (true);


                        }
                        else
                        {
                            throw new Exception($"{login}: Некорректный пароль");
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

        public void moneyToCard()
        {
            log.printInLog("Неизвестный пользователь запросил пополнение счета", "INFO");
            string text = File.ReadAllText("Cards/!CardLogs.txt");
            string[] mas = text.Split("\n");

            Console.Write("Введите количество денег которые собираетесь положить: ");
            double convertMoney = double.Parse(Console.ReadLine());

            string cardId;
            bool isOk = false;
            //Алгоритм ЛУНА
            do
            {
                Console.Write("Напишите карточку на которую хотите положить деньги: ");
                cardId = Console.ReadLine();

                int sum = 0;
                for (int i = 0; i < 16; i++)
                {

                    if (i + 1 % 2 != 0)
                    {
                        sum += Convert.ToInt32(cardId[i]) * 2;
                    }
                    else
                    {
                        sum += Convert.ToInt32(cardId[i]);
                    }
                }
                isOk = sum % 10 == 0 ? true : false;
                if (!isOk)
                {
                    log.printInLog("неизвестный пользователь указал некорекктную карту {cardId}", "INFO");
                }

            } while (!isOk);
            log.printInLog("неизвестный пользователь указал корекктную карту {cardId}", "INFO");


            string[] temp;
            for (int i = 0; i < mas.Length; i++)
            {
                temp = mas[i].Split(" ");
                if (temp[1] == cardId)
                    log.printInLog($"неизвестный пользователь начал процесс пополнения денег на карте {cardId}", "INFO");
                {
                    string userInfo = File.ReadAllText($"Cards/{temp[0]}.txt");
                    string[] tmp1 = userInfo.Split("\n");
                    for (int j = 0; j < tmp1.Length; j++)
                    {
                        string[] tmp2 = tmp1[i].Split(" ");
                        if (tmp2[4] == cardId)
                        {
                            tmp2[1] = Math.Round(Convert.ToDouble(tmp2[1]) + convertMoney / transfer[Convert.ToUInt16(tmp2[2])-1],2).ToString();
                            tmp1[i] = String.Join(" ", tmp2);
                            log.printInLog($"неизвестный пользователь пополнил счет на карте {cardId}", "INFO");
                        }
                    }
                    userInfo = String.Join("\n", tmp1);
                    File.WriteAllText($"Cards/{temp[0]}.txt", userInfo);
                }
            }
        }
    }
}
