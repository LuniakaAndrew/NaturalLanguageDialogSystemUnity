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

        return str.IndexOf(substring, comp) >= 0;
    }
}

public class InOutText : MonoBehaviour {
       
    public Text output;
    public string BaseKnowledge="";
    public string Lang = "";
    public string[] NameofGamePerson;
    public List<Character> charactes=new List<Character>();
    public static InOutText iotext;
    //float dialogCount = 1.0f;
    
    // Use this for initialization
    void Start () {
        if (iotext == null)
            iotext = this;
        
        //charactes = new Character();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //input = gameObject.GetComponent<InputField>();
        //se = new InputField.SubmitEvent();
        //se.AddListener(SubmitInput);
        //input.onEndEdit = se;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
	
	// Update is called once per frame
	void FixedUpdate () {
             
    }

    void Update()
    {
       /*if (Input.GetKeyDown(KeyCode.E)) {
            StartDialog();
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            dialogCount += 1.0f;
            output.text = one.getSentence(dialogCount) + "\n";
        }
        
        if (one.IsEventEnd())
        {

            

        }*/
    }
    
    public void StartDialog(string currEvent)
    {
        for (int i = 0; i < NameofGamePerson.Length; i++)
        {
            charactes.Add(new Character(Application.dataPath + "/Resources/DialogBase/" + BaseKnowledge, NameofGamePerson[i], Lang));
            charactes[i].setEvent(currEvent);
        }
    }



    //не модифицируемая часть изменения отмечать
   // [Serializable]
    public class Character
    {
        string Knowlege;
        public string Name;
        public string language;
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
            setBase();
        }

        public Character(string Knowlege, string Name,string language)
        {
            this.Knowlege = Knowlege;
            this.Name = Name;
            this.language = language;
            setBase();
        }

        public void setBase()
        {
            a = new BaseOfKnowledge(Knowlege);
        }

        public void setBase(string knowlegefilepath)
        {
            a = new BaseOfKnowledge(knowlegefilepath);
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

        public Say setSentenceRU(string text)
        {
            
            LexicalParserRU lparse = new LexicalParserRU(text, a);
            TextParserRU parsetext = new TextParserRU(lparse.getTokens(), a, chapterOne.id);

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
                    rezult.text +="";
                    parsetext.TextParserStory(parsetext.getSubject(), parsetext.getVerb(), parsetext.getObject());
                    nextreply = playersay[i].move;
                    Debug.Log("Story");
                }
            }
            rezult.move = nextreply;
            return rezult;
        }

        public Say setSentenceUK(string text)
        {

            LexicalParserUK lparse = new LexicalParserUK(text, a);
            TextParserUK parsetext = new TextParserUK(lparse.getTokens(), a, chapterOne.id);

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
                    rezult.text += "";
                    parsetext.TextParserStory(parsetext.getSubject(), parsetext.getVerb(), parsetext.getObject());
                    nextreply = playersay[i].move;
                    Debug.Log("Story");
                }
            }
            rezult.move = nextreply;
            return rezult;
        }

        public Say setSentenceEN(string text)
        {

            LexicalParserEN lparse = new LexicalParserEN(text, a);
            TextParserEN parsetext = new TextParserEN(lparse.getTokens(), a, chapterOne.id);

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
                    rezult.text += "";
                    parsetext.TextParserStory(parsetext.getSubject(), parsetext.getVerb(), parsetext.getObject());
                    nextreply = playersay[i].move;
                    Debug.Log("Story");
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
            Say OutReplics=new Say();
            switch(language)
            {
                case "RU": OutReplics = setSentenceRU(text); break;
                case "UK": OutReplics = setSentenceUK(text); break;
                case "EN": OutReplics = setSentenceEN(text); break;
                default:   OutReplics = setSentenceEN(text); break;
            }


            if (!OutReplics.text.Equals("NON"))/////modife
                result += OutReplics.text+"\n";

            if(!IsEventEnd()|| OutReplics.move>0)
                result += getSentence(OutReplics.move);

            return result+"\n";
        }
    }

    public class LexicalParserRU
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

        public LexicalParserRU()
        {

        }
        /// <summary>
        /// /////modife
        /// </summary>
        public LexicalParserRU(string inputText, BaseOfKnowledge knowlege)
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
                if (inputText.IndexOf("!") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("!"), " ");
                }
                else
                {
                    inputText += " ";
                }
                ///////modified
                /////////////////////
                Person Stemp = knowlege.getPersonFromString(inputText);
                if (Stemp.Name != null)
                {
                    textObject += Stemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }

                Item SItemp = knowlege.getItemFromString(inputText);
                if (SItemp.Name != null)
                {
                    textObject += SItemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }

                Location SLtemp = knowlege.getLocationFromString(inputText);
                if (SLtemp.Name != null)
                {
                    textObject += SLtemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }

                Action Vtemp = knowlege.getActionFromString(inputText);
                if (Vtemp.Name != null)
                {
                    textVerb += Vtemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textVerb), textVerb.Length);
                }
                
                string[] words = inputText.ToLower().Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {

                    for (int j = 0; j < SubjectQuestionsTokens.Length; j++)
                    {
                        if (SubjectQuestionsTokens[j].Equals(words[i]))
                        {
                            textSubject += words[i] + " ";
                            words[i] = "";
                            break;
                        }
                    }

                    for (int j = 0; j < VerbQuestionsTokens.Length; j++)
                    {
                        if (VerbQuestionsTokens[j].Equals(words[i]))
                        {
                            textVerb += words[i] + " ";
                            words[i] = "";
                            break;
                        }
                    }

                    if (knowlege.getHello(words[i]).Equals("hello"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getAnsverType(words[i]).Equals("positive"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getAnsverType(words[i]).Equals("negative"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getBye(words[i]).Equals("bye"))
                    {
                        textSubject += words[i] + " ";
                    }
                    

                    if (words[i].Equals(".") || words[i].Equals("!") || words[i].Equals("?"))
                    {
                        endOfSentence = words[i];
                        words[i] = "";
                        break;
                    }

                    textObject += words[i] + " ";
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
                if (endOfSentence.Length == 0)
                {
                    endOfSentence += ".";
                }
                Debug.Log(textSubject);
                tokens = "[" + textSubject.Trim() + "]" + "[" + textVerb.Trim() + "]" + "[" + textObject.Trim() + "]" + endOfSentence.Trim();
            }
            else
                tokens = "[" + textSubject.Trim() + "]" + "[" + textVerb.Trim() + "]" + "[" + textObject.Trim() + "]" + endOfSentence.Trim();

        }

        public string getTokens()
        {
            return tokens;
        }
    }

    public class LexicalParserUK
    {
        BaseOfKnowledge knowlege;
        private string tokens;
        private string textSubject = "";
        private string textObject = "";
        private string textVerb = "";
        private string endOfSentence = "";
        string[] ReservedTokens = new string[] { "positive", "negative", "hello", "bye", "question", "action" };
        string[] SubjectQuestionsTokens = new string[] { "хто", "що", "який", "яка", "які", "яке", "чий", "чиє",
                                                        "чия", "чиї", "котрий", "скільки", "кого", "чого", "кому", "чому", "ким", "чим" };
        string[] VerbQuestionsTokens = new string[] { "коли", "де", "куди", "звідки", "чому", "навіщо", "як" };

        public LexicalParserUK()
        {

        }
        /// <summary>
        /// /////modife
        /// </summary>
        public LexicalParserUK(string inputText, BaseOfKnowledge knowlege)
        {
            this.knowlege = knowlege;
            if (!inputText.Equals(""))
            {
                /////////////////////
                ///////modified
                if (inputText.IndexOf(".") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("."), " ");
                }
                if (inputText.IndexOf("?") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("?"), " ");
                }
                if (inputText.IndexOf("!") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("!"), " ");
                }
                else
                {
                    inputText += " ";
                }
                ///////modified
                /////////////////////
                Person Stemp = knowlege.getPersonFromString(inputText);
                if (Stemp.Name != null)
                {
                    textObject += Stemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }

                Item SItemp = knowlege.getItemFromString(inputText);
                if (SItemp.Name != null)
                {
                    textObject += SItemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }
                Location SLtemp = knowlege.getLocationFromString(inputText);
                if (SLtemp.Name != null)
                {
                    textObject += SLtemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }
                Action Vtemp = knowlege.getActionFromString(inputText);
                if (Vtemp.Name != null)
                {
                    textVerb += Vtemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textVerb), textVerb.Length);
                }

                string[] words = inputText.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    for (int j = 0; j < SubjectQuestionsTokens.Length; j++)
                    {
                        if (SubjectQuestionsTokens[j].Equals(words[i]))
                        {
                            textSubject += words[i] + " ";
                            words[i] = "";
                            break;
                        }
                    }

                    for (int j = 0; j < VerbQuestionsTokens.Length; j++)
                    {
                        if (VerbQuestionsTokens[j].Equals(words[i]))
                        {
                            textVerb += words[i] + " ";
                            words[i] = "";
                            break;
                        }
                    }

                    if (knowlege.getHello(words[i]).Equals("hello"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getAnsverType(words[i]).Equals("positive"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getAnsverType(words[i]).Equals("negative"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getBye(words[i]).Equals("bye"))
                    {
                        textSubject += words[i] + " ";
                    }


                    if (words[i].Equals(".") || words[i].Equals("!") || words[i].Equals("?"))
                    {
                        endOfSentence = words[i];
                        words[i] = "";
                        break;
                    }

                    textObject += words[i] + " ";
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
                if (endOfSentence.Length == 0)
                {
                    endOfSentence += ".";
                }
                Debug.Log(textSubject);
                tokens = "[" + textSubject.Trim() + "]" + "[" + textVerb.Trim() + "]" + "[" + textObject.Trim() + "]" + endOfSentence.Trim();
            }
            else
                tokens = "[" + textSubject.Trim() + "]" + "[" + textVerb.Trim() + "]" + "[" + textObject.Trim() + "]" + endOfSentence.Trim();

        }

        public string getTokens()
        {
            return tokens;
        }
    }

    public class LexicalParserEN
    {
        BaseOfKnowledge knowlege;
        private string tokens;
        private string textSubject = "";
        private string textObject = "";
        private string textVerb = "";
        private string endOfSentence = "";
        string[] ReservedTokens = new string[] { "positive", "negative", "hello", "bye", "question", "action" };
        string[] SubjectQuestionsTokens = new string[] { "who", "what", "whose", "which", "much", "many", "whom" };
        string[] VerbQuestionsTokens = new string[] { "when", "where", "whence", "why", "how" };

        public LexicalParserEN()
        {

        }
        /// <summary>
        /// /////modife
        /// </summary>
        public LexicalParserEN(string inputText, BaseOfKnowledge knowlege)
        {
            this.knowlege = knowlege;
            if (!inputText.Equals(""))
            {
                /////////////////////
                ///////modified
                if (inputText.IndexOf(".") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("."), " ");
                }
                if (inputText.IndexOf("?") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("?"), " ");
                }
                if (inputText.IndexOf("!") > 0)
                {
                    inputText = inputText.Insert(inputText.IndexOf("!"), " ");
                }
                ///////modified
                /////////////////////
                Person Stemp = knowlege.getPersonFromString(inputText);
                if (Stemp.Name != null)
                {
                    textObject += Stemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }

                Item SItemp = knowlege.getItemFromString(inputText);
                if (SItemp.Name != null)
                {
                    textObject += SItemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textObject), textObject.Length);
                }
                Action Vtemp = knowlege.getActionFromString(inputText);
                if (Vtemp.Name != null)
                {
                    textVerb += Vtemp.Name + " ";
                    inputText = inputText.Remove(inputText.IndexOf(textVerb), textVerb.Length);
                }

                string[] words = inputText.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    for (int j = 0; j < SubjectQuestionsTokens.Length; j++)
                    {
                        if (SubjectQuestionsTokens[j].Equals(words[i]))
                        {
                            textSubject += words[i] + " ";
                            words[i] = "";
                            break;
                        }
                    }

                    for (int j = 0; j < VerbQuestionsTokens.Length; j++)
                    {
                        if (VerbQuestionsTokens[j].Equals(words[i]))
                        {
                            textVerb += words[i] + " ";
                            words[i] = "";
                            break;
                        }
                    }

                    if (knowlege.getHello(words[i]).Equals("hello"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getAnsverType(words[i]).Equals("positive"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getAnsverType(words[i]).Equals("negative"))
                    {
                        textSubject += words[i] + " ";
                    }
                    if (knowlege.getBye(words[i]).Equals("bye"))
                    {
                        textSubject += words[i] + " ";
                    }


                    if (words[i].Equals(".") || words[i].Equals("!") || words[i].Equals("?"))
                    {
                        endOfSentence = words[i];
                        words[i] = "";
                        break;
                    }

                    textObject += words[i] + " ";
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
                if (endOfSentence.Length == 0)
                {
                    endOfSentence += ".";
                }
                Debug.Log(textSubject);
                tokens = "[" + textSubject.Trim() + "]" + "[" + textVerb.Trim() + "]" + "[" + textObject.Trim() + "]" + endOfSentence.Trim();
            }
            else
                tokens = "[" + textSubject.Trim() + "]" + "[" + textVerb.Trim() + "]" + "[" + textObject.Trim() + "]" + endOfSentence.Trim();

        }

        public string getTokens()
        {
            return tokens;
        }
    }

    public class TextParserRU
    {
        string textSubject = "s";
        string textObject = "o";
        string textVerb = "v";
        string endOfSentence = "e";
        string AnsverData = "";
        BaseOfKnowledge knowlege;
        Person liveobject = new Person();
        Item inanimateobject = new Item();
        Location placeobject = new Location();
        Person livesubject = new Person();
        Item inanimatesubject = new Item();
        Location placesubject = new Location();
        Action whatDosubject = new Action();
        float condition;

        public TextParserRU()
        {

        }

        public TextParserRU(string inputText)
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
        public TextParserRU(string inputText, BaseOfKnowledge knowlege)
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

        public TextParserRU(string inputText, BaseOfKnowledge knowlege, float condition)
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
            string[] Owords = ObjectParse.Split(new Char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
            string[] Swords = SubjectParse.ToLower().Split(new Char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
            string[] Vwords = VerbParse.Split(new Char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Owords.Length; i++)
            {
                Person Stemp = knowlege.getPerson(Owords[i]);
                Item SItemp = knowlege.getItem(Owords[i]);
                Location SLtemp = knowlege.getLocation(Owords[i]);
                if (Stemp.Name != null)
                {
                    liveobject = Stemp;
                }
                else
                    if (SItemp.Name != null)
                    {
                        inanimateobject = SItemp;
                    }
                    else
                        if (SLtemp.Name != null)
                        {
                            placeobject = SLtemp;
                        }
            }

            for (int i = 0; i < Swords.Length; i++)
            {
                
                Person Stemp = knowlege.getPerson(Swords[i]);
                Item SItemp = knowlege.getItem(Swords[i]);
                Location SLtemp = knowlege.getLocation(Swords[i]);
                if (Stemp.Name != null)
                {
                    livesubject = Stemp;
                }
                else
                    if (SItemp.Name != null)
                    {
                        inanimatesubject = SItemp;
                    }
                    else
                        if (SLtemp.Name != null)
                        {
                            placeobject = SLtemp;
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
                if (Swords[i].Equals("*"))
                {
                    Result = (!liveobject.IsEmpty()) ? "Есть.":"Нет.";
                    Result = (!inanimateobject.IsEmpty())? "Есть." : "Нет.";
                    Result = (!placeobject.IsEmpty()) ? "Есть." : "Нет.";
                }
                if (Swords[i].Equals("кто"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("что"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("какой") || Swords[i].Equals("какая") || Swords[i].Equals("какие") || Swords[i].Equals("какое"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("чей") || Swords[i].Equals("чьё") || Swords[i].Equals("чья") || Swords[i].Equals("чьи"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("который"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("сколько"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кого"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чего"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кому"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чему"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кем"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чем"))
                {
                    if (!inanimateobject.IsEmpty())
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

            for (int i = 0; i < Owords.Length; i++)
            {
                string outstr = "";
                if (!liveobject.IsEmpty())
                {
                    outstr = liveobject.getPersonPropertieValue(Owords[i]);
                }

                if (!inanimateobject.IsEmpty())
                {
                    outstr = inanimateobject.getItemPropertieValue(Owords[i]);
                }
                if (!outstr.Contains("Error"))
                    Result = outstr;
            }

            if (Result.Equals(""))
                Result = "Ничего не знаю об этом.";
            if (Result.Contains("Error"))
                Result = "Не понимаю о чем ты.";

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
                Location SLtemp = knowlege.getLocation(Owords[i]);
                if (Stemp.Name != null)
                {
                    liveobject = Stemp;
                }
                else
                    if (SItemp.Name != null)
                    {
                        inanimateobject = SItemp;
                    }
                    else
                        if (SLtemp.Name != null)
                        {
                            placeobject = SLtemp;
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
            if (whatDosubject.Name!=null)
            {
                if (!liveobject.IsEmpty() && whatDosubject.IsCanIDoThisWithPerson(liveobject.id))
                {
                    result += liveobject.Name + ":";
                }
                if (!inanimateobject.IsEmpty() && whatDosubject.IsCanIDoThisWithItem(inanimateobject.id))
                {
                    result += inanimateobject.Name + ":";
                }
                if (!placeobject.IsEmpty() && whatDosubject.IsCanIDoThisWithLocation(placeobject.id))
                {
                    result += placeobject.Name + ":";
                }
            }

            if (result.Equals(""))
            {
                result += "error";
            }
            return "action:"+whatDosubject.DoAction()+ ":" + result.TrimStart();
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
    
    public class TextParserUK
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

        public TextParserUK()
        {

        }

        public TextParserUK(string inputText)
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
        public TextParserUK(string inputText, BaseOfKnowledge knowlege)
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

        public TextParserUK(string inputText, BaseOfKnowledge knowlege, float condition)
        {
            this.condition = condition;
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
            string[] Owords = ObjectParse.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] Swords = SubjectParse.ToLower().Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] Vwords = VerbParse.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
                if (Swords[i].Equals("*"))
                {
                    Result = (!liveobject.IsEmpty()) ? "Є." : "Не має.";
                    Result = (!inanimateobject.IsEmpty()) ? "Є." : "Не має.";
                }
                if (Swords[i].Equals("хто"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("що"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("який") || Swords[i].Equals("яка") || Swords[i].Equals("які") || Swords[i].Equals("яке"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("чий") || Swords[i].Equals("чиє") || Swords[i].Equals("чия") || Swords[i].Equals("чиї"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("котрий"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("скільки"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кого"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чого"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("кому"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чому"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("ким"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("чим"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
            }

            for (int i = 0; i < Vwords.Length; i++)
            {
                if (whatDosubject != null)
                {
                    if (Vwords[i].Equals("де"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("коли"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("куди"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("звідки"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("чому"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("навіщо"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("як"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                }
            }

            for (int i = 0; i < Owords.Length; i++)
            {
                string outstr = "";
                if (!liveobject.IsEmpty())
                {
                    outstr = liveobject.getPersonPropertieValue(Owords[i]);
                }

                if (!inanimateobject.IsEmpty())
                {
                    outstr = inanimateobject.getItemPropertieValue(Owords[i]);
                }
                if (!outstr.Contains("Error"))
                    Result = outstr;
            }

            if (Result.Equals(""))
                Result = "Нічого не знаю про це.";
            if (Result.Contains("Error"))
                Result = "Не розумію про що ви.";

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
            string result = "";
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
            if (whatDosubject.Name != null)
            {
                if (!liveobject.IsEmpty() && whatDosubject.IsCanIDoThisWithPerson(liveobject.id))
                {
                    result += liveobject.Name + ":";
                }
                if (!inanimateobject.IsEmpty() && whatDosubject.IsCanIDoThisWithItem(inanimateobject.id))
                {
                    result += inanimateobject.Name + ":";
                }
            }

            if (result.Equals(""))
            {
                result += "error";
            }
            return "action:" + whatDosubject.DoAction() + ":" + result.TrimStart();
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

    public class TextParserEN
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

        public TextParserEN()
        {

        }

        public TextParserEN(string inputText)
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
        public TextParserEN(string inputText, BaseOfKnowledge knowlege)
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

        public TextParserEN(string inputText, BaseOfKnowledge knowlege, float condition)
        {
            this.condition = condition;
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
            string[] Owords = ObjectParse.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] Swords = SubjectParse.ToLower().Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] Vwords = VerbParse.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
                if (Swords[i].Equals("*"))
                {
                    Result = (!liveobject.IsEmpty()) ? "Exist." : "No.";
                    Result = (!inanimateobject.IsEmpty()) ? "Exist." : "No.";
                }
                if (Swords[i].Equals("who"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }
                if (Swords[i].Equals("what"))
                {
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }

                if (Swords[i].Equals("whose"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("which"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("much")|| Swords[i].Equals("many"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                    if (!inanimateobject.IsEmpty())
                        Result = inanimateobject.getQuestionItemText(Swords[i]);
                }
                if (Swords[i].Equals("whom"))
                {
                    if (!liveobject.IsEmpty())
                        Result = liveobject.getQuestionPersonText(Swords[i]);
                }

            }

            for (int i = 0; i < Vwords.Length; i++)
            {
                if (whatDosubject != null)
                {
                    if (Vwords[i].Equals("where"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("when"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("why"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                    if (Vwords[i].Equals("how"))
                    {
                        Result = whatDosubject.getQuestionActionText(Vwords[i], condition);
                    }
                }
            }

            for (int i = 0; i < Owords.Length; i++)
            {
                string outstr = "";
                if (!liveobject.IsEmpty())
                {
                    outstr = liveobject.getPersonPropertieValue(Owords[i]);
                }

                if (!inanimateobject.IsEmpty())
                {
                    outstr = inanimateobject.getItemPropertieValue(Owords[i]);
                }
                if (!outstr.Contains("Error"))
                    Result = outstr;
            }

            if (Result.Equals(""))
                Result = "Ничего не знаю об этом.";
            if (Result.Contains("Error"))
                Result = "Не понимаю о чем ты.";

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
            string result = "";
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
            if (whatDosubject.Name != null)
            {
                if (!liveobject.IsEmpty() && whatDosubject.IsCanIDoThisWithPerson(liveobject.id))
                {
                    result += liveobject.Name + ":";
                }
                if (!inanimateobject.IsEmpty() && whatDosubject.IsCanIDoThisWithItem(inanimateobject.id))
                {
                    result += inanimateobject.Name + ":";
                }
            }

            if (result.Equals(""))
            {
                result += "error";
            }
            return "action:" + whatDosubject.DoAction() + ":" + result.TrimStart();
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

                            if (reader.Name == "PersonPropertie")
                            {
                                Propertie temp = new Propertie();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("Name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("Value");
                                temp.Value = reader.Value;
                                reader.MoveToAttribute("Appearance");
                                temp.Appearance = reader.Value;
                                Persons[Persons.Count - 1].AddPersonPropertie(temp);
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

                            if (reader.Name == "ItemPropertie")
                            {
                                Propertie temp = new Propertie();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("Name");
                                temp.Name = reader.Value;
                                reader.MoveToAttribute("Value");
                                temp.Value = reader.Value;
                                reader.MoveToAttribute("Appearance");
                                temp.Appearance = reader.Value;
                                Items[Items.Count - 1].AddItemPropertie(temp);
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
                                reader.MoveToAttribute("locationreferences");
                                temp.AddLocationRef(reader.Value);
                                Actions.Add(temp);
                                Debug.Log("A");
                            }

                            if (reader.Name == "QuestionAction")
                            {
                                Question temp = new Question();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                //reader.MoveToAttribute("personreferences");
                                //temp.AddPersonRef(float.Parse(reader.Value, CultureInfo.InvariantCulture));
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
                                Debug.Log("L");

                            }

                            if (reader.Name == "Event")
                            {
                                Event temp = new Event();
                                reader.MoveToAttribute("id");
                                temp.id = float.Parse(reader.Value, CultureInfo.InvariantCulture);
                                reader.MoveToAttribute("name");
                                temp.Name = reader.Value;
                                Events.Add(temp);
                                Debug.Log("E");
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
                                Debug.Log("S");
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
                return bk.Phrase.Contains(" " + text + " ", comp);
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
                return bk.Phrase.Contains(" " + text + " ", comp);
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
            Ansver temp = Ansvers.Find(delegate (Ansver bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Pharse.Contains(" " + text + " ", comp);
            });
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
                return bk.Pharse.Contains(" "+text+" ", comp);
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
            Person temp = Persons.Find(delegate (Person bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Name.Contains(name, comp);
            });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Person();
            }
        }

        public Person getPersonFromString(string text)
        {
            Person temp=null;
            for (int i = 0; i < Persons.Count; i++)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                string[] compare = Persons[i].Name.Split('/');
                for (int j = 0; j < compare.Length; j++)
                {
                    if (text.Contains(compare[j] + " ", comp))
                    {
                        temp = Persons[i];
                        temp.Name = compare[j];
                    }
                }
            }
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
            Item temp = Items.Find(delegate (Item bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Name.Contains(name, comp);
            });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Item();
            }
        }

        public Item getItemFromString(string text)
        {
            Item temp = null;
            for (int i = 0; i < Items.Count; i++)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                string[] compare = Items[i].Name.Split('/');
                for (int j = 0; j < compare.Length; j++)
                {
                    if (text.Contains(compare[j] + " ", comp))
                    {
                        temp = Items[i];
                        temp.Name = compare[j];
                        Debug.Log(temp.Name);
                        break;
                        
                    }
                }
            }
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
            Action temp = Actions.Find(delegate (Action bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Name.Contains(name, comp);
            });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Action();
            }
        }

        public Action getActionFromString(string text)
        {
            Action temp = null;
            for (int i = 0; i < Actions.Count; i++)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                string[] compare = Actions[i].Name.Split('/');
                for (int j = 0; j < compare.Length; j++)
                {
                    if (text.Contains(compare[j] + " ", comp))
                    {
                        temp = Actions[i];
                        temp.Name = compare[j];
                    }
                }
            }
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Action();
            }
        }

        public Location getLocation(string name)
        {
            Location temp = Locations.Find(delegate (Location bk)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Name.Contains(name, comp);
            });
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Location();
            }
        }

        public Location getLocationFromString(string text)
        {
            Location temp = null;
            for (int i = 0; i < Locations.Count; i++)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                string[] compare = Locations[i].Name.Split('/');
                for (int j = 0; j < compare.Length; j++)
                {
                    if (text.Contains(compare[j] + " ", comp))
                    {
                        temp = Locations[i];
                        temp.Name = compare[j];
                    }
                }
            }
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return new Location();
            }
        }

        public Event getEvents(string name)
        {
            Event temp = Events.Find(delegate (Event bk) 
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                return bk.Name.Contains(name, comp);
            });
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

    public class Propertie
    {
        public float id = -1.0f;
        public string Name;
        public string Value;
        public string Appearance;

        public Propertie()
        {

        }
    }

    public class Item
    {
        public float id=-1.0f;
        public string Name;
        public string Appearance;
        List<Question> listQuestionItem = new List<Question>();
        List<Propertie> listItemPropertie = new List<Propertie>();

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

        public void AddItemPropertie(Propertie propertie)
        {
            listItemPropertie.Add(propertie);
        }

        public string getItemPropertieName(string text)
        {
            Propertie temp = listItemPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.Name;
            }
            else
            {
                return "Error ItemPropertieName";
            }
        }

        public string getItemPropertieValue(string text)
        {
            Propertie temp = listItemPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.Value;
            }
            else
            {
                return "Error ItemPropertieValue";
            }
        }

        public string getItemPropertieAppearance(string text)
        {
            Propertie temp = listItemPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.Appearance;
            }
            else
            {
                return "Error ItemPropertieAppearance";
            }
        }

        public float getItemPropertieID(string text)
        {
            Propertie temp = listItemPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.id;
            }
            else
            {
                return -1.0f;
            }
        }

        public bool IsEmpty()
        {
            return Name == null;
        }

    }

    public class Location
    {
        public float id;
        public string Name;
        public string Appearance;
        public int positionX;
        public int positionY;

        public bool IsEmpty()
        {
            return Name == null;
        }
    }

    public class Action
    {
        public float id=-1.0f;
        public string Name;
        public string Appearance;
        private string personreferences;
        private string itemreferences;
        private string locationreferences;
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
        public bool IsCanIDoThisWithLocation(float refid)
        {
            bool result = false;
            string[] temp = locationreferences.Split(new Char[] { ' ', '/' });
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

        public void AddLocationRef(string locationreferences)
        {
            this.locationreferences = locationreferences;
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

        public bool IsEmpty()
        {
            return Name==null; 
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
        public float id=-1.0f;
        public string Name;
        public string Appearance;
        public float Mood;
        List<Question> listQuestionPerson = new List<Question>();
        List<Propertie> listPersonPropertie = new List<Propertie>();

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

        public void AddPersonPropertie(Propertie propertie)
        {
            listPersonPropertie.Add(propertie);
        }

        public string getPersonPropertieName(string text)
        {
            Propertie temp = listPersonPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.Name;
            }
            else
            {
                return "Error PersonPropertieName";
            }
        }

        public string getPersonPropertieValue(string text)
        {
            Propertie temp = listPersonPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name==text; });
            if (temp != null)
            {
                return temp.Value;
            }
            else
            {
                return "Error PersonPropertieValue";
            }
        }

        public string getPersonPropertieAppearance(string text)
        {
            Propertie temp = listPersonPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.Appearance;
            }
            else
            {
                return "Error PersonPropertieAppearance";
            }
        }

        public float getPersonPropertieID(string text)
        {
            Propertie temp = listPersonPropertie.Find(delegate (Propertie bk) {
                bk.Name = bk.Name.ToLower();
                text = text.ToLower();
                return bk.Name == text; });
            if (temp != null)
            {
                return temp.id;
            }
            else
            {
                return -1.0f;
            }
        }

        public bool IsEmpty()
        {
            return Name == null;
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
