using System;

namespace KajensCykeluthyrning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Uthyrningshanterare hanterare =
                new Uthyrningshanterare();

            bool korProgram = true;

            while (korProgram)
            {
                Console.WriteLine("\n=== Kajens Cykeluthyrning ===");

                Console.WriteLine("1. Visa fordon");
                Console.WriteLine("2. Registrera kund");
                Console.WriteLine("3. Hyr ut");
                Console.WriteLine("4. Lämna tillbaka");
                Console.WriteLine("5. Ladda");
                Console.WriteLine("6. Avsluta");

                Console.Write("\nVal: ");

                string val = Console.ReadLine();

                switch (val)
                {
                    case "1":

                        hanterare.VisaFordon();

                        break;

                    case "2":

                        RegistreraKund(hanterare);

                        break;

                    case "3":

                        HyrUt(hanterare);

                        break;

                    case "4":

                        LamnaTillbaka(hanterare);

                        break;

                    case "5":

                        Ladda(hanterare);

                        break;

                    case "6":

                        korProgram = false;

                        Console.WriteLine("Programmet avslutas.");

                        break;

                    default:

                        Console.WriteLine("Ogiltigt val. Försök igen.");

                        break;
                }
            }
        }

        // Registrera kund

        static void RegistreraKund(Uthyrningshanterare hanterare)
        {
            Console.Write("Namn: ");

            string namn = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(namn))
            {
                Console.WriteLine("Du måste ange ett namn.");
                return;
            }

            Console.Write("Ålder: ");

            if (!int.TryParse(Console.ReadLine(), out int alder) ||
                alder < 0)
            {
                Console.WriteLine("Ange en giltig ålder.");
                return;
            }

            Console.Write("Medlem (j/n): ");

            string svar = Console.ReadLine().Trim().ToLower();

            if (svar != "j" && svar != "n")
            {
                Console.WriteLine("Svara med j eller n.");
                return;
            }

            bool medlem = svar == "j";

            bool lyckades =
                hanterare.RegistreraKund(namn, alder, medlem);

            if (lyckades)
            {
                Console.WriteLine($"{namn} är registrerad.");
            }
            else
            {
                Console.WriteLine("Kunden finns redan registrerad.");
            }
        }

        // Hyr ut fordon

        static void HyrUt(Uthyrningshanterare hanterare)
        {
            Console.Write("Kund: ");

            string kundNamn = Console.ReadLine();

            Console.Write("Fordonsnummer: ");

            if (!int.TryParse(Console.ReadLine(), out int nummer))
            {
                Console.WriteLine("Ogiltigt fordonsnummer.");
                return;
            }

            Console.Write("Antal timmar: ");

            if (!int.TryParse(Console.ReadLine(), out int timmar))
            {
                Console.WriteLine("Ogiltigt antal timmar.");
                return;
            }

            bool lyckades = hanterare.HyrUt(
                kundNamn,
                nummer,
                timmar,
                out string meddelande
            );

            Console.WriteLine(meddelande);
        }

        // Lämna tillbaka fordon

        static void LamnaTillbaka(Uthyrningshanterare hanterare)
        {
            Console.Write("Fordonsnummer: ");

            if (!int.TryParse(Console.ReadLine(), out int nummer))
            {
                Console.WriteLine("Ogiltigt fordonsnummer.");
                return;
            }

            bool lyckades = hanterare.LamnaTillbaka(
                nummer,
                out string kvitto
            );

            Console.WriteLine(kvitto);
        }

        // Ladda elcykel

        static void Ladda(Uthyrningshanterare hanterare)
        {
            Console.Write("Fordonsnummer: ");

            if (!int.TryParse(Console.ReadLine(), out int nummer))
            {
                Console.WriteLine("Ogiltigt fordonsnummer.");
                return;
            }

            hanterare.LaddaFordon(
                nummer,
                out string meddelande
            );

            Console.WriteLine(meddelande);
        }
    }
}