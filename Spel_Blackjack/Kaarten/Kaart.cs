using System;
using System.Collections.Generic;
using System.Text;

namespace Spel_Blackjack.Kaarten
{
    public class Kaart
    {
        public string TypeKaart;
        public int Waarde;
        public string Kleur;
        public string Vormpje;
        public string NaamKaart;

        public Kaart(int waarde, string kleur, string vormpje, string typekaart = null)
        {
            Waarde = waarde;
            Kleur = kleur;
            Vormpje = vormpje;
            TypeKaart = typekaart;
            NaamKaart = genNaam();
        }

        private string genNaam()
        {
            string emptyName = "";
            char letterVormpje = Vormpje[0];
            char letterKleur = Kleur[0];
            if (TypeKaart!=null)
            {
                char letterKaart = TypeKaart[0];
                emptyName = $"{letterVormpje}{letterKaart}-{letterKleur}";
            }
            else
            {
                string letterKaart = Waarde.ToString();
                emptyName = $"{letterVormpje}{letterKaart}-{letterKleur}";
            }
            return emptyName;
        }
    }
}
