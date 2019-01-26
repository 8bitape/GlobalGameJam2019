using UnityEngine;

public class PlayerSpawned
{
    public int PlayerID { get; set; }
    public GameObject Obj { get; set; }

    public PlayerSpawned(int playerID, GameObject obj)
    {
        this.PlayerID = playerID;
        this.Obj = obj;
    }
}
