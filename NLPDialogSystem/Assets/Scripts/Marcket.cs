using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.UI;

public class Marcket : MonoBehaviour {

    public static bool EnterMarcket;
    public static bool ExitMarcket;
    public static Marcket marcket;
    public Text ClickE;
        
    // Use this for initialization
    void Start () {
        if (marcket == null)
            marcket = this;

        ClickE.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        EnterMarcket = true;
        ExitMarcket = false;
        ClickE.enabled = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        ExitMarcket = true;
        EnterMarcket = false;
        ClickE.enabled = false;
    }

}
