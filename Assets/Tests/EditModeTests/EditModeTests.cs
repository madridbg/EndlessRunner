using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

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
}

