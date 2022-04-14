using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class BankAccount
    {
        private Logger log = new Logger();
        public DateTime Birthday;
        public DateTime DayOfCreation;
        public string UserID;
        public string FIO;
        public Card[] Cards;
        public string Login { get; set; }
        public string Password { get; set; }
        //cards


        private BankAccount()
        {
            transfer[0] = 1;//UAH to UAH
            transfer[1] = Statistic.USDtransferUAH;//USD to UAH
            transfer[2] = Statistic.EURtransferUAH;//EUR to UAH
        }
        //for registration
        public BankAccount(string surname, string name, string patronymicon, DateTime birthday, string login, string password) : this()
        {
            Login = login;
            Password = password;


            Statistic.addUser();
            UserID = Statistic.countUsers;

            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = DateTime.Now;


            //.txt это кстати топовая база данных 2022 
            File.AppendAllText("Accounts.txt", String.Format("{0} {1} \n{2} \n{3} {4} {5}\n", login, password, UserID, FIO, birthday.ToShortDateString(), DayOfCreation.ToShortDateString()));
            log.printInLog($"Банковский счёт {login} успешно записан в базу данных", "INFO");

        }


        //for verify
        public BankAccount(string surname, string name, string patronymicon, DateTime birthday, DateTime dayOfCreation, string login, string password, string userID) : this()
        {
            Login = login;
            Password = password;
            UserID = userID;

            Birthday = birthday;
            FIO = String.Format("{0} {1} {2}", surname, name, patronymicon);
            DayOfCreation = dayOfCreation;
            Cards = verifyCard();
        }

        //USD,UAH,EUR
        public void createCard()
        {
            Console.Clear();
            ushort type;
            do
            {
                Console.WriteLine("(1 - UAH, 2 - UAH, 3 - EUR)");
                Console.Write("Введите валюту карточки которую хотите создать: ");
                type = ushort.Parse(Console.ReadLine());

                if (type < 1 || type > 3)
                {
                    log.printInLog($"{Login} выбрал недоступную валюту", "WARN");
                }
                {
                    log.printInLog($"{Login} выбрал валюту для своей новой карты", "INFO");
                }
            }
            while (type < 1 || type > 3);
            if (Cards == null)
            {
                Cards = new Card[1];
                Cards[0] = new Card(type);
                log.printInLog($"{Login} завел свою первую карту", "INFO");

                if (!Directory.Exists("Cards")) { Directory.CreateDirectory("Cards"); }
                log.printInLog($"Новая каррта {Login} добавлена в базу данных", "INFO");
                File.AppendAllText($"Cards/{Login}.txt", String.Format("{0} {1} {2} {3} {4} {5}\n", Cards[0].CVV, Cards[0].Money, type, Cards[0].Type, Cards[0].CardId, Cards[0].EndTime));

                addToSystem(Cards[0].CardId);
            }
            else
            {
                Card[] temp = new Card[Cards.Length + 1];
                for (int i = 0; i < Cards.Length; i++)
                {
                    temp[i] = Cards[i];
                }
                temp[Cards.Length] = new Card(type);

                Cards = temp;
                log.printInLog($"{Login} завел новую карту", "INFO");

                if (!Directory.Exists("Cards")) { Directory.CreateDirectory("Cards"); }
                File.AppendAllText($"Cards/{Login}.txt", String.Format("{0} {1} {2} {3} {4} {5} \n", temp[Cards.Length - 1].CVV, temp[Cards.Length - 1].Money, type, temp[Cards.Length - 1].Type, temp[Cards.Length - 1].CardId, temp[Cards.Length - 1].EndTime));
                log.printInLog($"Новая каррта {Login} добавлена в базу данных", "INFO");

                addToSystem(temp[Cards.Length - 1].CardId);
            }
        }

        private void addToSystem(string cardId)
        {
            //название начинается с восклицательного знака чтобы легко найти файл
            File.AppendAllText("Cards/!CardLogs.txt", String.Format("{0} {1}\n", Login, cardId));

            string text = File.ReadAllText("Cards/!CardLogs.txt");
            string[] mas = text.Split("\n");
            Array.Sort(mas);

            File.WriteAllText("Cards/!CardLogs.txt", "");

            for (int i = 0; i < mas.Length; i++)
            {
                File.AppendAllText("Cards/!CardLogs.txt", String.Format("{0}\n", mas[i]));
            }

        }

        private double[] transfer = new double[3];
        public void getAllMoney()
        {
            Console.Clear();
            log.printInLog($"{Login} запросил общий счёт", "INFO");
            double sum = 0;


            for (int i = 0; i < Cards.Length; i++)
            {
                sum += Cards[i].Money * transfer[Cards[i].MoneyType - 1];
            }
            Console.WriteLine($"Общее количество денег (в грн): {sum}");
        }

        public void showCards()
        {
            Console.Clear();
            log.printInLog($"{Login} запросил информацию о картах", "INFO");
            for (int i = 0; i < Cards.Length; i++)
            {
                Console.WriteLine(String.Format("{0} - {1} \n{2} {3} \n{}\n", i, Cards[i].Type, Cards[i].CardId, Cards[i].Money, Cards[i].MoneyType));
            }
        }
        public void payMoneyTo()
        {
            Console.Clear();
            if (Cards.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("У вас еще нет карточек");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            log.printInLog($"{Login} запросил перевод на другую карту", "INFO");
            string text = File.ReadAllText("Cards/!CardLogs.txt");
            string[] mas = text.Split("\n");


            showCards();
            Console.Write("Введите карту с которой собираетесь списать деньги: ");
            ushort yourCard = ushort.Parse(Console.ReadLine());
            log.printInLog($"{Login} выбрал карту для перевода", "INFO");


            double yourMoney;
            do
            {
                Console.Write("Введите количество денег которые собираетесь передать: ");
                yourMoney = double.Parse(Console.ReadLine());
                if (yourMoney > Cards[yourCard].Money)
                {
                    log.printInLog($"{Login} выбрал сумму превышающую имеющуюся", "INFO");
                }
            } while (yourMoney > Cards[yourCard].Money);
            log.printInLog($"{Login} выбрал сумму для перевода", "INFO");

            double convertMoney = yourMoney * transfer[Cards[yourCard].MoneyType - 1];
            log.printInLog($"{Login} деньги для перевода конвертированы в гривны", "INFO");

            string cardId;
            bool isOk = false;
            //Алгоритм ЛУНА
            do
            {
                Console.WriteLine("Напишите карточку на которую хотите перевести деньги: ");
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
                    log.printInLog($"{Login} указал некорекктную карту {cardId}", "INFO");
                }

            } while (!isOk);
            log.printInLog($"{Login} указал корекктную карту {cardId}", "INFO");


            string[] temp;
            for (int i = 0; i < mas.Length; i++)
            {
                temp = mas[i].Split(" ");
                if (temp[1] == cardId)
                    log.printInLog($"{Login} начал перевод на карту {cardId}", "INFO");
                {
                    string userInfo = File.ReadAllText($"{temp[0]}.txt");
                    string[] tmp1 = userInfo.Split("\n");
                    for (int j = 0; j < tmp1.Length; j++)
                    {
                        string[] tmp2 = tmp1[i].Split(" ");
                        if (tmp2[4] == cardId)
                        {
                            tmp2[1] += convertMoney / transfer[Convert.ToUInt16(tmp2[2])];
                            Cards[yourCard].Money -= yourMoney;

                            tmp1[i] = String.Join(" ", tmp2);
                        }
                    }
                    userInfo = String.Join("\n", tmp1);
                    File.WriteAllText($"{temp[0]}.txt", userInfo);
                }
            }


        }
        private Card[] verifyCard()
        {
            string text;
            string[] cards, temp;

            log.printInLog($"Верификация карточек {Login} начата", "INFO");
            if (!Directory.Exists("Cards")) { Directory.CreateDirectory("Cards"); }


            if (!File.Exists($"Cards/{Login}.txt"))
            {
                log.printInLog($"Верификация карточек {Login} закончена (отсутствие карточек)", "INFO");
                return null;
            }



            text = File.ReadAllText($"Cards/{Login}.txt");
            cards = text.Split("\n");
            Card[] verifiedCards = new Card[cards.Length - 1];

            for (int i = 0; i < cards.Length - 1; i++)
            {
                temp = cards[i].Split(" ");
                verifiedCards[i] = new Card(Convert.ToInt32(temp[0]), Convert.ToDouble(temp[1]), Convert.ToUInt16(temp[2]), temp[3], temp[4], Convert.ToDateTime(temp[5]));
            }
            log.printInLog($"Верификация карточек {Login} закончена, верифицировано {verifiedCards.Length} карточек)", "INFO");
            return verifiedCards;

            //  String.Format("{0} {1} {2} {3} {4} {5}\n", temp[Cards.Length].CVV, temp[Cards.Length].Money, type, temp[Cards.Length].Type, temp[Cards.Length].CardId, temp[Cards.Length].EndTime));
            // 
        }

    }
}
