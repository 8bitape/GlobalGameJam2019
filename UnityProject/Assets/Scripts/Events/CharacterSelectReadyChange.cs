namespace Events
{
    public class CharacterSelectReadyChange
    {
        // Player id 1 or 2
        public int playerID { get; set; }

        // Character ID starts at 0 and goes up to the number of
        // characters minus one. The numbers correspond to the position
        // on the character selection screen.
        // TODO: Ideally the mapping here would relate to a character enum
        //       and not care about the position on the character select screen
        public int newCharacterID { get; set; }

        // Ready will be true if the player confirmed their character selection
        // and are ready to start, and false if the player cancelled the character
        // selection and are no longer ready
        public bool ready { get; set; }

        public CharacterSelectReadyChange(int playerID, int newCharacterID, bool ready)
        {
            this.playerID = playerID;
            this.newCharacterID = newCharacterID;
            this.ready = ready;
        }
    }
}