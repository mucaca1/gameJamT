using System;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static event Action<Player.PlayerTag> onPlayerDeath;

    private void Awake() 
    {
        onPlayerDeath += PlayerDeath;
    }

    private void PlayerDeath(Player.PlayerTag deadPlayerTag)
    {
        // TODO handle player death - GAME OVER screen?
    }
}