using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{

    NetworkManager _network;
    // Start is called before the first frame update
    void Start()
    {

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }

    void Update()
    {
        
    }

    
}
