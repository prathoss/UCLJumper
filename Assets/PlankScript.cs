using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankScript : MonoBehaviour {

    
    private string ObjectName;
    
    private GameObject Player;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
        Debug.Log(ObjectName);
	}

    private void OnCollisionEnter(Collision collision)
    {
        ObjectName = collision.gameObject.name;
        
    }
}
