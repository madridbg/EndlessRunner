using System.Collections;
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
}
