using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogText  : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _delay;
    [HideInInspector] public List<DialogString> _TextLine = new List<DialogString>();
    bool isTextTyping = false;
    [SerializeField] private TMP_Text[] _textChois = new TMP_Text[3];
    [SerializeField] private GameObject _textChoisPanel;
    private UnityEvent[] choisAction = new UnityEvent[3];
    private bool _isChoise;
    private int selected = 0;
    [SerializeField] public List<Sprite> sprites = new List<Sprite>();
    [SerializeField] Image img;

  

    private void Start()    {
        ClierText();
        SetDialog(new List<DialogString>()
        {
            new DialogCoise("Ty… w końcu przyszedłeś. Czekałem na ciebie. A może… nie wiesz, dlaczego tu jesteś?", sprites[0], "Kim jesteś?", "O czym mówisz?", "Czekałeś na mnie?",()=>{_TextLine.Insert(0, new DialogString("Hmmm. Dobre pytanie, ale nie to, które powinieneś zadać jako pierwsze. Może lepiej zapytać, kim ty jesteś?", sprites[1]));}, ()=>{_TextLine.Insert(0, new DialogString("Ciągnie cię tutaj, ale nie wiesz dlaczego? A może boisz się to przyznać? Nie udawaj.", sprites[1]));}, ()=>{_TextLine.Insert(0, new DialogString("Czy nie czujesz tego? Coś nas łączy. A może wolisz nie pamiętać?", sprites[1]));}),
            new DialogCoise("Uważaj. Każdy ruch zmienia nie tylko grę… ale i rzeczywistość.", sprites[2], "Co masz na myśli?", "Przerażasz mnie…", "Nie boję się. Po prostu powiedz mi, co robić.", ()=>{_TextLine.Insert(0, new DialogString("Zobaczysz. A może… już widziałeś?", sprites[3]));}, ()=>{_TextLine.Insert(0, new DialogString("Naprawdę? To dlaczego wciąż tu jesteś? Strach może zatrzymać… ale także popchnąć naprzód.", sprites[3]));}, ()=>{_TextLine.Insert(0, new DialogString("Dobrze. Idź naprzód. Ale pamiętaj… odwaga bez rozsądku to lekkomyślność.", sprites[3]));})
        });
        NextText();
    }

    private void Update()
    {
        if (_isChoise)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _textChois[selected].fontStyle = TMPro.FontStyles.Normal;
                selected = (selected + 2) % 3;
                _textChois[selected].fontStyle = TMPro.FontStyles.Bold;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _textChois[selected].fontStyle = TMPro.FontStyles.Normal;
                selected = (selected + 1) % 3;
                _textChois[selected].fontStyle = TMPro.FontStyles.Bold;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isTextTyping)
                {
                    _isChoise = false;
                    choisAction[selected]?.Invoke();
                }
                NextText();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            NextText();
        }
    }

    public bool NextText()
    {
        if (_TextLine.Count > 0)
        {
            if (isTextTyping)
            {
                isTextTyping = false;
                return false;
            }
            else
            {
                ClierText();
                SetText(_TextLine[0].Text);
                
                if(_TextLine[0] is DialogCoise)
                {
                    _textChois[selected].fontStyle = TMPro.FontStyles.Bold;
                    for (int i =0; i< _textChois.Length; i++)
                    {
                        _textChois[i].text = (_TextLine[0] as DialogCoise).choisText[i];
                    } for (int i =0; i< _textChois.Length; i++)
                    {
                        choisAction[i] = (_TextLine[0] as DialogCoise).choisAction[i];
                    }
                    _isChoise = true;
                    _textChoisPanel.SetActive(true);
                }else
                {
                    _textChoisPanel.SetActive(false);
                }

                img.sprite = _TextLine[0].sprite;
                _TextLine.RemoveAt(0);
                return true;
            }
        }
        else
        {
            if (isTextTyping)
            {
                isTextTyping = false;
                return false;
            }
            else
            {
                transform.parent.gameObject.SetActive(false);
                return true;
            }
        }
    }
    public void SetDialog(List<DialogString> text)
    {
        _text.gameObject.SetActive(true);
        _TextLine = text;
    }
    public void ClierText()
    {
        _text.text = "";
    }

    public void SetText(string text)
    {
        StopCoroutine("ShowText");
        StartCoroutine(ShowText(text));
    }

    private IEnumerator ShowText(string text)
    {
        isTextTyping = true;
        float timer = 0;
        while (0 < text.Length)
        {
            yield return null;
            timer += Time.deltaTime;
            if (timer > _delay)
            {
                timer -= _delay;
                _text.text += text[0];
                text = text.Remove(0, 1);
            }

            if (!isTextTyping)
            {
                _text.text += text;
                text = "";
            }
        }
        isTextTyping = false;
    }
}


[Serializable]
public class DialogString
{
    [HideInInspector] public bool isChoise = false;
    public string Text;
    public Sprite sprite;

    public DialogString(string text, Sprite sprite)
    {
        Text = text;
        this.sprite = sprite;
    }public DialogString()
    {
        Text = "text";
    }
}

[Serializable]
public class DialogCoise : DialogString
{
    [SerializeField]public string[] choisText = new string[3];
    [SerializeField]public UnityEvent[] choisAction = new UnityEvent[3];

    public DialogCoise(string text, Sprite sprite, string chois1, string chois2, string chois3, UnityEvent action1, UnityEvent action2,UnityEvent action3):base(text, sprite)
    {
        choisText[0] = chois1;
        choisText[1] = chois2;
        choisText[2] = chois3;
        choisAction[0] = action1;
        choisAction[1] = action2;
        choisAction[2] = action3;
    }
    public DialogCoise(string text, Sprite sprite, string chois1, string chois2, string chois3, UnityAction action1, UnityAction action2,UnityAction action3):base(text, sprite)
    {
        choisText[0] = chois1;
        choisText[1] = chois2;
        choisText[2] = chois3;
        choisAction[0] = new UnityEvent();
        choisAction[0].AddListener(action1);
        choisAction[1] = new UnityEvent();
        choisAction[1].AddListener(action2);
        choisAction[2] = new UnityEvent();
        choisAction[2].AddListener(action3);
    }
    public DialogCoise():base()
    {
        choisText[0] = "";
        choisText[1] = "";
        choisText[2] = "";
        choisAction[0] = new UnityEvent();
        choisAction[1] = new UnityEvent();
        choisAction[2] = new UnityEvent();
    }
}

