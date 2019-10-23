using System;
using System.Collections.Generic;
using System.Text;
using Spel_Blackjack.Kaarten;
using Spel_Blackjack.Spelers;
using System.Linq;

namespace Spel_Blackjack
{
    class Spel
    {
        public event UpdateMessageDelegate updateMessage;
        public event ReadMessageDelegate readMessage;
        public event WaitForKeyMessageDelegate waitForKey;


        Speler dealer = new Dealer("dealer");
        public List<Speler> SpelerLijst = new List<Speler>();
        public List<Speler> VerwijderLijst = new List<Speler>();

        public List<Kaart> Kaarten;
        private List<String> TemplateKaarten = new List<String>() {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Boer", "Vrouw", "Heer", "Aas"};
        private List<String> TemplateKleuren = new List<String>() { "Rood", "Zwart" };
        private List<String> TemplateVormen = new List<String>() { "Harten", "Ruiten", "Schoppen", "Klaver"};

        private static Random rng = new Random();


        public Spel()
        {
            genKaarten();
            schudKaarten();
        }



        public void Starten()
        {
            StatusTonen(dealer, false);
            foreach (Speler speler in SpelerLijst)
            {
                if (Convert.ToString(speler.Naam).Equals("dealer"))
                {
                    Console.WriteLine("Dealer is aan de beurt.");
                    dealerBeurt();
                }
                else
                {
                    StatusTonen(speler, false);
                    int spelerHand = speler.BerekenHand();

                    bool nogKeerVragen = true;
                    while (nogKeerVragen)
                    {
                        updateMessage("(" + speler.Naam + ") Druk 'k' om een kaart te pakken, 'p' om te passen of 'q' om het spel te verlaten.");
                        ConsoleKey gedrukteKey = waitForKey();
                        if (gedrukteKey == ConsoleKey.K)
                        {
                            updateMessage(speler.Naam + " krijg een nieuwe kaart!");
                            geefKaart(speler);
                            spelerHand = speler.BerekenHand();
                            StatusTonen(speler, false);
                            if (spelerHand < 21)
                            {
                                nogKeerVragen = true;
                            }
                            else
                            {
                                nogKeerVragen = false;
                            }

                        }
                        else if (gedrukteKey == ConsoleKey.Q)
                        {
                            updateMessage(speler.Naam + " heeft het spel verlaten.");
                            VerwijderLijst.Add(speler);
                            nogKeerVragen = false;
                        }

                        else if (gedrukteKey == ConsoleKey.P)
                        {
                            updateMessage(speler.Naam + " passed.");
                            nogKeerVragen = false;
                        }
                        else
                        {
                            nogKeerVragen = true;
                        }
                    }
                        
                }
            }
            spelerVerwijderen();
            checkWins();
            // CHECK OF SPELER MEER DAN 0 PUNTEN HEEFT ANDERS UIT LIJST VERWIJDEREN.
            
        }


        private void spelerVerwijderen()
        {
            foreach(Speler speler in VerwijderLijst)
            {
                SpelerLijst.Remove(speler);
            }
        }

        private void dealerBeurt()
        {
            StatusTonen(dealer, true);

            if (dealer.BerekenHand() > 18)
            {
                updateMessage("Dealer is gereed met totale kaarten waarde: " + dealer.BerekenHand());
            }
            else
            {
                while(dealer.BerekenHand() < 17)
                {
                    updateMessage("Dealer moet nog een kaart pakken, tot dat hij 17 of hoger heeft.");
                    geefKaart(dealer);
                    StatusTonen(dealer, true);
                }
            }
        }




        private void checkWins()
        {
            foreach(Speler speler in SpelerLijst)
            {
                
                if (speler.Naam == "dealer")
                {
                    continue;
                }
                else
                {
                    int totaalPunten_speler = speler.BerekenHand();
                    int totaalPunten_dealer = dealer.BerekenHand();
                    long uitbetaling = speler.Inzet * 2;
                    if (totaalPunten_speler > 21)
                    {
                        updateMessage(speler.Naam + " heeft meer dan 21 (" + totaalPunten_speler + "), en verliest " + speler.Inzet + " punten.");
                        speler.AantalPunten -= speler.Inzet;
                    }
                    else if (totaalPunten_speler == 21)
                    {
                        if (totaalPunten_dealer == 21)
                        {
                            updateMessage(speler.Naam + " heeft totale waarde kaarten: " + totaalPunten_speler + ", wat hetzelfde is als de dealer. (PUSH) Geen uitbetaling voor zowel speler als dealer.");
                        }
                        else
                        {
                            updateMessage(speler.Naam + " heeft BLACKJACK en wint " + uitbetaling + " punten");
                            speler.AantalPunten += uitbetaling;
                        }

                    }
                    else if (totaalPunten_dealer == 21)
                    {
                        updateMessage("De dealer heeft blackjack en wint van speler " + speler.Naam + "!");
                    }

                    else if (totaalPunten_dealer > 21 && totaalPunten_speler < 21)
                    {
                        updateMessage("De dealer ging over de 21. Hierdoor wint speler " + speler.Naam + " " + uitbetaling + " punten.");
                        speler.AantalPunten += uitbetaling;
                    }
                    else if (totaalPunten_speler > totaalPunten_dealer)
                    {
                        updateMessage(speler.Naam + " heeft een betere hand dan de dealer en wint " + uitbetaling);
                        speler.AantalPunten += uitbetaling;
                    }
                    else if (totaalPunten_dealer > totaalPunten_speler)
                    {
                        updateMessage("De dealer had een betere hand dan " + speler.Naam + ". Deze speler verliest zijn inzet (" + speler.Inzet + ")");
                        speler.AantalPunten -= speler.Inzet;
                    }
                }
                speler.ResetHand();
         
            }
            dealer.ResetHand();
        }



        private void geefKaart(Speler speler)
        {
            Kaart gepakteKaart = Kaarten[0];
            speler.KaartGeven(gepakteKaart);
            Kaarten.RemoveAt(0);
        }



        public void InzetInnen()
        {
            foreach (Speler speler in SpelerLijst)
            {
                if (Convert.ToString(speler.Naam).Equals("dealer"))
                {
                    continue;
                }
                else
                {
                    bool showMessage = true;
                    while (showMessage){
                        updateMessage(Convert.ToString(speler.Naam) + " u heeft momenteel " + Convert.ToString(speler.AantalPunten) + " punten. Hoeveel wilt u inzetten?");
                        string inzet = readMessage();
                        long inzetWaarde = Convert.ToInt64(inzet);

                        if ((speler.AantalPunten - inzetWaarde) >= 0 ){
                            speler.Inzet = inzetWaarde;
                            showMessage = false;
                        }
                        else{
                            updateMessage("Warning: De inzet waarde kan niet meer zijn dan uw totaal aantal punten.");
                       
                        }
                    }

                }

            }
        }




        public void StartKaartenGeven()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (Speler speler in SpelerLijst)
                {
                    geefKaart(speler);
                }
            }
        }




        public void SpelersMaken(int aantalSpelers)
        {

            for (int x = 1; x <= aantalSpelers; x++)
            {
                updateMessage("Voer een Naam in: ");
                string naam = readMessage();
                Speler SpelerInstance = new Speler(naam);
                SpelerLijst.Add(SpelerInstance);
            }

            SpelerLijst.Add(dealer);
        }



        public void genKaarten()
        {
            Kaarten = new List<Kaart>();
            for (int i = 0; i < TemplateKleuren.Count; i++)
            {
                string currKleur = TemplateKleuren[i];

                for (int x = 0; x < TemplateVormen.Count; x++)
                {
                    string currVorm = TemplateVormen[x];

                    for (int y = 0; y < TemplateKaarten.Count; y++)
                    {
                        string currKaart = TemplateKaarten[y];

                        if (currKaart.Equals("Boer") || currKaart.Equals("Vrouw") || currKaart.Equals("Heer"))
                        {
                            Kaart kaart = new Kaart(10, currKleur, currVorm, currKaart);
                            Kaarten.Add(kaart);
                        } 
                        else if (currKaart.Equals("Aas"))
                        {
                            Kaart kaart = new Kaart(11, currKleur, currVorm, currKaart);
                            Kaarten.Add(kaart);
                        }
                        else
                        {
                            Kaart kaart = new Kaart(Convert.ToInt32(currKaart), currKleur, currVorm);
                            Kaarten.Add(kaart);
                        }

                    }
                }
            }

        }



        public void schudKaarten()
        {
            Kaarten = Kaarten.OrderBy(a => Guid.NewGuid()).ToList();
        }



        private void StatusTonen(Speler speler, bool meerKaarten)
        {
            updateMessage("==========================================================================================");
            if (Convert.ToString(speler.Naam).Equals("dealer"))
            {
                if (meerKaarten)
                {
                    updateMessage(Convert.ToString(speler.Naam) + " heeft de volgende kaarten: ");
                    foreach (Kaart k in dealer.Hand)
                    {

                        updateMessage(k.NaamKaart);

                    }
                    updateMessage("De totale waarden van de kaarten: " + dealer.BerekenHand());
                }
                else
                {
                    updateMessage(Convert.ToString(speler.Naam) + " heeft de volgende kaart: ");
                    updateMessage(speler.Hand[0].NaamKaart); // EERSTE KEER 1 KAART ANDERE KEER ALLE KAARTEN.
                    updateMessage("De totale waarden van de kaart: " + speler.Hand[0].Waarde);
                }

            }
            else
            {
                updateMessage(Convert.ToString(speler.Naam) + " heeft " + Convert.ToString(speler.Inzet) + " ingezet.");
                updateMessage("En heeft de volgende kaarten:");
                foreach (Kaart k in speler.Hand)
                {

                    updateMessage(k.NaamKaart);

                }
                updateMessage("De totale waarden van de kaarten: " + Convert.ToString(speler.BerekenHand()));
            }
            updateMessage("==========================================================================================");

        }












        //check printer
        public void ToonKaarten()
        {
            foreach (Kaart kaart in Kaarten)
            {
                Console.WriteLine(kaart.NaamKaart);
            }
        }

    }
}
