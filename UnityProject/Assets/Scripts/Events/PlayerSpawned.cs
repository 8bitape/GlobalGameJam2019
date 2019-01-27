using UnityEngine;

namespace Events
{
    public class PlayerSpawned : PlayerEvent
    {
        public PlayerSpawned(Player player) : base(player)
        {

        }
    }
}
