using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1LifeBar;
    public GameObject player2LifeBar;

    public GameObject lifePrefab;

    private void Awake() 
    {
        Player.onDeath += PlayerDeath;
    }

    private void InitGame()
    {
        DestroyAllChildrens(player1LifeBar);
        DestroyAllChildrens(player2LifeBar);
    }

    private void SpawnLifeBar()
    {
        
    }
    
    private void DestroyAllChildrens(GameObject obj) {
        foreach (Transform child in obj.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void PlayerDeath(Player.PlayerTag deadPlayerTag)
    {
        // TODO handle player death - GAME OVER screen?
    }
}