using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceJson;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen_Controller : MonoBehaviour {

    public string path;

    public Text worldName;
    public Text seed;

    public void PlayGame()
    {
        if(worldName.text != "")
        {
            CreateWorldFile(worldName.text, Random.Range(seed.text.GetHashCode(), 100000));

            SceneManager.LoadScene("World");
        }


    }

    void CreateWorldFile(string name, float seed)
    {
        Debug.Log("Made seed: " + seed);
        path = Application.dataPath + "/Saves/" + name;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "/Chunks");
        }
        else
        {
            Debug.LogError("That world already exists");
            return;
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
    }
}
