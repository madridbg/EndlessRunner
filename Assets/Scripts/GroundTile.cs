using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public IGroundSpawner groundSpawner;
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public GameObject tallObstaclePrefab;
    public float tallObstacleChance = 0.3f;

    private void Awake()
    {
        if (groundSpawner == null)
        {
            var spawnerObj = GameObject.FindGameObjectWithTag("GroundSpawner");
            if (spawnerObj != null)
                groundSpawner = spawnerObj.GetComponent<IGroundSpawner>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TraiterSortie(other);
    }

    public void TraiterSortie(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            groundSpawner.SpawnTile(true);
            if(Application.isPlaying)
                Destroy(gameObject, 2);
            else
                DestroyImmediate(gameObject);
        }
    }

    public void SpawnObstacle()
    {
        GameObject obstacleToSpawn = obstaclePrefab;
        float random = Random.Range(0f, 1f);
        if (random < tallObstacleChance)
        {
            obstacleToSpawn = tallObstaclePrefab;
        }

        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;


        Instantiate(obstacleToSpawn, spawnPoint.position, Quaternion.identity, transform);
    }

    public void SpawnCoins()
    {
        int coinsToSpawn = 10;
        Collider col = GetComponent<Collider>();
        for (int i = 0; i < coinsToSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(col);
        }
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }

        point.y = 1;
        return point;
    }

}
