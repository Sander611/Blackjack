using Spel_Blackjack.Kaarten;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spel_Blackjack.Spelers
{ 

    public class Speler
    {
        public string Naam;
        public long AantalPunten;
        public List<Kaart> Hand = new List<Kaart>();
        public long Inzet { get; set; }

        public Speler(string naam)
        {
            Naam = naam;
            AantalPunten = 100;
            Inzet = 0;
        }

        public void KaartGeven(Kaart gegevenKaart)
        {
            Hand.Add(gegevenKaart);
        }

        public int BerekenHand()
        {
            int totaal = 0;
            foreach(Kaart kaart in Hand)
            {
                totaal += kaart.Waarde;
            }

            if (totaal > 21){
                
                foreach(Kaart kaart in Hand){
                    if (kaart.Waarde == 11){
                        totaal-=10;
                    }
                }   
            }


            return totaal;
        }

        public void ResetHand()
        {
            Hand = new List<Kaart>();
        }
    }
}
