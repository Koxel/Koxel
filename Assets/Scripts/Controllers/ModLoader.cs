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
    private Transform PrefabHolder;

    public GameObject defaultModel;
    public static Dictionary<string, Material> Materials;
    public static Dictionary<string, GameObject> Models;
    public static Dictionary<string, Asset_Interaction> AssetInteractions;
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
        GameObject PrefabHolderGO = new GameObject("PrefabHolder");
        PrefabHolderGO.transform.SetParent(this.transform);
        PrefabHolder = PrefabHolderGO.transform;
        PrefabHolderGO.SetActive(false);
    }

    public void LoadMods()
    {
        foreach (string Mod in Mods)
        {
            ImportMaterials(ModsFolder + Mod);
            ImportModels(ModsFolder + Mod);
            ImportAssetInteractions(ModsFolder + Mod);
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
            TileAsset.transform.SetParent(PrefabHolder);
            GameObject assetModel = Instantiate(model, TileAsset.transform);
            assetModel.name = "Model";
            assetModel.transform.position = new Vector3(0f, 0f, .3f);
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
                    //Set Tag
                    renderer.gameObject.tag = "TileAssetModel";
                }
            }
            //Chance
            int chance = 0;
            if(jObject["SpawnChance"] != null) chance = jObject["SpawnChance"].ToObject<int>();
            //SizeRange
            float[] ranges = new float[] { 1, 1 };
            if (jObject["SizeRange"] != null) ranges = jObject["SizeRange"].ToObject<float[]>();
            Vector2 sizeRange = new Vector2(ranges[0], ranges[1]);
            //AssetInteractions
            List<string> interactions = new List<string>();
            if (jObject["Interactions"] != null) interactions = jObject["Interactions"].ToObject<List<string>>();
            List<Asset_Interaction> assetInteractions = new List<Asset_Interaction>();
            foreach (string interaction in interactions)
            {
                Asset_Interaction _Interaction;
                if (AssetInteractions.TryGetValue(interaction, out _Interaction))
                    assetInteractions.Add(_Interaction);
            }

            //Create TileAsset
            TileAsset ta = TileAsset.AddComponent<TileAsset>();
            ta.Setup(name, TileAsset, chance, sizeRange, assetInteractions);
            TileAssets.Add(name, ta);
        }
    }

    private void ImportAssetInteractions(string ModPath)
    {
        AssetInteractions = new Dictionary<string, Asset_Interaction>();

        DirectoryInfo dir = new DirectoryInfo(ModPath + "/AssetInteractions");
        FileInfo[] files = dir.GetFiles("*.json", SearchOption.AllDirectories);

        foreach (FileInfo file in files)
        {
            //JSON
            string json = File.ReadAllText(file.FullName);
            JToken jObject = JToken.Parse(json);
            //Name
            string name = "New AssetInteration";
            if (jObject["Name"] != null) name = jObject["Name"].ToObject<string>();
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
            ///Create AssetInteraction with model copy as child
            GameObject prefab = new GameObject(name);
            prefab.transform.SetParent(PrefabHolder);
            GameObject assetModel = Instantiate(model, prefab.transform);
            assetModel.name = "Model";
            assetModel.transform.position = new Vector3(0f, 0f, 0f);
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

            //Create AssetInteraction
            Asset_Interaction AI = new Asset_Interaction(name, prefab);
            AssetInteractions.Add(name, AI);
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
            foreach (Transform child in model.transform) {
                MeshCollider coll = child.gameObject.AddComponent<MeshCollider>();
                coll.convex = true;
            }
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
            float[] gammaColor = new float[4];
            if (jObject["Color"] != null) gammaColor = jObject["Color"].ToObject<float[]>();
            
            double gamma = 1 / 2.2;
            float[] linearColor = new float[4];
            //You might want to not use "Math.Pow" as it is slow compared to multiply operator
            linearColor[0] = (float)Math.Pow(gammaColor[0], gamma);
            linearColor[1] = (float)Math.Pow(gammaColor[1], gamma);
            linearColor[2] = (float)Math.Pow(gammaColor[2], gamma);
            
            material.color = new Color(linearColor[0], linearColor[1], linearColor[2], linearColor[3]);

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
