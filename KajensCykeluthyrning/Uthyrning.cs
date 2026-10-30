namespace KajensCykeluthyrning
{
    public class Uthyrning
    {
        public Kund Kund { get; set; }

        public Fordon Fordon { get; set; }

        public int Timmar { get; set; }

        public Uthyrning(Kund kund, Fordon fordon, int timmar)
        {
            Kund = kund;
            Fordon = fordon;
            Timmar = timmar;
        }
    }
}
