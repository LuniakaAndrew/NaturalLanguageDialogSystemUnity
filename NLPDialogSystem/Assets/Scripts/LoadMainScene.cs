using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMainScene : MonoBehaviour {

    public Text output;
    public Text output2;
    bool step = true;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (step)
            {
                output.text = "";
                output2.text = "";
                output.fontSize = 24;
                output.text = "Розробленний приклад системи імітації та підтримки діалогу\n"+
                    "надає можливіть самостійно вводити відповіді на питання, задавати питання та віддавати команди за допомогою природно-мовного інтерфейсу.\n"+
                    "Для відповіді на питання використовуйте звичайне речення яке закінчується \".\".\n"+
                    "Для того щоб задати питання закінчить речення знаком \"?\".\n"+
                    "Для відання наказу закінчить речення знаком \"!\".\n"+
                    "Наприклад:\n"+
                    "1) На запитання\"Хочеш яблуко?\"\n" +
                    "Можна відповісти \"так.\" або \"ні.\" чи фразою, що схожа на позитивну або негативну відповідь.\n" +
                    "2) Можна запитати про щось або когось.\n" +
                    "\"Що таке яблуко?\" або \"Хто такий Тарас?\"\n" +
                    "3) Можна \"віддати наказ\".\n" +
                    "\"Будь ласка дайте яблуко!\"\n" +
                    "Усі відповіді, питання та \"накази\" залежать тільки від людини, яка створила діалог.\n" +
                    "Кінець.";
                step = false;
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("DialogV2");
            }
            
        }
    }
}
