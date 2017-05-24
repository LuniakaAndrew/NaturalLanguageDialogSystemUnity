using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string[] input = { "музея", "музею", "музея", "музеем", "музее", "музет" };
        string[] example = { "музей", "музей", "музей", "музей", "музей", "музей" };
        for (int i = 0; i < input.Length; i++)
        {
            Padej2(input[i], example[i]);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Padej(string input, string example)
    {
        string inputlastchar=input.Substring(input.Length-1,1);
        string inputword= input.Substring(0, input.Length-1);

        string examplelastchar = input.Substring(input.Length - 1, 1);
        string exampleword = input.Substring(0, input.Length - 1);

        if (inputlastchar == "й")
        {
            inputlastchar = input.Substring(input.Length - 2, 2);
            inputword = input.Substring(0, input.Length - 2);
        }
        if (inputlastchar == "м")
        {
            inputlastchar = input.Substring(input.Length - 2, 2);
            inputword = input.Substring(0, input.Length - 2);
        }

        if ((inputword + "а") == example || (inputword + "я") == example)
        {
            Debug.Log("Same 1");
        }

        if ((inputword + "о") == example || (inputword + "е") == example || (inputword + "ь") == example || inputword == example)
        {
            Debug.Log("Same 2");
        }

        Debug.Log(inputlastchar+" "+ inputword);

        if (inputlastchar == "а" || inputlastchar == "я")
        {
            Debug.Log("Imenitelnii");
        }
        if (inputlastchar == "ы" || inputlastchar == "и")
        {
            Debug.Log("Roditelnii");
        }
        if (inputlastchar == "е" || inputlastchar == "и")
        {
            Debug.Log("Datelnii");
        }
        if (inputlastchar == "у" || inputlastchar == "ю")
        {
            Debug.Log("Vinitelnii");
        }
        if (inputlastchar == "ой" || inputlastchar == "ей")
        {
            Debug.Log("Tvoritelnii");
        }
        if (inputlastchar == "е" || inputlastchar == "и")
        {
            Debug.Log("Predlojnii");
        }
        ///return true;
    }

    public void Padej2(string input, string example)
    {
        string inputlastchar = input.Substring(input.Length - 1, 1);
        string inputword = input.Substring(0, input.Length - 1);

        string examplelastchar = example.Substring(example.Length - 1, 1);
        string exampleword = example.Substring(0, example.Length - 1);

        if (inputlastchar == "й")
        {
            inputlastchar = input.Substring(input.Length - 2, 2);
            inputword = input.Substring(0, input.Length - 2);
        }
        if (inputlastchar == "м")
        {
            inputlastchar = input.Substring(input.Length - 2, 2);
            inputword = input.Substring(0, input.Length - 2);
        }

        if (inputword == exampleword)
        {
            Debug.Log("Same"+ inputword +" "+ exampleword);
        }
        else
        {
            Debug.Log("Not Same"+inputword +" "+exampleword);
        }
       /* if (examplelastchar == "о" || examplelastchar == "е" || 
            examplelastchar == "а" || examplelastchar == "я" || 
            examplelastchar == "ь")
        {

        }*/
    }
}
