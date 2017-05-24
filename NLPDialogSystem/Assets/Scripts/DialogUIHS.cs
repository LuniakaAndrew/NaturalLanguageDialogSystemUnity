using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUIHS : MonoBehaviour {
    bool onetime;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!InOutText.iotext.enter && onetime)
        {
            gameObject.GetComponent<Canvas>().sortingOrder = -10;
            

            onetime = false;
        }
        if (InOutText.iotext.enter && !onetime)
        {
            gameObject.GetComponent<Canvas>().sortingOrder = 10;
                        onetime = true;
        }
    }
}
