using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static event Action<bool> gameStatus;
    
    public GameObject player1LifeBar;
    public GameObject player2LifeBar;

    public GameObject lifePrefab;

    public GameObject []p1Life = new GameObject[3];
    public GameObject []p2Life = new GameObject[3];

    public GameObject gameOverScreen;
    public Text timerText;

    public int gameTime = 50;
    private int elapsedTime = 0;
    private System.Timers.Timer timer;

    public Text player1Score;
    public Text player2Score;
    public Text winPlayerInfoText;

    private int player1ScoreCounter;
    private int player2ScoreCounter;

    private void Awake() 
    {
        Player.onDeath += PlayerDeath;
        Player.onSpawn += PlayerSpawn;
        Player.onHit += OnPlayerHit;
        Collectible.onCollect += ScoreUpdate;
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
        winPlayerInfoText.text = "";
        
        player1ScoreCounter = -1;
        player2ScoreCounter = -1;

        ScoreUpdate(Player.PlayerTag.One, null);
        ScoreUpdate(Player.PlayerTag.Two, null);

        DestroyAllChildrens(Player.PlayerTag.One);
        DestroyAllChildrens(Player.PlayerTag.Two);

        SpawnLifeBar(Player.PlayerTag.One);
        SpawnLifeBar(Player.PlayerTag.Two);
        
        foreach (var o in GameObject.FindGameObjectsWithTag("Player"))
        {
            o.GetComponent<Player>().Respawn();
        }

        timer.Start();
        timerText.text = "" + (gameTime - elapsedTime);

        gameOverScreen.SetActive(false);
        gameStatus?.Invoke(true);
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
    
    private void DestroyAllChildrens(Player.PlayerTag playerTag) {
        if  (playerTag == Player.PlayerTag.One) 
        {
            foreach (GameObject obj in p1Life)
            {
                Destroy(obj);
            }
        }
        else 
        {
            foreach (GameObject obj in p2Life)
            {
                Destroy(obj);
            }
        }
    }

    private void PlayerDeath(Player.PlayerTag deadPlayerTag) {}

    private void PlayerSpawn(Player player)
    {
        if  (player.playerTag == Player.PlayerTag.One)
        {
            foreach (var o in p1Life)
            {
                if (o == null)
                    continue;
                o.GetComponent<LifeUI>().LifeUp();
            }
        }
        else 
        {
            foreach (var o in p2Life)
            {
                if (o == null)
                    continue;
                o.GetComponent<LifeUI>().LifeUp();
            }
        }
    }

    public void RestartGame(bool b)
    {
        InputManager.onFire -= RestartGame;
        
        InitGame();
    }

    private void EndGame()
    {
        gameStatus?.Invoke(false);
        Debug.Log("GameOver");
        GameOverScreen();
    }

    private void GoToManu(bool b)
    {
        InputManager.onFire -= RestartGame;
        InputManager.onAction -= GoToManu;
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        winPlayerInfoText.text = "";
        Invoke("SecretText", 1f);
    }

    private void SecretText()
    {
        InputManager.onFire += RestartGame;
        InputManager.onAction += GoToManu;
        int i = (player1ScoreCounter > player2ScoreCounter) ? 1 : 2;
        winPlayerInfoText.text = "JK. Player " + i + " win. Maybe.";
        if (player1ScoreCounter == player2ScoreCounter)
            winPlayerInfoText.text = "No one wins. So sad. :(";
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

    private void OnDestroy() 
    {
        Player.onDeath -= PlayerDeath;
        Player.onSpawn -= PlayerSpawn;
        Player.onHit -= OnPlayerHit;  
        Collectible.onCollect -= ScoreUpdate;
    }
}