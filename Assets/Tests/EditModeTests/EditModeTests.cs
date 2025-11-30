using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class EditModeTests
{
    [Test]
    public void TestConfigPrefab_Obstacle_Madrid()
    {
        string assetPath = "Assets/Prefab/Obstacle.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        Assert.IsNotNull(prefab, "Le prefab obstacle est introuvable");

        var bc = prefab.GetComponent<BoxCollider>();
        Assert.IsNotNull(bc, "Le prefab obstacle ne détient pas de Box Collider");

        var render = prefab.GetComponent<MeshRenderer>();
        Assert.IsNotNull(render, "Le prefab obstacle ne détient pas de composant Renderer");

        string materialPath = "Assets/Materials/Obstacle_Mat.mat";
        var expectedMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        Assert.IsNotNull(expectedMaterial, $"Le material de référence n'a pas été trouvé au chemin {materialPath}");

        Assert.AreEqual(expectedMaterial, render.sharedMaterial, "Le material assigné à l'obstacle ne correspond pas au bon");
    }

    [Test]
    public void CollisionAvecObsatcle_PlayerMeurt_Madrid()
    {
        GameObject obstacleGO = new GameObject("Obstacle");
        Obstacle obstacle = obstacleGO.AddComponent<Obstacle>();

        IPlayerMovement mockPlayer = Substitute.For<IPlayerMovement>();

        obstacle.playerMovement = mockPlayer;

        GameObject playerObj = new GameObject("Player");

        obstacle.CheckCollision(playerObj);

        mockPlayer.Received(1).Die();

        Object.DestroyImmediate(obstacleGO);
        Object.DestroyImmediate(playerObj);
    }

    [Test]
    public void CollisionSol_PlayerMeurtPas_Madrid()
    {
        GameObject obstacleGO = new GameObject("Obstacle");
        Obstacle obstacle = obstacleGO.AddComponent<Obstacle>();

        IPlayerMovement mockPlayer = Substitute.For<IPlayerMovement>();
        obstacle.playerMovement = mockPlayer;

        GameObject groundObj = new GameObject("Ground");

        obstacle.CheckCollision(groundObj);

        mockPlayer.DidNotReceive().Die();

        Object.DestroyImmediate(obstacleGO);
        Object.DestroyImmediate(groundObj);
    }

    [Test]
    public void TestConfigPrefab_GroundTile_Emerick()
    {
        string assetPath = "Assets/Prefab/GroundTile.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        Assert.IsNotNull(prefab, "Le prefab GroundTile est introuvable");
        Assert.AreEqual("Ground", prefab.tag, "Le non du tag n'est pas le bon");

        var boxCollider = prefab.GetComponent<BoxCollider>();
        Assert.IsNotNull(boxCollider, "Le BoxCollider de GroundTile n'est pas trouver");
        Assert.IsTrue(boxCollider.isTrigger, "Le BoxCollider doit être en IsTrigger");

        var scritp = prefab.GetComponent<GroundTile>();
        Assert.IsNotNull(scritp, "Le script Ground Tile est introuvable");
        Assert.IsNotNull(scritp.tallObstacleChance, "Le parametre tallObstacleChance est introuvable");

        var trueValue = 0.3f;
        var expectedValue = scritp.tallObstacleChance;
        Assert.AreEqual(expectedValue, trueValue, "La valeur de tallObstacleChance n'est pas 0.3");
    }

    [Test]
    public void OnTriggerExit_Player_Emerick()
    {
        GameObject tileGround = new GameObject("GroundTile");
        var groundTileScript = tileGround.AddComponent<GroundTile>();

        IGroundSpawner mockSpawner = Substitute.For<IGroundSpawner>();
        groundTileScript.groundSpawner = mockSpawner;

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        var playerBC = player.AddComponent<BoxCollider>();

        groundTileScript.TraiterSortie(playerBC);
        mockSpawner.Received(1).SpawnTile(true);

        Object.DestroyImmediate(tileGround);
        Object.DestroyImmediate(player);
    }

    [Test]
    public void OnTriggerExit_NonPlayer_Emerick()
    {
        GameObject groundTile = new GameObject("GroundTile");
        var groundTileScript = groundTile.AddComponent<GroundTile>();

        IGroundSpawner mockSpawner = Substitute.For<IGroundSpawner>();
        groundTileScript.groundSpawner = mockSpawner;

        GameObject otherGO = new GameObject("AutreObjet");
        otherGO.tag = "Untagged";
        var otherBC = otherGO.AddComponent<BoxCollider>();

        groundTileScript.TraiterSortie(otherBC);
        mockSpawner.DidNotReceive().SpawnTile(true);

        Object.DestroyImmediate(groundTile);
        Object.DestroyImmediate(otherGO);
    }

    [Test]
    public void SpawnCoin_Emerick()
    {
        GameObject groundTile = new GameObject("GroundTile");
        var tile = groundTile.AddComponent<GroundTile>();
        BoxCollider bc = groundTile.AddComponent<BoxCollider>();
        bc.size = new Vector3(10, 1, 10);

        GameObject testCoin = new GameObject("Coin");
        tile.coinPrefab = testCoin;

        var objetAvant = groundTile.transform.childCount;
        tile.SpawnCoins();
        var objetApres = groundTile.transform.childCount;

        var expectedCoin = objetAvant + 10;
        var realCoin = objetApres;

        Assert.AreEqual(realCoin, expectedCoin, "Il devrait y avoir 10 piece en plus");

        Object.DestroyImmediate(groundTile);
        Object.DestroyImmediate(testCoin);
    }
}

