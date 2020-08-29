using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1LifeBar;
    public GameObject player2LifeBar;

    public GameObject lifePrefab;

    private GameObject []p1Life = new GameObject[3];
    private GameObject []p2Life = new GameObject[3];

    public GameObject gameOverScreen;

    private void Awake() 
    {
        Player.onDeath += PlayerDeath;
    }

    private void Start()
    {
        Player.onHit += OnPlayerHit;
        InitGame();
    }

    private void InitGame()
    {
        gameOverScreen.SetActive(false);
        DestroyAllChildrens(player1LifeBar);
        DestroyAllChildrens(player2LifeBar);
        SpawnLifeBar();
        foreach (var o in GameObject.FindGameObjectsWithTag("Player"))
        {
            o.GetComponent<Player>().Respawn();
        }
    }

    private void SpawnLifeBar()
    {
        for (int i = 0; i < 3; i++) {
            GameObject islandView1 = Instantiate(lifePrefab, player1LifeBar.transform);
            p1Life[i] = islandView1;
            GameObject islandView2 = Instantiate(lifePrefab, player2LifeBar.transform);
            p2Life[i] = islandView2;
        }
    }

    public void OnPlayerHit(Player.PlayerTag tag, int hp)
    {
        if (tag == Player.PlayerTag.One)
        {
            p1Life[hp].GetComponent<LifeUI>().LifeDown();
        }
        else
        {
            p2Life[(Math.Abs(hp - 2))].GetComponent<LifeUI>().LifeDown();
        }
    }
    
    private void DestroyAllChildrens(GameObject obj) {
        foreach (Transform child in obj.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void PlayerDeath(Player.PlayerTag deadPlayerTag)
    {
        // TODO handle player death - GAME OVER screen?
        InputManager.onFire += RestartGame;
        Invoke("GameOverScreen", 2);
    }

    public void RestartGame(bool b)
    {
        InputManager.onFire -= RestartGame;
        
        InitGame();
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
}