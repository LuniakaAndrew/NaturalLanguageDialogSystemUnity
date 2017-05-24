using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Xml;


public static class StringExtensions
{
    public static bool Contains(this String str, String substring,
                                StringComparison comp)
    {
        if (substring == null)
            throw new ArgumentNullException("substring",
                                            "substring cannot be null.");
        else if (!Enum.IsDefined(typeof(StringComparison), comp))
            throw new ArgumentException("comp is not a member of StringComparison",
                                        "comp");

        return str.IndexOf(" "+substring+" ", comp) >= 0;
    }
}

public class InOutText : MonoBehaviour {

    InputField input;
    InputField.SubmitEvent se;
    public Text output;
    public Button[] Ansvers;
    Character one = new Character("Торговец");
    Say[] Variants=new Say[0];
    public bool enter=true;
    public static InOutText iotext;
    // Use this for initialization
    void Start () {
        if (iotext == null)
            iotext = this;

        one = new Character(Application.dataPath+"/Resources/base/TradeConversation.xml", "Торговец");
        one.setBase();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        input = gameObject.GetComponent<InputField>();
        se = new InputField.SubmitEvent();
        se.AddListener(SubmitInput);
        input.onEndEdit = se;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        one.setEvent("chapter1");

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.E) && Marcket.marcket.EnterMarcket)
        {
            Marcket.marcket.ShowGO();
            if (Marcket.marcket.EnterMarcket)
            {
                output.text = one.Name + ": " + one.getSentence(0.0f) + "\n";
                ClearAnsvers();
                Variants = one.getDialogVariantsSentence();
                HideAnsvers();
                for (int i = 0; i < Variants.Length; i++)
                {
                    Ansvers[i].GetComponentInChildren<Text>().text = Variants[i].text;
                }
                ShowAnsvers();
                Marcket.marcket.EnterMarcket = false;
            }
            else
            {
                ClearAnsvers();
                output.text = "";
                Marcket.marcket.ExitMarcket = false;
            }
            enter = true;
        }
        else
        {
            if (Marcket.marcket.ExitMarcket)
            {
                ClearAnsvers();
                output.text = "";
                Marcket.marcket.ExitMarcket = false;
            }
            else
            if (one.IsEventEnd())
            {
                enter = false;
                Marcket.marcket.HideGO();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void AnsversButton(int ID)
    {
        ClearAnsvers();
        if (Variants.Length>0)
        {
            try
            {
                one.SetNextReplic(Variants[ID].move);
                output.text = one.Name + ": " + one.getSentence(Variants[ID].move) + "\n";
                ClearAnsvers();
                Variants = one.getDialogVariantsSentence();
                HideAnsvers();
                for (int i = 0; i < Variants.Length; i++)
                {
                    Ansvers[i].GetComponentInChildren<Text>().text = Variants[i].text;
                }
                ShowAnsvers();
            }
            catch
            {
                Debug.Log("Miss Click");
            }
        }
    }
    public void SubmitInput(string arg0)
    {
        if (!arg0.Equals(""))
        {
            //string currentText = output.text;
            string newText =/* currentText+"\n" + */"Игрок: " + arg0;
            output.text = newText + "\n" + one.Name + ": " + one.CompliteAnsver(arg0) + "\n";
            input.text = "";
            input.DeactivateInputField();
            ClearAnsvers();
            Variants = one.getDialogVariantsSentence();
            for (int i = 0; i < Variants.Length; i++)
            {
                Ansvers[i].GetComponentInChildren<Text>().text = Variants[i].text;
            }
        }
    }
    void InputClick()
    {
        input.ActivateInputField();
    }
    public void ClearAnsvers()
    {
        for (int i = 0; i < Ansvers.Length; i++)
            Ansvers[i].GetComponentInChildren<Text>().text = "";
    }
    public void HideAnsvers()
    {
        for (int i = 0; i < Ansvers.Length; i++)
            if(Ansvers[i].GetComponentInChildren<Text>().text=="")
                Ansvers[i].gameObject.SetActive(false);
    }
    public void ShowAnsvers()
    {
        for (int i = 0; i < Ansvers.Length; i++)
            if (Ansvers[i].GetComponentInChildren<Text>().text != "")
                Ansvers[i].gameObject.SetActive(true);
    }

    //не модифицируемая часть изменения отмечать
    public class Character
    {
        string Knowlege;
        public string Name;
        BaseOfKnowledge a;
        Event chapterOne = new Event();
        private float nextreply = -1.0f;
        


        public Character()
        {

        }

        public Character(string Name)
        {
            this.Name = Name;
        }

        public Character(string Knowlege, string Name)
        {
            this.Knowlege = Knowlege;
            this.Name = Name;
            
        }

        public void setBase()
        {
            a = new BaseOfKnowledge(Knowlege);
        }

        public void setEvent(string nameOfEvent)
        {
            chapterOne = a.getEvents(nameOfEvent);
            nextreply = 1.0f;///modified
        }
        ///modified
        public bool IsEventEnd()
        {
            bool eventEnd;
            if (nextreply < 0)
            {
                eventEnd = true;
            }
            else
            {
                eventEnd = false;
            }

            return eventEnd;
        }

        public string getEventName(string nameOfEvent)
        {
            return chapterOne.Name;
        }

        public float getEventID(string nameOfEvent)
        {
            return chapterOne.id;
        }

        public Say setSentence(string text)
        {
            LexicalParser lparse = new LexicalParser(text, a);
            TextParser parsetext = new TextParser(lparse.getTokens(), a, chapterOne.id);

            Say rezult = new Say();
            rezult.text = "";
            List<Say> playersay = chapterOne.getActorSayFromType("игрок", nextreply);

            for (int i = 0; i < playersay.Count; i++)
            {
                /*if (text.ToLower().Equals("помощь"))
                {
                    rezult.text+=a.getHelp();
                }*/
                if (playersay[i].text.Equals("positive") && a.getAnsverType(parsetext.getSubject()).Equals("positive"))
                {
                    nextreply = playersay[i].move;
                }
                if (playersay[i].text.Equals("negative") && a.getAnsverType(parsetext.getSubject()).Equals("negative"))
                {
                    nextreply = playersay[i].move;
                }
                if (playersay[i].text.Equals("hello") && a.getHello(parsetext.getSubject()).Equals("hello"))
                {
                    nextreply = playersay[i].move;
                }
                if (playersay[i].text.Equals("bye") && a.getBye(parsetext.getSubject()).Equals("bye"))
                {
                    nextreply = playersay[i].move;
                    Debug.Log("Bye");
                }
                if (playersay[i].text.Equals("question") && parsetext.getSentenceType().Equals("question"))
                {
                    rezult.text += parsetext.TextParserQuestion(parsetext.getSubject(), parsetext.getVerb(), parsetext.getObject());
                    nextreply = playersay[i].move;
                }
                if (playersay[i].text.Equals("action") && parsetext.getSentenceType().Equals("action"))
                {
                    rezult.text += parsetext.TextParserAction(parsetext.getSubject(), parsetext.getVerb(), parsetext.getObject());
                    nextreply = playersay[i].move;
                }
                if (playersay[i].text.Equals("story") && parsetext.getSentenceType().Equals("story"))
                {
                    rezult.text += parsetext.TextParserAction(parsetext.getSubject(), parsetext.getVerb(), parsetext.getObject());
                    nextreply = playersay[i].move;
                }
            }
            rezult.move = nextreply;
            return rezult;
        }

        public Say[] getDialogVariantsSentence()
        {
            List<Say> playersay = new List<Say>();
            playersay = chapterOne.getActorSayFromType("игрок", nextreply);
            List<Say> temp = new List<Say>();

            for (int i = 0; i < playersay.Count; i++)
            {
                if (playersay[i].text.Equals("positive") || playersay[i].text.Equals("negative") || playersay[i].text.Equals("hello") ||
                    playersay[i].text.Equals("bye") || playersay[i].text.Equals("question") || playersay[i].text.Equals("action"))
                {
                  //  temp.Add(new Say());///////////////////modife
                }
                else
                    temp.Add(playersay[i]);
            }
            Say[] rezult = temp.ToArray();
            return rezult;
        }

        public Say[] getDialogVariantsSentence(float replicid)
        {
            List<Say> playersay = chapterOne.getActorSayFromType("игрок", replicid);
            //string[] replicsmas = replics.Split(new Char[] { '*', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Say[] rezult = new Say[playersay.Count];
            for (int i = 0; i < playersay.Count; i++)
            {
                if (!playersay[i].Equals("positive") || !playersay[i].Equals("negative") || !playersay[i].Equals("hello") ||
                    !playersay[i].Equals("bye") || !playersay[i].Equals("question") || !playersay[i].Equals("action"))
                {
                    rezult[i].text = playersay[i].text;
                }
            }
            return rezult;
        }

        public void SetNextReplic(float idOfReplic)
        {
            nextreply = idOfReplic;
        }

        public string getSentence(float idOfReplic)
        {
            float left = (float)Math.Truncate(idOfReplic);
            float right = idOfReplic - left;
            StringBuilder actrosay = new StringBuilder();
            if (right > 0.0f)
            {
                actrosay.AppendLine(chapterOne.getActorSay(Name, left, idOfReplic).text);
                idOfReplic = left;
            }
            else
                actrosay = chapterOne.getActorSay(Name, idOfReplic);

            string replics = actrosay.ToString();
            string[] replicsmas = replics.Split(new Char[] { '*', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string t = "";
            if (!replics.Equals("Error ActorSay"))
            {
                for (int i = 0; i < replicsmas.Length; i++)
                {
                    if (!replicsmas[i].Equals("NON"))
                        t += replicsmas[i];
                    else
                        t += "";
                    nextreply = chapterOne.getActorSay(Name, idOfReplic, replicsmas[i]).move;
                }
            }
            else
            {
                t += "Повторите непонятно.";
                nextreply = nextreply;
            }

            return t;
        }

        public string CompliteAnsver(string text)
        {
            string result = "";
            Say OutReplics = setSentence(text);

            if (!OutReplics.text.Equals("NON"))/////modife
                result += OutReplics.text+"\n";

            if(!IsEventEnd()|| OutReplics.move>0)
                result += getSentence(OutReplics.move);

            return result+"\n";
        }
    }

    public class LexicalParser
    {
        BaseOfKnowledge knowlege;
        private string tokens;
        private string textSubject = "";
        private string textObject = "";
        private string textVerb = "";
        private string endOfSentence = "";
        string[] ReservedTokens = new string[] { "positive", "negative", "hello", "bye", "question", "action" };
        string[] SubjectQuestionsTokens = new string[] { "кто", "что", "какой", "какая", "какие", "какое", "чей", "чьё",
                                                         "чья", "чьи", "который", "сколько", "кого", "чего", "кому",
                                                         "чему", "кем", "чем" };
        string[] VerbQuestionsTokens = new string[] { "когда","где","куда","откуда","почему","зачем","как"};

        public LexicalParser()
        {

        }
        /// <summary>
        /// /////modife
        /// </summary>
        public LexicalParser(string inputText, BaseOfKnowledge knowlege)
        {
            this.knowlege = knowlege;
            if(!inputText.Equals(""))
            {   
                /////////////////////
                ///////modified
                if ( inputText.IndexOf(".") > 0 )
                {
                    inputText = inputText.Insert(inputText.IndexOf("."), " ");
                }
                if ( inputText.IndexOf("?") > 0 )
                {
                    inputText = inputText.Insert(inputText.IndexOf("?"), " ");
                }
                if ( inputText.IndexOf("!") > 0 )
                {
                    inputText = inputText.Insert(inputText.IndexOf("!"), " ");
                }
                ///////modified
                /////////////////////
                string[] words = inputText.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    Person Stemp = knowlege.getPerson(words[i]);
                    if (Stemp.Name != null)
                    {
                        textObject += words[i] + " ";
                        words[i] = "";
                    }

                    Item SItemp = knowlege.getItem(words[i]);
                    if (SItemp.Name != null)
                    {
                        textObject += words[i] + " ";
                        words[i] = "";
                    }
                    Action Vtemp = knowlege.getAction(words[i]);
                    if (Vtemp.Name != null)
                    {
                        textVerb += words[i] + " ";
                        words[i] = "";
                    }

                    for (int j = 0; j < SubjectQuestionsTokens.Length; j++)
                    {
                        if (SubjectQuestionsTokens[j].Equals(words[i]))
                        {
                            textSubject += words[i] + " ";
                            words[i] = "";
                        }
                    }

                    for (int j = 0; j < VerbQuestionsTokens.Length; j++)
                    {
                        if (VerbQuestionsTokens[j].Equals(words[i]))
                        {
                            textVerb += words[i] + " ";
                            words[i] = "";
                        }
                    }

                    if (words[i].Equals(".") || words[i].Equals("!") || words[i].Equals("?"))
                    {
                        endOfSentence = words[i];
                        words[i] = "";
                    }
                    else
                    {
                        endOfSentence = ".";
                    }
                    textSubject += words[i] + " ";
                  }



                if (textSubject.Length == 0)
                {
                    textSubject += "*";
                }
                if (textVerb.Length == 0)
                {
                    textVerb += "*";
                }
                if (textObject.Length == 0)
                {
                    textObject += "*";
                }

                tokens = "[" + textSubject + "]" + "[" + textVerb + "]" + "[" + textObject + "]" + endOfSentence;
            }
            else
                tokens = "[" + textSubject + "]" + "[" + textVerb + "]" + "[" + textObject + "]" + endOfSentence;

        }

        public string getTokens()
        {
            return tokens;
        }
    }

    public class TextParser
    {
        string textSubject = "s";
        string textObject = "o";
        string textVerb = "v";
        string endOfSentence = "e";
        string AnsverData = "";
        BaseOfKnowledge knowlege;
        Person liveobject = new Person();
        Item inanimateobject = new Item();
        Person livesubject = new Person();
        Item inanimatesubject = new Item();
        Action whatDosubject = new Action();
        float condition;

        public TextParser()
        {

        }

        public TextParser(string inputText)
        {
            if (!inputText.Equals(""))
            {
                string[] SVO = inputText.Split(new Char[] { '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (SVO.Length > 0)
                {
                    textSubject = SVO[0];
                    textVerb = SVO[1];
                    textObject = SVO[2];
                    endOfSentence = SVO[3];
                    if (endOfSentence.Length > 1)
                    {
                        endOfSentence = endOfSentence[0].ToString();
                    }
                }
            }
        }
        ///////modife
        public TextParser(string inputText, BaseOfKnowledge knowlege)
        {
            this.knowlege = knowlege;
            if (!inputText.Equals(""))
            {
                string[] SVO = inputText.Split(new Char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (SVO.Length > 0)
                {
                    textSubject = SVO[0];
                    textVerb = SVO[1];
                    textObject = SVO[2];
                    endOfSentence = SVO[3];
                    if (endOfSentence.Length > 1)
                    {
                        endOfSentence = endOfSentence[0].ToString();
                    }
                }
            }
        }

        public TextParser(string inputText, BaseOfKnowledge knowlege, float condition)
        {
            this.condition=condition;
            this.knowlege = knowlege;
            if (!inputText.Equals(""))
            {
                string[] SVO = inputText.Split(new Char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (SVO.Length > 0)
                {
                    textSubject = SVO[0];
                    textVerb = SVO[1];
                    textObject = SVO[2];
                    endOfSentence = SVO[3];
                    if (endOfSentence.Length > 1)
                    {
                        endOfSentence = endOfSentence[0].ToString();
                    }
                }
            }
        }

        public string getSentenceType()
        {
            string ansver = "";
            switch (endOfSentence)
            {
                case ".":
                    ansver = "story";
                    break;
                case "?":
                    ansver = "question";
                    break;
                case "!":
                    ansver = "action";
                    break;
            }
            return ansver;
        }

        public string TextParserQuestion(string SubjectParse, string VerbParse, string ObjectParse)
        {
            string Result = "";
            string[] Owords = ObjectParse.Split(new Char[] { ' ' });
            string[] Swords = SubjectParse.ToLower().Split(new Char[] { ' ' });
            string[] Vwords = VerbParse.Split(new Char[] { ' ' });

            for (int i = 0; i < Owords.Length; i++)
            {
                Person Stemp = knowlege.getPerson(Owords[i]);
                Item SItemp = knowlege.getItem(Owords[i]);
                if (Stemp.Name != null)
                {
                    liveobject = Stemp;
                }
                else
                if (SItemp.Name != null)
                {
                    inanimateobject = SItemp;
                }
            }

            for (int i = 0; i < Swords.Length; i++)
            {
                Person Stemp = knowlege.getPerson(Swords[i]);
                Item SItemp = knowlege.getItem(Swords[i]);
                if (Stemp.Name != null)
                {
                    livesubject = Stemp;
                }
                else
                if (SItemp.Name != null)
                {
                    inanimatesubject = SItemp;
                }
            }

            for (int i = 0; i < Vwords.Length; i++)
            {
                Action Vtemp = knowlege.getAction(Vwords[i]);
                if (Vtemp.Name != null)
                {
                    whatDosubject = Vtemp;
                }
            }

            for (int i = 0; i < Swords.Length; i++)
            {
                if (Swords[i].Equals(""))
                {
                    Result = (!liveobject.IsEmpty()) ? "Есть.":"Нет.";
                    Result = (!inanimateobject.IsEmpty())? "Есть." : "Нет.";
                }
                if (Swords[i].Equals("кто"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("что"))
                {
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("какой") || Swords[i].Equals("какая") || Swords[i].Equals("какие") || Swords[i].Equals("какое"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("чей") || Swords[i].Equals("чьё") || Swords[i].Equals("чья") || Swords[i].Equals("чьи"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("который"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("сколько"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кого"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чего"))
                {
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кому"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чему"))
                {
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кем"))
                {
                    if (liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чем"))
                {
                    if (inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
            }

            for (int i = 0; i < Vwords.Length; i++)
            {
                if (whatDosubject != null)
                {
                    if (Vwords[i].Equals("где"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("когда"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("куда"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("откуда"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("почему"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("зачем"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("как"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                }
            }
            if (Result.Equals(""))
                Result = "Ничего не знаю об этом.";
            return Result;
        }

        public void TextParserStory(string SubjectParse, string VerbParse, string ObjectParse)
        {

        }

        public string TextParserAction(string SubjectParse, string VerbParse, string ObjectParse)
        {
            string[] Owords = ObjectParse.Split(new Char[] { ' ' });
            string[] Swords = SubjectParse.ToLower().Split(new Char[] { ' ' });
            string[] Vwords = VerbParse.Split(new Char[] { ' ' });
            string result="";
            for (int i = 0; i < Owords.Length; i++)
            {
                Person Stemp = knowlege.getPerson(Owords[i]);
                Item SItemp = knowlege.getItem(Owords[i]);
                if (Stemp.Name != null)
                {
                    liveobject = Stemp;
                }
                else
                if (SItemp.Name != null)
                {
                    inanimateobject = SItemp;
                }
            }
            for (int i = 0; i < Vwords.Length; i++)
            {
                Action Vtemp = knowlege.getAction(Vwords[i]);
                if (Vtemp.Name != null)
                {
                    whatDosubject = Vtemp;
                }
            }
            if (liveobject.IsEmpty() && whatDosubject.IsCanIDoThisWithPerson(liveobject.id))
            {
                result += liveobject.Name + " ";
            }
            if (inanimateobject.IsEmpty() && whatDosubject.IsCanIDoThisWithItem(inanimateobject.id))
            {
                result += inanimateobject.Name + " ";
            }
            return "action:"+whatDosubject.DoAction()+ ":" + result;
        }

        public string getSubject()
        {
            return this.textSubject.Trim();
        }

        public string getVerb()
        {
            return this.textVerb.Trim();
        }

        public string getObject()
        {
            return this.textObject.Trim();
        }

    }

    public class BaseOfKnowledge
    {
        XmlReader reader;
        List<Person> Persons = new List<Person>();
        List<Item> Items = new List<Item>();
        List<Action> Actions = new List<Action>();
        List<Location> Locations = new List<Location>();
        List<Hello> Greetings = new List<Hello>();
        List<Bye> GoodBye = new List<Bye>();
        List<Ansver> Ansvers = new List<Ansver>();
        List<Event> Events = new List<Event>();

        public BaseOfKnowledge(string PersonName)
        {

            LoadKnowlege(PersonName);
        }

        public void LoadKnowlege(string PersonName)
        {
            try
            {
                reader = new XmlTextReader(PersonName);
   
                if (reader.IsEmptyElement)
                {
                    Debug.Log("Load Error Empty");
                    
                }

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if (reader.Name == "Hello")
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    Hello temp = new Hello();
                                    temp.Phrase = reader.Value;
                                    Greetings.Add(temp);
                                }
                            }

                            if (reader.Name == "Bye")
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    Bye temp = new Bye();
                                    temp.Phrase = reader.Value;
                                    GoodBye.Add(temp);
                                }
                            }

                            if (reader.Name == "Ansver")
                            {
                                Ansver temp = new Ansver();
                                reader.MoveToAttribute("type");
                                temp.type = reader.Value;
                                reader.MoveToAttribute("phrase");
                                temp.Pharse = reader.Value;
                                Ansvers.Add(temp);
                            }

                            if (reader.Name == "Person")
                            {
                                Person temp = new Person();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("Name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("Appearance");
                                temp.Appearance = reader.Value;
                                reader.MoveToAttribute("Mood");
                                temp.Mood = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                Persons.Add(temp);
                            }

                            if (reader.Name == "QuestionPerson")
                            {
                                Question temp = new Question();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("personreferences");
                                temp.AddPersonRef(float.Parse(reader.Value, CultureInfo.InvariantCulture));
                                reader.MoveToAttribute("type");
                                temp.type = reader.Value;
                                reader.MoveToAttribute("text");
                                temp.text = reader.Value;
                                Persons[Persons.Count - 1].AddQuestionPerson(temp);
                            }

                            if (reader.Name == "Item")
                            {
                                Item temp = new Item();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("Name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("Appearance");
                                temp.Appearance = reader.Value;
                                Items.Add(temp);
                            }

                            if (reader.Name == "QuestionItem")
                            {
                                Question temp = new Question();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("personreferences");
                                temp.AddPersonRef(float.Parse(reader.Value, CultureInfo.InvariantCulture));
                                reader.MoveToAttribute("type");
                                temp.type = reader.Value;
                                reader.MoveToAttribute("text");
                                temp.text = reader.Value;
                                Items[Items.Count - 1].AddQuestionItem(temp);
                            }

                            if (reader.Name == "Action")
                            {

                                Action temp = new Action();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("Name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("Appearance");
                                temp.Appearance = reader.Value;
                                reader.MoveToAttribute("personreferences");
                                temp.AddPersonRef(reader.Value);
                                reader.MoveToAttribute("itemreferences");
                                temp.AddItemRef(reader.Value);
                                Actions.Add(temp);
                            }

                            if (reader.Name == "QuestionAction")
                            {
                                Question temp = new Question();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("personreferences");
                                temp.AddPersonRef(float.Parse(reader.Value, CultureInfo.InvariantCulture));
                                reader.MoveToAttribute("type");
                                temp.type = reader.Value;
                                reader.MoveToAttribute("text");
                                temp.text = reader.Value;
                                Actions[Actions.Count - 1].AddQuestionAction(temp);
                            }

                            if (reader.Name == "Location")
                            {
                                
                                Location temp = new Location();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("Name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("Appearance");
                                temp.Appearance = reader.Value;
                                reader.MoveToAttribute("positionX");
                                temp.positionX = int.Parse(reader.Value);
                                reader.MoveToAttribute("positionY");
                                temp.positionY = int.Parse(reader.Value);
                                Locations.Add(temp);

                            }

                            if (reader.Name == "Event")
                            {
                                Event temp = new Event();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("name");
                                temp.Name = reader.Value;
                                Events.Add(temp);
                            }

                            if (reader.Name == "Actor")
                            {
                                Actor temp = new Actor();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("type");
                                temp.Type = reader.Value;
                                Events[Events.Count - 1].AddActor(temp);
                            }

                            if (reader.Name == "say")
                            {
                                Say temp = new Say();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("move");
                                temp.move = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                Events[Events.Count - 1].setActorSay(temp);
                            }
                            break;

                        case XmlNodeType.Text:
                            Events[Events.Count - 1].setActorSayText(reader.Value);
                            break;
                    }
                }
            }
            catch
            {
                Debug.Log("Load Error");
            }
        }

        public string getHelloPhrase(string text)
        {
            Hello temp = Greetings.Find(delegate (Hello bk) { return bk.Phrase == text; });
            if (temp != null)
            {
                return temp.Phrase;
            }
            else
            {
                return "Not Match";
            }
        }

        public string getHello(string text)
        {
            Hello temp = Greetings.Find(delegate (Hello bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Phrase.Contains(text, comp);
            });
            if (temp != null)
            {
                return "hello";
            }
            else
            {
                return "Not Match";
            }
        }

        public string getByePhrase(string text)
        {
            Bye temp = GoodBye.Find(delegate (Bye bk) { return bk.Phrase == text; });
            if (temp != null)
            {
                return temp.Phrase;
            }
            else
            {
                return "Not Match";
            }
        }

        public string getBye(string text)
        {
            Bye temp = GoodBye.Find(delegate (Bye bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Phrase.Contains(text, comp);
            });
            if (temp != null)
            {
                return "bye";
            }
            else
            {
                return "Not Match";
            }
        }

        public string getAnsver(string text)
        {
            Ansver temp = Ansvers.Find(delegate (Ansver bk) { return bk.Pharse == text; });
            if (temp != null)
            {
                return temp.Pharse;
            }
            else
            {
                return "Not Match";
            }
        }

        public string getAnsverType(string text)
        {
            Ansver temp = Ansvers.Find(delegate (Ansver bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Pharse.Contains(text, comp);
            });
            if (temp != null)
            {
                return temp.type;
            }
            else
            {
                return "Not Match";
            }
        }

        public Person getPerson(string name)
        {
            Person temp = Persons.Find(delegate (Person bk) { return bk.Name == name; });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Person();
            }
        }

        public Item getItem(string name)
        {
            Item temp = Items.Find(delegate (Item bk) { return bk.Name == name; });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Item();
            }
        }

        public Action getAction(string name)
        {
            Action temp = Actions.Find(delegate (Action bk) { return bk.Name == name; });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Action();
            }
        }

        public Event getEvents(string name)
        {
            Event temp = Events.Find(delegate (Event bk) { return bk.Name.Equals(name); });
            if (temp != null)
            {
                return temp;
            }

            else
            {
                return new Event("Error");
            }

        }

        public string getHelp()
        {
            string result = "Вводимые предложения могут быть трёх типов:\n"+
                            "Вопросительные: обязаны заканчиватся знаком \"?\"\n"+
                            "Действия: обязаны заканчиватся знаком \"!\"\n"+
                            "Обычное: обязаны заканчиватся знаком \".\"\n";

            return result;
        }

    }

    public class Hello
    {
        public string Phrase;
    }

    public class Bye
    {
        public string Phrase;
    }

    public class Ansver
    {
        public string type;
        public string Pharse;
    }

    public class Item
    {
        public float id;
        public string Name;
        public string Appearance;
        List<Question> listQuestionItem = new List<Question>();

        public  Item()
        {

        }

        public void AddQuestionItem(Question question)
        {
            listQuestionItem.Add(question);
        }

        public string getQuestionItemType(string text)
        {
            Question temp = listQuestionItem.Find(delegate (Question bk) { return bk.type == text; });
            if (temp != null)
            {
                return temp.type;
            }
            else
            {
                return "Error ItemType";
            }
        }

        public string getQuestionItemText(string text)
        {
            Question temp = listQuestionItem.Find(delegate (Question bk) { return bk.type == text; });
            if (temp != null)
            {
                return temp.text;
            }
            else
            {
                return "Error ItemText";
            }
        }

        public float getQuestionItemId(float idp)
        {
            Question temp = listQuestionItem.Find(delegate (Question bk) { return bk.id == idp; });
            if (temp != null)
            {
                return temp.id;
            }
            else
            {
                return -1;
            }
        }

        public bool IsEmpty()
        {
            Item Empty = new Item();
            return Empty.Equals(this); ;
        }

    }

    public class Location
    {
        public float id;
        public string Name;
        public string Appearance;
        public int positionX;
        public int positionY;
    }

    public class Action
    {
        public float id;
        public string Name;
        public string Appearance;
        private string personreferences;
        private string itemreferences;
        List<Question> listQuestionAction = new List<Question>();

        public string DoAction()
        {
            return Name;
        }

        public bool IsCanIDoThisWithItem(float refid)
        {
            bool result = false;
            string[] temp = itemreferences.Split(new Char[] { ' ','/' });
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Equals(refid.ToString()))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool IsCanIDoThisWithPerson(float refid)
        {
            bool result = false;
            string[] temp = personreferences.Split(new Char[] { ' ', '/' });
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Equals(refid.ToString()))
                {
                    result = true;
                }
            }
            return result;
        }

        public void AddItemRef(string itemreferences)
        {
            this.itemreferences = itemreferences;
        }

        public void AddPersonRef(string personreferences)
        {
            this.personreferences = personreferences;
        }

        public void AddQuestionAction(Question question)
        {
            listQuestionAction.Add(question);
        }

        public string getQuestionActionType(string type)
        {
            Question temp = listQuestionAction.Find(delegate (Question bk) { return bk.type == type; });
            if (temp != null)
            {
                return temp.type;
            }
            else
            {
                return "Error ActionType";
            }
        }

        public string getQuestionActionText(string text)
        {
            Question temp = listQuestionAction.Find(delegate (Question bk) { return bk.type == text; });
            if (temp != null)
            {
                return temp.text;
            }
            else
            {
                return "Error ActionText";
            }
        }

        public string getQuestionActionText(string text, float id)
        {
            Question temp = listQuestionAction.Find(delegate (Question bk) { return bk.type == text && bk.id == id; });
            if (temp != null)
            {
                return temp.text;
            }
            else
            {
                return "Error ActionText";
            }
        }

        public float getQuestionActionId(float idp)
        {
            Question temp = listQuestionAction.Find(delegate (Question bk) { return bk.id == idp; });
            if (temp != null)
            {
                return temp.id;
            }
            else
            {
                return -1;
            }
        }

    }

    public class Question
    {
        public float id;
        private float personreferences;
        public string type;
        public string text;

        public void AddPersonRef(float personreferences)
        {
            this.personreferences = personreferences;
        }

        public float getPersonRef()
        {
            return personreferences;
        }
    }

    public class Person
    {
        public float id;
        public string Name;
        public string Appearance;
        public float Mood;
        List<Question> listQuestionPerson = new List<Question>();

        public void AddQuestionPerson(Question question)
        {
            listQuestionPerson.Add(question);
        }

        public string getQuestionPersonType(string text)
        {
            Question temp = listQuestionPerson.Find(delegate (Question bk) { return bk.type == text; });
            if (temp != null)
            {
                return temp.type;
            }
            else
            {
                return "Error PersonType";
            }
        }

        public string getQuestionPersonText(string text)
        {
            Question temp = listQuestionPerson.Find(delegate (Question bk) { return bk.type == text; });
            if (temp != null)
            {
                return temp.text;
            }
            else
            {
                return "Error PersonText";
            }
        }

        public float getQuestionPersonId(float idp)
        {
            Question temp = listQuestionPerson.Find(delegate (Question bk) { return bk.id == idp; });
            if (temp != null)
            {
                return temp.id;
            }
            else
            {
                return -1;
            }
        }

        public void setMood(float Mood)
        {
            this.Mood = Mood;
        }

        public bool IsEmpty()
        {
            return new Person().Equals(this);
        }

    }
    /// <summary>
    /// Код относиться к обработке блока событий 
    /// </summary>
    public class Say
    {
        public float id;
        public float move;
        public string text;

        public Say()
        {
            this.text = "NON";///////////////////modife
        }

        public Say(float id, int move)
        {
            this.id = id;
            this.move = move;
        }

        public Say(float id, int move, string text)
        {
            this.id = id;
            this.move = move;
            this.text = text;
        }

        public void setText(string text)
        {
            this.text = text;
        }
        /////////////////modife
        public void Clear()
        {
            this.id = 0;
            this.move = 0;
            this.text = "";
        }

    }

    public class Actor
    {
        public float id;
        public string Name;
        public string Type;
        List<Say> listSay = new List<Say>();
        public Actor()
        {

        }

        public Actor(int id, string Name, string Type)
        {
            this.id = id;
            this.Name = Name;
            this.Type = Type;
        }

        public void AddSay(Say say)
        {
            listSay.Add(say);
        }

        public void setSayText(string text)
        {
            listSay[listSay.Count - 1].setText(text);
        }

        public Say getSay(float id)
        {
            Say temp = listSay.Find(delegate (Say bk) { return bk.id == id; });
            if (temp != null)
                return temp;
            else
                return new Say();
        }

        public Say getSay(string text)
        {
            Say temp = listSay.Find(delegate (Say bk) { return bk.text == text; });
            if (temp != null)
                return temp;
            else
                return new Say();
        }

        public StringBuilder getAllSay()
        {
            StringBuilder line = new StringBuilder();
            foreach (Say temp in listSay)
            {
                if (temp.text != "" || temp.text != "NON")//////////modife
                {
                    line.AppendLine(temp.text + "*");
                }
                else
                    line.AppendLine("Not Found");
            }
            return line;
        }

        public List<Say> getAllListSay()
        {
            return listSay;
        }
    }

    public class Event
    {
        public float id;
        public string Name;

        List<Actor> listActor = new List<Actor>();

        public Event()
        {

        }

        public Event(string Name)
        {
            this.Name = Name;
        }

        public void AddActor(Actor actor)
        {
            listActor.Add(actor);
        }

        public void setActorSay(Say say)
        {
            listActor[listActor.Count - 1].AddSay(say);
        }

        public void setActorSayText(string text)
        {
            listActor[listActor.Count - 1].setSayText(text);
        }

        public string getActor(string name)
        {
            Actor temp = listActor.Find(delegate (Actor bk) { return bk.Name == name; });
            if (temp.Name != "")
                return temp.Name;
            else
                return "Not found";
        }

        public StringBuilder getActorSay(string name, float id)
        {
            Actor temp = listActor.Find(delegate (Actor bk) { return bk.Name.Equals(name) && bk.id == id; });

            if (temp != null)
            {
                return temp.getAllSay();
            }

            else
            {
                return new StringBuilder("Error ActorSay");
            }

        }

        public Say getActorSay(string name, float id, float idSay)
        {
            Actor temp = listActor.Find(delegate (Actor bk) { return bk.Name == name && bk.id == id; });
            if (temp != null)
                return temp.getSay(idSay);
            else
            {
                Say t = new Say();
                t.setText("Error ActorSay");
                return t;
            }

        }

        public Say getActorSay(string name, float id, string text)
        {
            Actor temp = listActor.Find(delegate (Actor bk) { return bk.Name == name && bk.id == id; });
            if (temp != null)
                return temp.getSay(text);
            else
            {
                ///modifie
                  return new Say();
            }
        }

        public List<Say> getActorSayFromType(string type, float id)
        {
            Actor temp = listActor.Find(delegate (Actor bk) { return bk.Type == type && bk.id == id; });
            if (temp != null)
                return temp.getAllListSay();
            else
                return new List<Say>();
        }

        public Say getActorSayFromType(string type, float id, float idSay)
        {
            Actor temp = listActor.Find(delegate (Actor bk) { return bk.Type == type && bk.id == id; });
            if (temp != null)
                return temp.getSay(idSay);
            else
                return new Say();
        }

    }
}
