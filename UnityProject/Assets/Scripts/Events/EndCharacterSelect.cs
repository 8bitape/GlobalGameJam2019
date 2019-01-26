namespace Events
{
    public class EndCharacterSelect
    {
        public int PlayerOneSelectedCharacter;
        public int PlayerTwoSelectedCharacter;

        public EndCharacterSelect(int playerOneCharacterSelect, int playerTwoCharacterSelect)
        {
            this.PlayerOneSelectedCharacter = playerOneCharacterSelect;
            this.PlayerTwoSelectedCharacter = playerTwoCharacterSelect;
        }
    }
}