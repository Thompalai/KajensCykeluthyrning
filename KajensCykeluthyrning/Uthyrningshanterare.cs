using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KajensCykeluthyrning
{
    public class Uthyrningshanterare
    {
        public List<Fordon> Fordon { get; set; }

        public List<Kund> Kunder { get; set; }

        public List<Uthyrning> AktivaUthyrningar { get; set; }

        private readonly CultureInfo svenskKultur =
            CultureInfo.GetCultureInfo("sv-SE");

        public Uthyrningshanterare()
        {
            Fordon = new List<Fordon>();
            Kunder = new List<Kund>();
            AktivaUthyrningar = new List<Uthyrning>();

            // Startfordon

            Fordon.Add(new Cykel(1));
            Fordon.Add(new Cykel(2));

            Fordon.Add(new Elcykel(3, 80));
            Fordon.Add(new Elcykel(4, 25));
        }

        // Registrera kund

        public bool RegistreraKund(string namn, int alder, bool medlem)
        {
            if (Kunder.Any(k =>
                k.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            Kund nyKund = new Kund(namn, alder, medlem);

            Kunder.Add(nyKund);

            return true;
        }

        // Hitta kund

        public Kund HittaKund(string namn)
        {
            return Kunder.FirstOrDefault(k =>
                k.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase));
        }

        // Hitta fordon

        public Fordon HittaFordon(int nummer)
        {
            return Fordon.FirstOrDefault(f => f.Nummer == nummer);
        }

        // Hyr ut fordon

        public bool HyrUt(
            string kundNamn,
            int fordonsNummer,
            int timmar,
            out string meddelande)
        {
            Kund kund = HittaKund(kundNamn);

            if (kund == null)
            {
                meddelande = "Kunden finns inte.";
                return false;
            }

            Fordon fordon = HittaFordon(fordonsNummer);

            if (fordon == null)
            {
                meddelande = "Fordonet finns inte.";
                return false;
            }

            if (timmar <= 0)
            {
                meddelande = "Antalet timmar måste vara större än noll.";
                return false;
            }

            if (fordon.ArUthyrd)
            {
                meddelande = "Fordonet är redan uthyrt.";
                return false;
            }

            // Kontrollera ålder

            if (fordon is Elcykel && kund.Alder < 18)
            {
                meddelande = "Kunden måste vara minst 18 år för att hyra elcykel.";
                return false;
            }

            if (fordon is Cykel && kund.Alder < 15)
            {
                meddelande = "Kunden måste vara minst 15 år för att hyra cykel.";
                return false;
            }

            // Kontrollera batteri

            if (fordon is Elcykel elcykel &&
                elcykel.Batteriniva < 30)
            {
                meddelande =
                    $"Elcykel {elcykel.Nummer} har för lite batteri " +
                    $"({elcykel.Batteriniva} %). Ladda den först.";

                return false;
            }

            // Skapa uthyrning

            Uthyrning uthyrning =
                new Uthyrning(kund, fordon, timmar);

            AktivaUthyrningar.Add(uthyrning);

            fordon.HyrUt();

            meddelande =
                $"{kund.Namn} hyr {fordon.Typ} {fordon.Nummer} i {timmar} timmar.";

            return true;
        }

        // Lämna tillbaka fordon

        public bool LamnaTillbaka(
            int fordonsNummer,
            out string kvitto)
        {
            Uthyrning uthyrning = AktivaUthyrningar.FirstOrDefault(
                u => u.Fordon.Nummer == fordonsNummer);

            if (uthyrning == null)
            {
                kvitto = "Det finns ingen aktiv uthyrning för detta fordon.";
                return false;
            }

            Fordon fordon = uthyrning.Fordon;

            Kund kund = uthyrning.Kund;

            int timmar = uthyrning.Timmar;

            // Beräkna pris

            decimal totalPris = fordon.Timpris * timmar;

            decimal rabatt = 0;

            if (kund.ArMedlem)
            {
                rabatt = totalPris * 0.10m;
            }

            decimal attBetala = totalPris - rabatt;

            // Förbruka batteri om det är en elcykel

            if (fordon is Elcykel elcykel)
            {
                elcykel.ForbrukaBatteri(timmar);
            }

            // Gör fordonet ledigt

            fordon.LamnaTillbaka();

            // Ta bort aktiv uthyrning

            AktivaUthyrningar.Remove(uthyrning);

            // Skapa kvitto

            kvitto = "\n========== KVITTO ==========\n";

            kvitto +=
                $"{fordon.Typ}, {timmar} timmar à " +
                $"{fordon.Timpris} kr\t" +
                $"{totalPris.ToString("N2", svenskKultur)} kr\n";

            if (kund.ArMedlem)
            {
                kvitto +=
                    $"Medlemsrabatt (10 %) \t-" +
                    $"{rabatt.ToString("N2", svenskKultur)} kr\n";
            }

            kvitto +=
                $"Att betala\t\t" +
                $"{attBetala.ToString("N2", svenskKultur)} kr\n";

            kvitto += "============================\n";

            return true;
        }

        // Ladda elcykel

        public bool LaddaFordon(int nummer, out string meddelande)
        {
            Fordon fordon = HittaFordon(nummer);

            if (fordon == null)
            {
                meddelande = "Fordonet finns inte.";
                return false;
            }

            if (fordon is not Elcykel elcykel)
            {
                meddelande = "Detta fordon har inget batteri.";
                return false;
            }

            if (elcykel.ArUthyrd)
            {
                meddelande = "En uthyrd elcykel kan inte laddas.";
                return false;
            }

            elcykel.Ladda();

            meddelande =
                $"Elcykel {nummer} är nu laddad till 100 %.";

            return true;
        }

        // Visa alla fordon

        public void VisaFordon()
        {
            Console.WriteLine("\nNr\tTyp\t\tStatus\t\tBatteri");

            foreach (Fordon fordon in Fordon)
            {
                string status = fordon.ArUthyrd
                    ? "Uthyrd"
                    : "Ledig";

                string batteri = "";

                if (fordon is Elcykel elcykel)
                {
                    batteri = $"{elcykel.Batteriniva} %";
                }

                Console.WriteLine(
                    $"{fordon.Nummer}\t{fordon.Typ}\t\t{status}\t\t{batteri}");
            }
        }
    }
}
