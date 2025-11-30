using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public IGroundSpawner groundSpawner;
    public GameObject[] smallObstaclesToSpawn;
    public GameObject[] largeObstaclesToSpawn;
    public GameObject coinPrefab;
    public float tallObstacleChance = 0.3f;


    private const string GROUNDSPAWNER_NAME = "GroundSpawner";
    private const string PLAYER_TAG = "Player";

    private void Awake()
    {
        var spawnerObj = GameObject.FindGameObjectWithTag(GROUNDSPAWNER_NAME);
        if (spawnerObj == null)
        {
            Debug.LogError($"Aucun objet nommé {GROUNDSPAWNER_NAME} trouvé dans la scène.");
            enabled = false;
            return;
        }
        groundSpawner = spawnerObj.GetComponent<IGroundSpawner>();
        if (groundSpawner == null)
        {

            Debug.LogError($"Aucune composante IGroundSpawner associée à l'objet {GROUNDSPAWNER_NAME} trouvé dans la scène.");
            enabled = false;
            return;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        TraiterSortie(other);
    }

    public void TraiterSortie(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            groundSpawner.SpawnTile(true);
            if (Application.isPlaying)
                Destroy(gameObject, 2);
            else
                DestroyImmediate(gameObject);
        }
    }

    public void SpawnObstacle()
    {
        GameObject obstacleToSpawn = RandomGameObject;
        _ = Instantiate(obstacleToSpawn, obstacleToSpawn.transform.position, obstacleToSpawn.transform.rotation, transform);
    }

    private GameObject RandomGameObject
    {
        get
        {
            int obstacleIndex = Random.Range(0, smallObstaclesToSpawn.Length);
            GameObject obstacleToSpawn = smallObstaclesToSpawn[obstacleIndex];
            obstacleToSpawn.transform.position = RandomPositionSmall;

            // Déterminer s'il s'agit d'un obstacle large ou non. (Peut être modifié dans l'inspecteur)
            float random = Random.Range(0.0f, 1.0f);
            if (random < tallObstacleChance)
            {
                obstacleIndex = Random.Range(0, largeObstaclesToSpawn.Length);
                obstacleToSpawn = largeObstaclesToSpawn[obstacleIndex];
                obstacleToSpawn.transform.position = RandomPositionLarge;
            }


            return obstacleToSpawn;
        }
    }

    private Vector3 RandomPositionSmall
    {
        get
        {
            float randomXPosition = Random.Range(-3.3f, 3.3f);
            float groundYPosition = transform.position.y;
            float groundZPosition = transform.position.z;
            Vector3 spawnPoint = new Vector3(randomXPosition, groundYPosition, groundZPosition);
            return spawnPoint;
        }

    }

    // Puisque les objets larges prennent beaucoup de place, leur point d'apparition est différent que celui des petits obstacles.
    private Vector3 RandomPositionLarge
    {
        get
        {
            float randomXPosition = Random.Range(-3.0f, 3.0f);
            float groundYPosition = transform.position.y;
            float groundZPosition = transform.position.z;
            Vector3 spawnPoint = new Vector3(randomXPosition, groundYPosition, groundZPosition);
            return spawnPoint;
        }

    }


    public void SpawnCoins()
    {
        int coinsToSpawn = 5;
        Collider col = GetComponent<Collider>();
        for (int i = 0; i < coinsToSpawn; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.transform.position = GetRandomPointInCollider(col);
        }
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        int limitOffset = 1;
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x + limitOffset, collider.bounds.max.x - limitOffset),
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
