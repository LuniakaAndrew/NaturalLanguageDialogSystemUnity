using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBar : MonoBehaviour {

    GameObject[,] grid = new GameObject[22, 22];
    public GameObject block;
    public Sprite[] sprites;
    public float sizeWH;
    // Use this for initialization
    void Start () {
        float ypos = gameObject.transform.position.x;
        float xpos = gameObject.transform.position.y;
        //float xpos = 2;
        //float ypos = -6;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                grid[i, j] = Instantiate(block, new Vector2((i*sizeWH)+ypos, -1 * (j*sizeWH)+xpos), Quaternion.identity) as GameObject;
                grid[i, j].name = "Item"+(i*8+j);
                grid[i, j].transform.parent = transform;
                grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[j * 8 + i];
            }
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
