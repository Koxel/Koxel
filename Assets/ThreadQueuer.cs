//-----------------------------------------------------------------------
// <copyright file="ThreadQueuer.cs" company="Quill18 Productions">
//     Copyright (c) Quill18 Productions. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class ThreadQueuer : MonoBehaviour 
{
    public static ThreadQueuer instance;
    /*void Start()
    {
        // BIG HUGE NOTE:     WebGL doesn't do multithreading.

        Debug.Log("Start() -- Started.");
        functionsToRunInMainThread = new List<Action>();

        StartThreadedFunction( () => { SlowFunctionThatDoesAUnityThing( Vector3.zero, new float[4], new Color[100] ); } );

        Debug.Log("Start() -- Done.");
    }*/

    void Awake()
    {
        instance = this;
        functionsToRunInMainThread = new List<Action>();
    }

    void Update()
    {
        // Update() always runs in the main thread

        while(functionsToRunInMainThread.Count > 0)
        {
            // Grab the first/oldest function in the list
            Action someFunc = functionsToRunInMainThread[0];
            functionsToRunInMainThread.RemoveAt(0);

            // Now run it
            someFunc();
        }
    }

    List<Action> functionsToRunInMainThread;

    public void StartThreadedFunction( Action someFunctionWithNoParams )
    {
        Thread t = new Thread( new ThreadStart( someFunctionWithNoParams ) );
        t.Start();
    }

    public void QueueMainThreadFunction( Action someFunction )
    {
        // We need to make sure that someFunction is running from the
        // main thread

        //someFunction(); // This isn't okay, if we're in a child thread

        functionsToRunInMainThread.Add(someFunction);
    }

    /*void SlowFunctionThatDoesAUnityThing( Vector3 foo, float[] bar, Color[] pixels )
    {
        // First we do a really slow thing
        Thread.Sleep(2000); // Sleep for 2 seconds

        // Now we need to modify a Unity gameobject
        Action aFunction = () => {
            Debug.Log("The results of the child thread are being applied to a Unity GameObject safely.");
            this.transform.position = new Vector3(1,1,1);   // NOT ALLOWED FROM A CHILD THREAD
        };

        // NOTE: We still aren't allowed to call this from a child thread
        //aFunction();

        QueueMainThreadFunction( aFunction );
    }*/
}
