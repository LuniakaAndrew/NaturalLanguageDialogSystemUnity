using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HouseBehaviour : MonoBehaviour {

	public int state;
	public string activeChapterName;
	public string pastChapterName;
	public string futureChapterName;
	public TimeStampCall[] timeStamps;
	public TimeStampCall[] pastTimeStamps;
	public TimeStampCall[] futureTimeStamps;
	public HouseBehaviour nextObject;

	bool isTriggerEntered = false;


	void Start () {
	}
	
	void Update () {
		if(isTriggerEntered && Input.GetKeyDown(KeyCode.E))
  		{ 
  			Debug.Log("isTriggerEntered && Input.GetKeyDown(KeyCode.E)");
  			switch(state) {
  				case 0: 
  					Subtitles.instance.StartDialog(pastChapterName, pastTimeStamps);
  					break;
  				case 1:
  					Subtitles.instance.StartDialog(activeChapterName, timeStamps);
  					nextObject.state = 1;
  					state = 0;
  					break;
  				case 2:
  					Subtitles.instance.StartDialog(futureChapterName, futureTimeStamps);
  					break;
  			}
    	}	
	}

	void OnTriggerEnter(Collider collider) {
		isTriggerEntered = true;
		Debug.Log("onTriggerEnter" + name + collider.gameObject.name);
	}

	void OnTriggerExit(Collider collider) {
		isTriggerEntered = false;
		Debug.Log("onTriggerExit");
	}
}
