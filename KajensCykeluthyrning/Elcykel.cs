namespace KajensCykeluthyrning
{
    public class Elcykel : Fordon
    {
        public override string Typ => "Elcykel";

        public int Batteriniva { get; private set; }

        public Elcykel(int nummer, int batteriniva)
            : base(nummer, 70)
        {
            Batteriniva = batteriniva;
        }

        public void ForbrukaBatteri(int timmar)
        {
            Batteriniva -= timmar * 10;

            if (Batteriniva < 0)
            {
                Batteriniva = 0;
            }
        }

        public void Ladda()
        {
            Batteriniva = 100;
        }
    }
}
