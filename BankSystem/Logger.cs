using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class Logger
    {
        public delegate void MyDelegate(string text,string type);

        private MyDelegate[] delegmas = new MyDelegate[3];
        public MyDelegate printInLog; 

        public Logger()
        {
            delegmas[0] = null;
            delegmas[1] = (string text,string type) => Console.WriteLine(String.Format("[{0}] {1}: {2}\n", DateTime.Now.ToShortTimeString(), type, text));
            delegmas[2] = (string text,string type) => File.AppendAllText($"logs/{DateTime.Now.ToShortDateString()}.txt", String.Format("[{0}] {1}: {2}\n",DateTime.Now.ToShortTimeString(),type,text));
            setType(2);
        }
        public void setType(int type)
        {
            printInLog = delegmas[type];
        }
    }
}
