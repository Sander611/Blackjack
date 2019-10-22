using System;
using System.Collections.Generic;
using System.Text;

namespace Spel_Blackjack.Spelers
{
    public class Dealer : Speler
    {
        public Dealer(string naam) : base(naam) //base extends constructor
        {
            Naam = naam;
            AantalPunten = 9999999999999;
            Inzet = 0;
        }
    }
}
