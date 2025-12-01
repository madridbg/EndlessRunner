using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeTests
{
    [UnityTest]
    public IEnumerator GameTest_Madrid()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;

        }

        var player = GameObject.Find("Player");
        Assert.IsTrue(player != null, "Pas de joueur trouvé par le nom");

        var playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsTrue(playerMovement != null, "Le script PlayerMovement n'existe pas sur player");

        Assert.IsTrue(playerMovement.MyDependencies.isAlive, "Le player devrait être vivant du début du test");

        yield return new WaitForSeconds(3.0f);

        var obstacle = GameObject.FindGameObjectWithTag("Obstacle");
        Assert.IsNotNull(obstacle, "Auncun obstacle trouvé avec le tag obstacle");

        player.transform.position = obstacle.transform.position;

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        Assert.IsFalse(playerMovement.MyDependencies.isAlive, "Le joueur devrait être mort après la collision");
    }

    [UnityTest]
    public IEnumerator ObstacleSansJoueurMovement_Madrid()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;

        }

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerGO, "Player avec tag Player existe pas");

        // Object.Destroy(playerGO);
        var playerMovement = playerGO.GetComponent<PlayerMovement>();
        Object.Destroy(playerMovement);

        yield return null;

        var obstacleGO = new GameObject("Obstacle");
        var obstacle = obstacleGO.AddComponent<Obstacle>();
        Assert.IsTrue(true);


    }

    [UnityTest]
    public IEnumerator GameTest_Emerick()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;

        }

        var groundTile = GameObject.Find("GroundTile(Clone)");
        Assert.IsNotNull(groundTile, "Pas de grondTile trouvé par le nom");

        var scriptGroundTile = groundTile.GetComponent<GroundTile>();
        Assert.IsNotNull(scriptGroundTile, "Le script GroundTile est introuvable");

        var player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Pas de player trouvé par le nom");

        var positionEndTile = new Vector3(0, 0, 10);
        player.transform.position = groundTile.transform.position + positionEndTile;

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        Assert.IsNotNull(groundTile, "La tuile ne devrait pas être détruite immédiatement");

        yield return new WaitForSeconds(2.1f);

        Assert.IsTrue(groundTile == null, "La tuile aurait dû être détruite après 2 secondes");
    }
}
