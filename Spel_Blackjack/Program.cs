using System;

namespace Spel_Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Spel nieuwspel = new Spel();
            nieuwspel.updateMessage += update_Message;
            nieuwspel.waitForKey += wait_For_Key_Message;
            nieuwspel.readMessage += read_Message;

            //nieuwspel.ToonKaarten(); //om alle kaarten gesorteerd in dek te zien.

            update_Message("Hoeveel spelers doen er mee?: ");
            string AantalSpelers = read_Message();
            nieuwspel.SpelersMaken(int.Parse(AantalSpelers));

            bool doorgaan = true;

            while (doorgaan)
            {
                update_Message(" ");

                nieuwspel.genKaarten();
                nieuwspel.schudKaarten();

                nieuwspel.InzetInnen();
                nieuwspel.StartKaartenGeven();

                nieuwspel.Starten();

                update_Message("Druk op enter voor een nieuwspel.");
                Console.ReadKey();
            }

        }

        private static void update_Message(string message)
        {
            Console.WriteLine(message);
        }

        private static string read_Message()
        {
            return Console.ReadLine();
        }
        private static ConsoleKey wait_For_Key_Message()
        {
            return Console.ReadKey(true).Key;
        }
    }
}
