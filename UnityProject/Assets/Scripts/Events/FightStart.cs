namespace Events
{
    public class FightStart
    {
        EndCharacterSelect EndCharacterSelect { get; set; }

        public FightStart(EndCharacterSelect endCharacterSelect)
        {
            this.EndCharacterSelect = endCharacterSelect;
        }
    }
}
