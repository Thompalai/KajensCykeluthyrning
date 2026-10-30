namespace KajensCykeluthyrning
{
    public class Cykel : Fordon
    {
        public override string Typ => "Cykel";

        public Cykel(int nummer)
            : base(nummer, 40)
        {
        }
    }
}
