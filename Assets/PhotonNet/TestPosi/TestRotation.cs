﻿using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : PunBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine) {
            transform.Rotate(Vector3.forward * Time.deltaTime * 50);
        }
        
	}
}
