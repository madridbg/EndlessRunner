using UnityEngine;

public interface IGroundSpawner
{
    void SpawnTile(bool spawnItems);
}

public class GroundSpawner : MonoBehaviour, IGroundSpawner
{
    [SerializeField] GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile(bool spawnItems)
    {
        GameObject ground = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = ground.transform.GetChild(1).transform.position;

        if (spawnItems)
        {
            ground.GetComponent<GroundTile>().SpawnObstacle();
            ground.GetComponent<GroundTile>().SpawnCoins();
        }
    }
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            if (i < 3)
            {
                SpawnTile(false);
            }
            else
            {
                SpawnTile(true);
            }
        }
    }
}
