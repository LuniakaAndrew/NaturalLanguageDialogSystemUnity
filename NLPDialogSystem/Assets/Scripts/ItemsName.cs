using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsName : MonoBehaviour {

    public Text ItemNameOut;
    public string NameItem;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseEnter()
    {
        ItemNameOut.text = NameItem;
    }
}
