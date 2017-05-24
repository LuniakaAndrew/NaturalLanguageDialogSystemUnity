using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

    bool onetime;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!InOutText.iotext.enter && onetime)
        {

            for (int i = 0; i < gameObject.transform.childCount; i++)
                gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = -5;

            onetime = false;
        }
        if (InOutText.iotext.enter && !onetime)
        {

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 5;
            }

            onetime = true;
        }
    }
}
