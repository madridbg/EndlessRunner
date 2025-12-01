using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class EditModeTests
{
    [Test]
    public void TestConfigPrefab_Obstacle_Madrid()
    {
        string assetPath = "Assets/Prefab/Small Obstacles/SM_Prop_ParkBench_01.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        Assert.IsNotNull(prefab, "Le prefab obstacle est introuvable");

        var mc = prefab.GetComponent<MeshCollider>();
        Assert.IsNotNull(mc, "Le prefab obstacle ne détient pas de Mesh Collider");

        var render = prefab.GetComponent<MeshRenderer>();
        Assert.IsNotNull(render, "Le prefab obstacle ne détient pas de composant Renderer");

        string materialPath = "Assets/SyntyStudios/PolygonCity/Materials/PolygonCity_Mat_01_A.mat";
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
        playerObj.tag = "Player"; 

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

    [Test]
    public void TestConfigPrefab_Coin_Jacob()
    {
        const string path = "Assets/Prefab/Coin.prefab";

        var coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        Assert.IsNotNull(coinPrefab, "Coin.prefab non trouvé au chemin fourni");

        var isPrefab = PrefabUtility.IsPartOfPrefabAsset(coinPrefab);
        Assert.IsTrue(isPrefab, $"Le «{coinPrefab.name}» n'est pas un prefab");

        var composanteCapsuleCollider = coinPrefab.GetComponent<CapsuleCollider>();
        Assert.IsNotNull(composanteCapsuleCollider, $"Aucune composante CapsuleCollider associée au {coinPrefab.name}");

        var isTrigger = composanteCapsuleCollider.isTrigger;
        Assert.IsTrue(isTrigger, $"La composante {composanteCapsuleCollider.name} du {coinPrefab.name} n'est pas un Trigger");

        var composanteRigidBody = coinPrefab.GetComponent<Rigidbody>();
        Assert.IsNotNull(composanteRigidBody, $"Aucune composante RigidBody associée au {coinPrefab.name}");

        var usesGravity = composanteRigidBody.useGravity;
        Assert.IsTrue(usesGravity, $"La composante {composanteRigidBody.name} du {coinPrefab.name} n'utilise pas de gravité");

        var isKinematic = composanteRigidBody.isKinematic;
        Assert.IsTrue(isKinematic, $"La composante {composanteRigidBody.name} du {coinPrefab.name} n'est pas affecté par la physique");

        var composanteCoinScript = coinPrefab.GetComponent<Coin>();
        Assert.IsNotNull(composanteCoinScript, $"Aucune script Coin associée au {coinPrefab.name}");

        var audioClip = composanteCoinScript.pickupSound;
        Assert.IsNotNull(audioClip, $"Aucune valeur au champ pickupSound du script {composanteCoinScript.name} ");

    }
}

