
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class UILogEdit : UIBase
{

    public TextMeshProUGUI questionText;
    public TMP_InputField inputField;
    private int currentBlankIndex = -1;
    private int currentStartIndex = -1;
    private int currentEndIndex = -1;
    DateTime currdateTime;
    string words;
    public Button finish_btn;
    public int curId;
    Action<string> action;
    public Button close_btn;
    public GameObject new_bie;
    private bool isNewBie = false;
    private bool isEdit = true;
    private int min_length;
    private string previousText = "";
    private string template = "";

    public override void OnAwake()
    {

    }
    public void OpenNewBie()
    {

        isNewBie = true;
       

    }
    // Start is called before the first frame update
    public override void OnStart()
    {

        finish_btn.onClick.AddListener(() =>
        {
           
            UIFeedingFishSettle gui = Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () =>
            {
                

            }, () => {
                DateStringStorage storage = new DateStringStorage("DateStringStorage.txt");
                storage.ModifyString(currdateTime, questionText.text);
                if (curId > 0)
                {
                    DateTime date1 = new DateTime(1, 1, 1); // ���������
                    storage.StoreString(date1, curId.ToString());
                }
                if (action != null)
                {
                    action(questionText.text);
                }
                CloseSelf();
                Common.Instance.ShowTips("��¼�ɹ�");

            }, () =>
            {
               

            });
            gui.come_back_btn.gameObject.SetActive(false);
        });
        close_btn.onClick.AddListener(CloseSelf);
        if (inputField != null)
        {
            // ���������ֶεİ����¼�
            inputField.onValidateInput += OnValidateInput;
        }
        //  inputField.readOnly = true;

    }
    char OnValidateInput(string text, int charIndex, char addedChar)
    {
        inputField.caretPosition = previousText.Length; // ���ֹ����ĩβ
        MoveCursorToEnd();

        // ������������
        return addedChar;
    }
    public void SetData(DateTime currdateTime,string words,int currId,Action<string> action, bool isEdit = true)
    {
        this.currdateTime = currdateTime;
        this.words = words;
        curId = currId;
        this.action = action;
        this.isEdit = isEdit;
        InitTextUI();
    }
    private void InitTextUI()
    {

        // ��ʼ�������ı�
        string originalText = this.words;

        questionText.text = originalText;
        questionText.ForceMeshUpdate();
        if (!isEdit)
        {
            return;
        }
        // ��ʼ�������
        inputField.gameObject.SetActive(false);
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // �����¼�����ϵͳ
        EventTrigger eventTrigger = questionText.gameObject.AddComponent<EventTrigger>();

        // ���õ���¼�
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnTextClick(data as PointerEventData); });
        eventTrigger.triggers.Add(entry);
        if (isNewBie)
        {
            new_bie.SetActive(true);
            TMP_TextInfo textInfo = questionText.textInfo;
            TMP_LinkInfo linkInfo = textInfo.linkInfo[0];
            int linkStartIndex = -1;
            linkStartIndex = linkInfo.linkTextfirstCharacterIndex;
            TMP_CharacterInfo lastCharacterInfo = questionText.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex];
            Vector3 lastCharacterPosition = lastCharacterInfo.bottomRight;
            // ����������ת��Ϊ��������
            Vector3 lastCharPosition = questionText.transform.TransformPoint(lastCharacterPosition);
            new_bie.transform.position = new Vector3(lastCharPosition.x + 0.80f, lastCharPosition.y - 0.4f, 0);

        }
    }
    void MoveCursorToEnd()
    {
        // ������Ƶ��ı�ĩβ
        inputField.MoveTextEnd(false);
    }
    void OnInputFieldValueChanged(string text)
    {
     
        if (text.Length < min_length)
        {
            inputField.text = previousText;

            inputField.caretPosition = previousText.Length; // ���ֹ����ĩβ
            MoveCursorToEnd();
        }
        else
        {
            StartCoroutine(MoveCaretToEndNextFrame());
    
        }
        if (text.Length > min_length + 200)
        {
            // �ض�������ı�������ַ���
            inputField.text = text.Substring(0, min_length + 200);

        }

        // ȷ����һ֡���¹��λ��
  


    }
    string GetNewText(string oldText, string newText)
    {
        if (newText.StartsWith(oldText))
        {
            return newText.Substring(oldText.Length);
        }
        return newText; // ������Ǽ򵥵�׷�ӣ�����ɾ�����޸ģ��������������ı�
    }

    string InsertTextIntoTemplate(string newText)
    {
        return template.Replace("<link={1}></link>", $"<link={{1}}>{newText}</link>");
    }
    string RemoveTextInsideLink(string text)
    {
        // ʹ��������ʽƥ�� <link={1}> �� </link> ֮������֣��������滻Ϊ���ַ���
        string pattern = @"<link=\{1\}>.*?</link>";
        string replacement = "<link={1}></link>";
        string result = Regex.Replace(text, pattern, replacement);
        return result;

    }
    public string RemoveUnderscoreFromLink(string input)
    {
        // �ҵ� <link={1}> �� </link> ��λ��
        string startTag = "<link={1}>";
        string endTag = "</link>";
      
        int startIndex = input.IndexOf(startTag) + startTag.Length;
        currentStartIndex = startIndex;
        int endIndex = input.IndexOf(endTag);

        if (startIndex >= startTag.Length && endIndex > startIndex)
        {
            // ��ȡ����ǩ֮��Ĳ���
            string middleContent = input.Substring(startIndex, endIndex - startIndex);

            // �Ƴ��»���
            middleContent = middleContent.Replace("_", "");

            // �ؽ��ַ���
            string result = input.Substring(0, startIndex) + middleContent + input.Substring(endIndex);
            return result;
        }

        return input; // �����ǩ��ƥ�䣬����ԭʼ�ַ���
    }
    void OnTextClick(PointerEventData eventData)
    {
        inputField.gameObject.SetActive(true);
        questionText.gameObject.SetActive(false);
     
        inputField.text = RemoveTextInsideLink(questionText.text);
        previousText = inputField.text;
        min_length = previousText.Length;
        template = inputField.text;
        new_bie.SetActive(false);

    
        // ȷ����һ֡���¹��λ��
        StartCoroutine(MoveCaretToEndNextFrame());
        //bool isClickLink = false;
        //int linkStartIndex = -1;
        //int linkEndIndex = -1;
        // // �����λ��ת��Ϊ�ı�����
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(questionText.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        // // �������Ӽ����λ��
        // TMP_TextInfo textInfo = questionText.textInfo;
        // for (int i = 0; i < textInfo.linkCount; i++)
        // {
        //     TMP_LinkInfo linkInfo = textInfo.linkInfo[i];
        //     linkStartIndex = linkInfo.linkTextfirstCharacterIndex;
        //     linkEndIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1;

        //     // ��ȡ���ӵķ�Χ
        //     TMP_CharacterInfo charInfoStart = textInfo.characterInfo[linkStartIndex];
        //     TMP_CharacterInfo charInfoEnd = textInfo.characterInfo[linkEndIndex];

        //     float linkWidth = charInfoEnd.bottomRight.x - charInfoStart.topLeft.x;
        //     float linkHeight = charInfoStart.topLeft.y + 50 - charInfoStart.bottomLeft.y;

        //     Rect linkRect = new Rect(charInfoStart.topLeft.x, charInfoStart.bottomLeft.y, linkWidth, linkHeight);
        //     if (linkRect.Contains(localPoint))
        //     {
        //         currentStartIndex = linkStartIndex;
        //         string linkTag = "<link={1}>";
        //         int contentStartIndex = linkStartIndex + linkTag.Length;
        //         int endIndex = questionText.text.IndexOf("</link>", linkStartIndex);
        //         // �������ӱ�ǩ�����ݵĽ�������
        //         int contentEndIndex = endIndex - 1;
        //         // ��ȡ���ӱ�ǩ������
        //         string content = questionText.text.Substring(contentStartIndex + 3, contentEndIndex - contentStartIndex - 2);
        //         string spaces = new string(' ', content.Length); // ������ N ���ո���ɵ��ַ���
        //         questionText.text = questionText.text.Remove(contentStartIndex + 3, contentEndIndex - contentStartIndex - 2);
        //         questionText.text = questionText.text.Insert(contentStartIndex + 3, spaces);
        //         inputField.text = content;
        //         currentEndIndex = contentEndIndex;
        //         // �������ӵ���¼�
        //         isClickLink = true;
        //         break;
        //     }
        // }

        //// ����»����ж�
        // if (isClickLink)
        // {

        //     inputField.gameObject.SetActive(true);
        //     //inputField.text = ""; // ������������
        //     inputField.ActivateInputField(); // ���������
        //     TMP_CharacterInfo lastCharacterInfo = questionText.textInfo.characterInfo[currentStartIndex];
        //     Vector3 lastCharacterPosition = lastCharacterInfo.bottomRight;
        //     // ����������ת��Ϊ��������
        //     Vector3 lastCharPosition = questionText.transform.TransformPoint(lastCharacterPosition);
        //     inputField.transform.position = new Vector3(lastCharPosition.x - 0.20f, lastCharPosition.y + 0.3f, 0);
        //     new_bie.SetActive(false);
        // }
        // else
        // {
        //     // ��������λ��û���»��ߣ������������
        //     inputField.gameObject.SetActive(false);
        //     currentStartIndex = -1;
        //     currentEndIndex = -1;
        // }



    }
    // ����һ֡������ƶ����ı�ĩβ
    private System.Collections.IEnumerator MoveCaretToEndNextFrame()
    {
        // �ȴ�����һ֡
        yield return null;

        // �����λ������Ϊ�ı��ĳ��ȣ���ĩβ
        inputField.caretPosition = inputField.text.Length;
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }

    void OnInputFieldEndEdit(string userAnswer)
    {
        //// �����»���Ϊ�û����������
        //if (currentStartIndex != -1)
        //{
            string currentText = questionText.text;
            if (userAnswer == "")
            {
                return;
            }
           // string currentText = userAnswer;
            string newText = GetNewText(previousText, userAnswer);
            string result = InsertTextIntoTemplate(newText);
            questionText.text = result;
         //   previousText = currentText; // ���� previousText �Ա��´αȽ�
            //string newText = userAnswer;

            // ���� TextMeshProUGUI ������ı�
            //    yourTMPComponent.text = newText;
            questionText.text = result;

            // ���������
            inputField.gameObject.SetActive(false);
            questionText.gameObject.SetActive(true);
            currentStartIndex = -1;
            currentEndIndex = -1;
       // }
    }
}