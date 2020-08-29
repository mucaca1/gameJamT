using System;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private void Awake() 
    {
        Player.onDeath += PlayerDeath;
    }

    private void PlayerDeath(Player.PlayerTag deadPlayerTag)
    {
        // TODO handle player death - GAME OVER screen?
    }
}