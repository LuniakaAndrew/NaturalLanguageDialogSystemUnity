using UnityEngine;
using System.Collections;

public class Anim : MonoBehaviour {

    Animator animate;
	// Use this for initialization
	void Start () {
        animate = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {

                Debug.Log("Playing anim");
                animate.speed = 1;
                animate.Play("Asteroid");
            
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {

            Debug.Log("Stop anim");
            animate.speed=0;

        }
    }
}
