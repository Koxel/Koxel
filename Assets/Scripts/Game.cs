using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public GameObject World;
    public GameObject Player;

    void Start()
    {
        World = transform.FindChild("World").gameObject;



        Load();
    }

    public void ExitGame()
    {
        Save();
        Application.Quit();
    }

    public void Save()
    {

    }

    public void Load()
    {
        //TODO: Restore save file

        World.GetComponent<NiceJsonLoader>().Parse();
        World.GetComponent<WorldGenerator>().Generate();
        Player = World.GetComponent<WorldGenerator>().CreatePlayer();
        World.GetComponent<Map>().player = Player;
        GetComponent<CameraController>().Setup(Player);
    }
}
