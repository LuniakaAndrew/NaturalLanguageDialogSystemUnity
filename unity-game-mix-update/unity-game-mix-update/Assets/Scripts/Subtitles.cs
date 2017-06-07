using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Subtitles : MonoBehaviour {
    AudioSource audio;
    public static Subtitles instance;
    long index = 0;
    float nextDialog = 0;
    bool isDialogStarted = false;
    TimeStampCall[] timeStamps;

    void Start()
    {
        if (instance == null)
            instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void StartDialog(string chapter, TimeStampCall[] ts) {
        Debug.Log("StartDialog(string chapter, TimeStampCall[] ts)");
        InOutText.iotext.StartDialog(chapter);
        timeStamps = ts;
        isDialogStarted = true;
        index = 0;
        nextDialog = Time.time;
    }

    void FixedUpdate()
    {
        Debug.Log("outer Time.time " + Time.time);
        Debug.Log("Time.time > nextDialog " + (Time.time > nextDialog));
        if (timeStamps.Length != 0 && (Time.time > nextDialog) && index < timeStamps.Length){
            TimeStampCall t = timeStamps[index];
            nextDialog = Time.time + t.length;
            Debug.Log("Time.time " + Time.time);
            Debug.Log("nextDialog " + nextDialog);
            //audio.clip = t.audioClip;
            //audio.Play();
            for (int j = 0; j < InOutText.iotext.charactes.Count; j++)
            {
                if (InOutText.iotext.charactes[j].Name.Equals(t.CharacterName))
                {
                    string text = InOutText.iotext.charactes[j].getSentence(index);
                    if (!text.Equals("Повторите непонятно")) {
                        InOutText.iotext.output.text = text;
                        Debug.Log(text);
                    }
                }
            }
            index++;
        } else {
            isDialogStarted = false;
        }
    }
}
