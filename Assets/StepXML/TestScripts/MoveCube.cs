using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : PunBehaviour
{
    bool isActive = true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isActive = !isActive;
        }


        if (!isActive)
            return;

        if(photonView.isMine)
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,5));

        
    }



 


}
