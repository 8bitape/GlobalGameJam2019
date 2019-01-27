namespace Events
{
    public class CharacterSelectChange
    {
        // Player id 1 or 2
        public int playerID { get; set; }

        // Character ID starts at 0 and goes up to the number of
        // characters minus one. The numbers correspond to the position
        // on the character selection screen.
        // TODO: Ideally the mapping here would relate to a character enum
        //       and not care about the position on the character select screen
        public int newCharacterID { get; set; }
    
        // Direction will be -1 for left and 1 for right
        public int direction { get; set; }

        public CharacterSelectChange(int playerID, int newCharacterID, int direction)
        {
            this.playerID = playerID;
            this.newCharacterID = newCharacterID;
            this.direction = direction;
        }
    }
}