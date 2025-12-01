using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerMovementTests
{

    [UnityTest]
    public IEnumerator TriggerEnterChangesIsOnGround()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;

        var player = GameObject.Find("Player");
        Assert.IsNotNull(player);

        var playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsNotNull(playerMovement);

        playerMovement.MyDependencies = Substitute.For<PlayerDependencies>();
        playerMovement.MyDependencies.isOnGround = false;
        player.transform.position = new Vector3(player.transform.position.x, 1, player.transform.position.z);

        yield return new WaitForSeconds(1.0f);

        var expectedIsOnGroundValue = true;
        var actualIsOnGroundValue = playerMovement.MyDependencies.isOnGround;
        Assert.AreEqual(expectedIsOnGroundValue, actualIsOnGroundValue);
    }

    [UnityTest]
    public IEnumerator TriggerEnterStartsCoinParticle()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;

        var player = GameObject.Find("Player");
        Assert.IsNotNull(player);

        var playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsNotNull(playerMovement);

        var coinParticle = playerMovement.twinkleEffect;
        Assert.IsNotNull(coinParticle);
        Assert.IsFalse(coinParticle.isPlaying, "Les particules de la pièce jouent déjà");


        GameObject coin = GameObject.FindWithTag("Coin");
        coin.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(coinParticle.isPlaying, "Les particules de la pièce ne jouent pas");
    }

    [UnityTest]
    public IEnumerator DieMethodDoesNotRunWhenDead()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;

        var player = GameObject.Find("Player");
        Assert.IsNotNull(player);

        var playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsNotNull(playerMovement);

        playerMovement.MyDependencies = Substitute.For<PlayerDependencies>();
        playerMovement.MyDependencies.isAlive = false;
        playerMovement.MyDependencies.isOnGround = false;

        var explosionParticle = playerMovement.explosionParticle;
        Assert.IsNotNull(explosionParticle);
        Assert.IsFalse(explosionParticle.isPlaying, "Les particules d'explosions sont émises");

        var dustParticle = playerMovement.dust;
        Assert.IsNotNull(dustParticle);
        Assert.IsFalse(dustParticle.isPlaying, "Les particules de poussière sont émises");

        playerMovement.Die();
        yield return new WaitForFixedUpdate();

        Assert.IsFalse(dustParticle.isPlaying, "Les particules sont simulées alors que l'on est mort");
        Assert.IsFalse(explosionParticle.isPlaying, "Les particules d'explosions sont émises");
    }

    [UnityTest]
    public IEnumerator DieMethodRunsWhenAlive()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;

        var player = GameObject.Find("Player");
        Assert.IsNotNull(player);
        var mainCamera = GameObject.Find("Main Camera");

        var playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsNotNull(playerMovement);

        playerMovement.MyDependencies = Substitute.For<PlayerDependencies>();
        playerMovement.MyDependencies.isAlive = true;
        playerMovement.MyDependencies.isOnGround = true;

        var explosionParticle = playerMovement.explosionParticle;
        Assert.IsNotNull(explosionParticle);
        Assert.IsFalse(explosionParticle.isPlaying, "Les particules d'explosions sont émises");

        var dustParticle = playerMovement.dust;
        Assert.IsNotNull(dustParticle);
        Assert.IsFalse(dustParticle.isPlaying, "Les particules de poussière sont émises");

        var backgroundAudio = mainCamera.GetComponent<AudioSource>();
        Assert.IsNotNull(backgroundAudio);
        Assert.IsTrue(backgroundAudio.isPlaying, "La musique de fond ne joue pas dès le début de la scène");

        playerMovement.Die();
        yield return new WaitForFixedUpdate();

        Assert.IsFalse(backgroundAudio.isPlaying, "La musique joue après la mort");
        Assert.IsFalse(dustParticle.isPlaying, "Les particules sont simulées alors que l'on est mort");
        Assert.IsTrue(explosionParticle.isPlaying, "Les particules d'explosions ne sont pas émises");
    }

    [UnityTest]
    public IEnumerator JumpMethodSendsUpwardsThenDown()
    {
        const string scenePath = "Assets/Scenes/EndlessRunner.unity";
        var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;

        var player = GameObject.Find("Player");
        Assert.IsNotNull(player);

        var playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsNotNull(playerMovement);

        playerMovement.MyDependencies = Substitute.For<PlayerDependencies>();
        playerMovement.MyDependencies.isOnGround = true;

        var initialPosition = player.transform.position;

        playerMovement.Jump();

        yield return new WaitForFixedUpdate();

        var actualPosition = player.transform.position;

        Assert.IsTrue(initialPosition.y < actualPosition.y, "Le joueur n'a pas sauté");
        Assert.IsFalse(playerMovement.MyDependencies.isOnGround, "Le joueur est sur le sol");

        yield return new WaitForSeconds(1.5f);
        actualPosition = player.transform.position;
        Assert.IsTrue(initialPosition.y - actualPosition.y < 1, $"Le joueur n'est pas redescendu, {initialPosition.y} {actualPosition.y}");
        Assert.IsTrue(playerMovement.MyDependencies.isOnGround, "Le joueur n'est pas sur le sol");

    }
}
