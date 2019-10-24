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

            int aantalAas = 0;
            foreach(Kaart kaart in Hand){
                if(kaart.Waarde == 11){
                    aantalAas += 1;
                }
            }
            while (totaal > 21 && aantalAas > 0){
                totaal-=10;
                aantalAas-=1;
            }


            return totaal;
        }

        public void ResetHand()
        {
            Hand = new List<Kaart>();
        }
    }
}
