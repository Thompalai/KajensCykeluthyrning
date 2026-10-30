namespace KajensCykeluthyrning
{
    public abstract class Fordon
    {
        public int Nummer { get; set; }

        public decimal Timpris { get; set; }

        public bool ArUthyrd { get; private set; }

        public abstract string Typ { get; }

        public Fordon(int nummer, decimal timpris)
        {
            Nummer = nummer;
            Timpris = timpris;
            ArUthyrd = false;
        }

        public void HyrUt()
        {
            ArUthyrd = true;
        }

        public void LamnaTillbaka()
        {
            ArUthyrd = false;
        }
    }
}
