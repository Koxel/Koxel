using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMenu : MonoBehaviour {

    Animator animator;
    Animation anim;

    public GameObject[] options;

	void Start ()
    {
        animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
        
        //Setup();
    }
	
	void Update ()
    {
        if (options.Length == 0)
            return;

        if (Input.GetKeyDown(KeyCode.E) && !anim.isPlaying)
        {
            anim["Rotate"].speed = 1;
            anim["Rotate"].time = 0;
            anim.Play("Rotate");
            Rotate();
        }
        else if(Input.GetKeyDown(KeyCode.Q) && !anim.isPlaying)
        {
            anim["Rotate"].speed = -1;
            anim["Rotate"].time = anim["Rotate"].length;
            anim.Play("Rotate");
            RotateLeft();
        }
	}

    void Setup()
    {
        int index = 0;
        int indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        //0
        Transform hex0 = transform.GetChild(0);
        GameObject icon0 = Instantiate(options[index], hex0.GetChild(0));
        icon0.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //1
        Transform hex1 = transform.GetChild(1);
        GameObject icon1 = Instantiate(options[index], hex1.GetChild(0));
        icon1.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //2
        Transform hex2 = transform.GetChild(2);
        GameObject icon2 = Instantiate(options[index], hex2.GetChild(0));
        icon2.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //3
        Transform hex3 = transform.GetChild(3);
        GameObject icon3 = Instantiate(options[options.Length - 1], hex3.GetChild(0));
        icon3.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        icon3.name = "Icon";
        ShiftLeft(options);
    }

    public void Setup(List<AssetInteraction> interactions)
    {
        options = new GameObject[interactions.Count];
        for (int i = 0; i < interactions.Count; i++)
        {
            options[i] = interactions[i].sprite;
        } 

        int index = 0;
        int indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        //0
        Transform hex0 = transform.GetChild(0);
        GameObject icon0 = Instantiate(options[index], hex0.GetChild(0));
        icon0.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if(index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //1
        Transform hex1 = transform.GetChild(1);
        GameObject icon1 = Instantiate(options[index], hex1.GetChild(0));
        icon1.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //2
        Transform hex2 = transform.GetChild(2);
        GameObject icon2 = Instantiate(options[index], hex2.GetChild(0));
        icon2.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //3
        Transform hex3 = transform.GetChild(3);
        GameObject icon3 = Instantiate(options[options.Length-1], hex3.GetChild(0));
        icon3.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        icon3.name = "Icon";
        ShiftLeft(options);
    }

    public static void ShiftLeft<T>(T[] arr)
    {
        T x = arr[0];
        Array.Copy(arr, 1, arr, 0, arr.Length - 1);
        Array.Clear(arr, arr.Length - 1, 1);
        arr[arr.Length-1] = x;
    }
    public static void ShiftRight<T>(T[] arr)
    {
        T x = arr[arr.Length-1];
        Array.Copy(arr, 0, arr, 1, arr.Length - 1);
        Array.Clear(arr, 0, 1);
        arr[0] = x;
    }

    public void Rotate()
    {
        //Remove old icons
        for (int childnr = 0; childnr < transform.childCount; childnr++)
        {
            Transform child = transform.GetChild(childnr);
            try
            {
                Destroy(child.GetChild(0).GetChild(0).gameObject);
            }
            catch (NullReferenceException) { }
        }

        ShiftRight(options);

        int index = 0;
        int indexChange = 1;
        //0
        Transform hex0 = transform.GetChild(0);
        GameObject icon0 = Instantiate(options[index], hex0.GetChild(0));
        icon0.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //1
        Transform hex1 = transform.GetChild(1);
        GameObject icon1 = Instantiate(options[index], hex1.GetChild(0));
        icon1.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //2
        Transform hex2 = transform.GetChild(2);
        GameObject icon2 = Instantiate(options[index], hex2.GetChild(0));
        icon2.name = "Icon";
        if (index + 1 >= options.Length)
            indexChange = -1;
        else if (index - 1 <= 0)
            indexChange = 1;
        if (options.Length == 1)
            indexChange = 0;
        index += indexChange;
        //3
        Transform hex3 = transform.GetChild(3);
        GameObject icon3 = Instantiate(options[options.Length - 1], hex3.GetChild(0));
        icon3.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        icon3.name = "Icon";
        prev = false;
    }
    bool prev = true;
    public void RotateLeft()
    {
        ShiftLeft(options);
        if (!prev)
            prev = true;
        else
        {
            //prev = false;

            //Remove old icons
            for (int childnr = 0; childnr < transform.childCount; childnr++)
            {
                Transform child = transform.GetChild(childnr);
                try
                {
                    Destroy(child.GetChild(0).GetChild(0).gameObject);
                }
                catch (NullReferenceException) { }
                catch (UnityException) { }
            }

            int index = 0;
            int indexChange = -1;
            //0
            Transform hex0 = transform.GetChild(0);
            GameObject icon0 = Instantiate(options[options.Length - 1], hex0.GetChild(0));
            icon0.name = "Icon";
            if (index + 1 >= options.Length)
                indexChange = -1;
            else if (index - 1 <= 0)
                indexChange = 1;
            if (options.Length == 1)
                indexChange = 0;
            index += indexChange;

            //3
            Transform hex2 = transform.GetChild(3);
            GameObject icon2 = Instantiate(options[options.Length - 1 - index], hex2.GetChild(0));
            icon2.name = "Icon";
            //2
            Transform hex1 = transform.GetChild(2);
            GameObject icon1 = Instantiate(options[index], hex1.GetChild(0));
            icon1.name = "Icon";
            //1
            Transform hex3 = transform.GetChild(1);
            GameObject icon3 = Instantiate(options[0], hex3.GetChild(0));
            icon3.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            icon3.name = "Icon";
        }
    }
}
