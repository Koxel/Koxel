using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using NiceJson;

public class MainMenuController : MonoBehaviour {

    public Transform contentList;
    public GameObject worldButtonPrefab;
    public GameObject CreateNewWorldMenu;

    private void Start()
    {
        // Scan
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Saves");
        DirectoryInfo[] info = dir.GetDirectories();

        foreach (DirectoryInfo directory in info)
        {
            //Debug.Log(directory.Name + "   " + directory.FullName);

            AddWorld(ParseWorldData(directory.FullName), directory.FullName);
        }
    }

    GameObject AddWorld(WorldData worldData, string path)
    {
        GameObject newButton = Instantiate(worldButtonPrefab, contentList.transform);
        newButton.GetComponent<SelectWorldButton>().name = worldData.name;
        newButton.GetComponent<SelectWorldButton>().path = path;
        newButton.GetComponent<SelectWorldButton>().CreateNewWorld = false;
        newButton.GetComponent<SelectWorldButton>().worldData = worldData;
        newButton.transform.GetChild(0).GetComponent<Text>().text = worldData.name;
        newButton.GetComponent<Button>().onClick.AddListener(delegate() { WorldSelect(); });
        return newButton;
    }

    public void Play()
    {
        if (selectedButton != null)
        {
            gameObject.SetActive(false);
            if (CreateNewWorldMenu.activeSelf)
            {
                string path = CreateWorldFile(CreateNewWorldMenu.transform.FindChild("NameInput").FindChild("Text").GetComponent<Text>().text,
                        CalcSeed(CreateNewWorldMenu.transform.FindChild("SeedInput").FindChild("Text").GetComponent<Text>().text));
                bool isNew = true;
                WorldData worldData = ParseWorldData(path);

                GameObject.Find("Game").GetComponent<Game>().Play(path, isNew, worldData);
            }
            else
            {
                GameObject.Find("Game").GetComponent<Game>().Play(selectedButton.GetComponent<SelectWorldButton>().path, false, selectedButton.GetComponent<SelectWorldButton>().worldData);
            }
        }
    }

    GameObject selectedButton;
    GameObject prevSelectedButton;
    public void WorldSelect()
    {
        selectedButton = EventSystem.current.currentSelectedGameObject;
        if (selectedButton != prevSelectedButton)
        {
            prevSelectedButton = selectedButton;
            SelectWorldButton button = selectedButton.GetComponent<SelectWorldButton>();
            if (button.CreateNewWorld)
                CreateNewWorldMenu.SetActive(true);
            else
                CreateNewWorldMenu.SetActive(false);
        }
        else
        {
            prevSelectedButton = null;
            selectedButton = null;
            CreateNewWorldMenu.SetActive(false);
        }
    }

    WorldData ParseWorldData(string worldPath)
    {
        string filePath = worldPath + "/world_data.json";
        JsonObject json = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(filePath));
        return new WorldData
        {
            name = json["name"],
            seed = json["seed"]
        };
    }

    string CreateWorldFile(string name, float seed)
    {
        Debug.Log("Made seed: " + seed);
        string path = Application.dataPath + "/Saves/" + TimeString(DateTime.Now.ToString());

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "/Chunks");
        }
        else
        {
            Debug.LogError("That world already exists");
            return null;
        }

        //Create the JSON
        JsonObject json = new JsonObject();
        json.Add("name", name);
        json.Add("seed", seed);

        string text = json.ToJsonPrettyPrintString();
        string filePath = path + "/" + "world_data.json";
        if (File.Exists(filePath))
            File.Delete(filePath);
        File.WriteAllText(filePath, text);

        //GetComponent<WorldData>().Store(name, seed);
        return path;
    }

    string TimeString(string time)
    {
        time = time.Replace("/", "-");
        time = time.Replace(" ", "_");
        time = time.Replace(":", "-");
        return time;
    }

    float CalcSeed(string seedText)
    {
        /*float seed = Random.Range(seedText.GetHashCode(), 100000);
        return seed;*/
        return seedText.GetHashCode();
    }
}
