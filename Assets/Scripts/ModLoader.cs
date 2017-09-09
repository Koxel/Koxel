using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class ModLoader : MonoBehaviour {

    public static ModLoader instance;

    public string[] Mods;
    private string ModsFolder;
    private string GameFolder;
    private Transform ModelsHolder;
    private Transform TileAssetsHolder;

    public GameObject defaultModel;
    public static Dictionary<string, Material> Materials;
    public static Dictionary<string, GameObject> Models;
    public static Dictionary<string, TileAsset> TileAssets;

    void Awake()
    {
        instance = this;

        ModsFolder = Directory.GetParent(Application.dataPath) + "/Mods/";
        GameFolder = Directory.GetParent(Application.dataPath) + "/Game/";

        GameObject ModelsHolderGO = new GameObject("ModelsHolder");
        ModelsHolderGO.transform.SetParent(this.transform);
        ModelsHolder = ModelsHolderGO.transform;
        ModelsHolderGO.SetActive(false);
        GameObject TileAssetsHolderGO = new GameObject("TileAssetsHolder");
        TileAssetsHolderGO.transform.SetParent(this.transform);
        TileAssetsHolder = TileAssetsHolderGO.transform;
        TileAssetsHolderGO.SetActive(false);

        foreach (string Mod in Mods)
        {
            ImportMaterials(ModsFolder + Mod);
            ImportModels(ModsFolder + Mod);
            ImportTileAssets(ModsFolder + Mod);
        }
    }

    private void ImportTileAssets(string ModPath)
    {
        TileAssets = new Dictionary<string, TileAsset>();

        //Find files in the dir
        DirectoryInfo dir = new DirectoryInfo(ModPath + "/TileAssets");
        FileInfo[] files = dir.GetFiles("*.asset.json", SearchOption.AllDirectories);
        //for every file in that dir parse json :D
        foreach (FileInfo file in files)
        {
            //JSON
            string json = File.ReadAllText(file.FullName);
            JToken jObject = JToken.Parse(json);
            //Name
            string name = "New TileAsset"; ///DEFAULT VALUE
            if (jObject["Name"] != null) name = jObject["Name"].ToObject<string>(); ///IF JSON CONTAINS CHANGE VALUE
            //Model
            GameObject model = null;
            string modelName = "UNDEFINED";
            if (jObject["Model"] != null) modelName = jObject["Model"].ToObject<string>();
            if (modelName != "UNDEFINED") Models.TryGetValue(modelName, out model);
            if (model == null) model = defaultModel;
            //Materials
            Dictionary<string, string> materials = new Dictionary<string, string>();
            if (jObject["Materials"] != null) materials = jObject["Materials"].ToObject<Dictionary<string, string>>();
            //Create 'prefab'
            ///Create TileAsset with model copy as child
            GameObject TileAsset = new GameObject(name);
            TileAsset.transform.SetParent(TileAssetsHolder);
            GameObject assetModel = Instantiate(model, TileAsset.transform);
            assetModel.name = "Model";
            ///Set the corresponding materials on renderers
            if (modelName != "UNDEFINED")
            {
                MeshRenderer[] renderers = assetModel.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer renderer in renderers)
                {
                    Material[] materialList = renderer.materials;
                    Material[] newMaterials = new Material[materialList.Length];
                    foreach (Material material in materialList)
                    {
                        string materialName = material.name.Replace(" (Instance)", "");
                        string newMaterialName = "";
                        if (!materials.TryGetValue(materialName, out newMaterialName))
                            break;
                        Material newMaterial;
                        if (!Materials.TryGetValue(newMaterialName, out newMaterial))
                            break;
                        List<Material> mats = materialList.ToList();
                        int i = mats.IndexOf(material);
                        newMaterials[i] = newMaterial;
                    }
                    renderer.materials = newMaterials;
                }
            }
            TileAsset tileAsset = new TileAsset(name, TileAsset);
            TileAssets.Add(name, tileAsset);
        }
    }

    private void ImportModels(string ModPath)
    {
        Models = new Dictionary<string, GameObject>();

        DirectoryInfo dir = new DirectoryInfo(ModPath + "/Models");
        FileInfo[] files = dir.GetFiles("*.obj", SearchOption.AllDirectories);

        foreach (FileInfo file in files)
        {
            GameObject model = OBJLoader.LoadOBJFile(file.FullName);
            model.transform.SetParent(ModelsHolder);
            string modelname = file.FullName.Replace(@"\", "/").Remove(0, (ModPath + "/Models/").Count());
            Models.Add(modelname, model);
        }
    }

    private void ImportMaterials(string ModPath)
    {
        Materials = new Dictionary<string, Material>();

        DirectoryInfo dir = new DirectoryInfo(ModPath + "/Materials");
        FileInfo[] files = dir.GetFiles("*.json", SearchOption.AllDirectories);

        foreach (FileInfo file in files)
        {
            //JSON
            string json = File.ReadAllText(file.FullName);
            JToken jObject = JToken.Parse(json);
            //Shader
            string shader = "Standard";
            if (jObject["Shader"] != null) shader = jObject["Shader"].ToObject<string>();
            Material material = new Material(Shader.Find(shader));
            //Color
            float[] clr = new float[4];
            if (jObject["Color"] != null) clr = jObject["Color"].ToObject<float[]>();
            material.color = new Color(clr[0], clr[1], clr[2], clr[3]);
            //Name
            string name = "New Material";
            if (jObject["Name"] != null) name = jObject["Name"].ToObject<string>();
            else name = name + Materials.Count;
            material.name = name;

            //End
            Materials.Add(name, material);
        }
    }
}
