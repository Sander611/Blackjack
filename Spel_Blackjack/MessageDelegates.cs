using System;
using System.Collections.Generic;
using System.Text;

namespace Spel_Blackjack
{
    public delegate void UpdateMessageDelegate(string message);
    public delegate string ReadMessageDelegate();
    public delegate ConsoleKey WaitForKeyMessageDelegate();
}
