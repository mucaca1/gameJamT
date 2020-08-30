using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player1LifeBar;
    public GameObject player2LifeBar;

    public GameObject lifePrefab;

    private GameObject []p1Life = new GameObject[3];
    private GameObject []p2Life = new GameObject[3];

    public GameObject gameOverScreen;
    public Text timerText;

    public int gameTime = 50;
    private int elapsedTime = 0;
    private System.Timers.Timer timer;

    public Text player1Score;
    public Text player2Score;

    private int player1ScoreCounter;
    private int player2ScoreCounter;

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
        Collectible.onCollect += ScoreUpdate;
    }

    private void InitGame()
    {
        player1ScoreCounter = -1;
        player2ScoreCounter = -1;

        ScoreUpdate(Player.PlayerTag.One, null);
        ScoreUpdate(Player.PlayerTag.Two, null);

        gameOverScreen.SetActive(false);

        DestroyAllChildrens(player1LifeBar);
        DestroyAllChildrens(player2LifeBar);
        
        timer.Start();
        timerText.text = "" + (gameTime - elapsedTime);

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
        else 
        {
            timerText.text = "" + (gameTime - elapsedTime);
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

    public void ScoreUpdate(Player.PlayerTag tag, Collectible collectible)
    {
        Debug.Log(tag + ", " + collectible);
        if (tag == Player.PlayerTag.One)
        {
            ChangeColor(player1Score, Color.green);
            player1Score.text = "Score: " + ++player1ScoreCounter;
            Invoke("Change1Color", 0.5f);
        }
        else
        {
            ChangeColor(player2Score, Color.green);
            player2Score.text = "Score: " + ++player2ScoreCounter;
            Invoke("Change2Color", 0.5f);
        }
    }

    public void Change1Color()
    {
        ChangeColor(player1Score, Color.white);
    }

    public void Change2Color()
    {
        ChangeColor(player2Score, Color.white);
    }

    public void ChangeColor(Text t, Color c)
    {
        t.color = c;
    }
}