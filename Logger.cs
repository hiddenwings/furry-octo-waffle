using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatBot_WPF_
{
    //Логгер (Пытался сделать статик, но я тупой, сделал как смог)
    public class Logger
    {
        Stack<string> log = new Stack<string>();

        public void Set(string str)
        {
            log.Push(str);
        }

        public string Get()
        {
            if (log.Count != 0)
            {
                var txtlog = log.Pop();
                return txtlog;
            }
            else
                return null;
        }
    }
}
