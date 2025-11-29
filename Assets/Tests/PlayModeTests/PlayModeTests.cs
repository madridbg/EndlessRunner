using NUnit.Framework;
using System.Collections;
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

        Assert.IsTrue(playerMovement.alive, "Le player devrait être vivant du début du test");

        yield return new WaitForSeconds(3.0f);

        var obstacle = GameObject.FindGameObjectWithTag("Obstacle");
        Assert.IsNotNull(obstacle, "Auncun obstacle trouvé avec le tag obstacle");

        player.transform.position = obstacle.transform.position;

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        Assert.IsFalse(playerMovement.alive, "Le joueur devrait être mort après la collision");
    }

    [UnityTest]
    public IEnumerator ObstacleSansJoueur_Madrid()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;

        }

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerGO, "Player avec tag Player existe pas");

        Object.Destroy(playerGO);

        yield return null;

        var obstacleGO = new GameObject("Obstacle");
        var obstacle = obstacleGO.AddComponent<Obstacle>();
        Assert.IsTrue(true);


    }


}
