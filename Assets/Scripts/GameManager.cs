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

    public int gameTime = 50;
    private int elapsedTime = 0;
    private System.Timers.Timer timer;

    private void Awake() 
    {
        Player.onDeath += PlayerDeath;
        Player.onSpawn += PlayerSpawn;
        Player.onHit += OnPlayerHit;
    }

    private void Start()
    {
        timer = new System.Timers.Timer(1000);
        timer.AutoReset = true;
        timer.Elapsed += Tick;

        InitGame();
    }

    private void InitGame()
    {
        gameOverScreen.SetActive(false);
        DestroyAllChildrens(player1LifeBar);
        DestroyAllChildrens(player2LifeBar);
        
        timer.Start();

        // This is done in PlayerSpawn when player spawns
        //SpawnLifeBar(Player.PlayerTag.One);
        //SpawnLifeBar(Player.PlayerTag.Two);
        
        foreach (var o in GameObject.FindGameObjectsWithTag("Player"))
        {
            o.GetComponent<Player>().Respawn();
        }
    }

    void Update() 
    {
        // Bacasue fucking callback scope in C# -_-
        if (elapsedTime > gameTime)
        {
            timer.Stop();
            EndGame();
            elapsedTime = 0;
        }
    }

    private void Tick(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        elapsedTime++;
    }

    private void SpawnLifeBar(Player.PlayerTag playerTag)
    {
        for (int i = 0; i < 3; i++) {
            if  (playerTag == Player.PlayerTag.One) 
            {
                GameObject islandView1 = Instantiate(lifePrefab, player1LifeBar.transform);
                p1Life[i] = islandView1;
            }
            else 
            {
                GameObject islandView2 = Instantiate(lifePrefab, player2LifeBar.transform);
                p2Life[i] = islandView2;
            }
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

    private void PlayerDeath(Player.PlayerTag deadPlayerTag) {}

    private void PlayerSpawn(Player player)
    {
        if  (player.playerTag == Player.PlayerTag.One) 
        {
            DestroyAllChildrens(player1LifeBar);
        }
        else 
        {
            DestroyAllChildrens(player2LifeBar);
        }

        SpawnLifeBar(player.playerTag);
    }

    public void RestartGame(bool b)
    {
        InputManager.onFire -= RestartGame;
        
        InitGame();
    }

    private void EndGame()
    {
        Debug.Log("GameOver");
        InputManager.onFire += RestartGame;
        Invoke("GameOverScreen", 2);
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
}