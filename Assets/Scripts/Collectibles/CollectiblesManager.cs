using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public Collectible[] collectibles;

    // Index of currently active collectible
    private int activeSpawnPoint = -1;

    private Player[] players;

    void Start() 
    {
        Collectible.onCollect += OnCollectibleCollected;
        players = FindObjectsOfType<Player>();

        activeSpawnPoint = UnityEngine.Random.Range(0, collectibles.Length);
        collectibles[activeSpawnPoint].Spawn();
    }

    private void Spawn() 
    {
        int furthest = 0;
        float maxAvgDistance = 0;

        float avgDistance;

        for (int i = 0; i < collectibles.Length; i++)
        {
            if (i == activeSpawnPoint)
                continue;

            avgDistance = (
                (Vector3.Distance(players[0].transform.position, collectibles[i].transform.position) + 
                Vector3.Distance(players[1].transform.position, collectibles[i].transform.position)) 
                / 2
            );
            Debug.Log(i + ": " + avgDistance);

            if (avgDistance > maxAvgDistance)
            {
                maxAvgDistance = avgDistance;
                furthest = i;
            }
        }

        activeSpawnPoint = furthest;

        //Debug.Log("Spawning: " + activeSpawnPoint);
        collectibles[activeSpawnPoint].Spawn();
    }

    private void OnCollectibleCollected(Player.PlayerTag player, Collectible collectible)
    {
        collectible.Despawn();
        Spawn();
    }

    private void OnDestroy() 
    {
        Collectible.onCollect -= OnCollectibleCollected;    
    }
}