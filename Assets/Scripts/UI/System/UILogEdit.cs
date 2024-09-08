
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
                    DateTime date1 = new DateTime(1, 1, 1); // 今天的日期
                    storage.StoreString(date1, curId.ToString());
                }
                if (action != null)
                {
                    action(questionText.text);
                }
                CloseSelf();
                Common.Instance.ShowTips("记录成功");

            }, () =>
            {
               

            });
            gui.come_back_btn.gameObject.SetActive(false);
        });
        close_btn.onClick.AddListener(CloseSelf);
        if (inputField != null)
        {
            // 监听输入字段的按键事件
            inputField.onValidateInput += OnValidateInput;
        }
        //  inputField.readOnly = true;

    }
    char OnValidateInput(string text, int charIndex, char addedChar)
    {
        inputField.caretPosition = previousText.Length; // 保持光标在末尾
        MoveCursorToEnd();

        // 允许其他输入
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

        // 初始化问题文本
        string originalText = this.words;

        questionText.text = originalText;
        questionText.ForceMeshUpdate();
        if (!isEdit)
        {
            return;
        }
        // 初始化输入框
        inputField.gameObject.SetActive(false);
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // 设置事件触发系统
        EventTrigger eventTrigger = questionText.gameObject.AddComponent<EventTrigger>();

        // 设置点击事件
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
            // 将世界坐标转换为本地坐标
            Vector3 lastCharPosition = questionText.transform.TransformPoint(lastCharacterPosition);
            new_bie.transform.position = new Vector3(lastCharPosition.x + 0.80f, lastCharPosition.y - 0.4f, 0);

        }
    }
    void MoveCursorToEnd()
    {
        // 将光标移到文本末尾
        inputField.MoveTextEnd(false);
    }
    void OnInputFieldValueChanged(string text)
    {
     
        if (text.Length < min_length)
        {
            inputField.text = previousText;

            inputField.caretPosition = previousText.Length; // 保持光标在末尾
            MoveCursorToEnd();
        }
        else
        {
            StartCoroutine(MoveCaretToEndNextFrame());
    
        }
        if (text.Length > min_length + 200)
        {
            // 截断输入的文本到最大字符数
            inputField.text = text.Substring(0, min_length + 200);

        }

        // 确保下一帧更新光标位置
  


    }
    string GetNewText(string oldText, string newText)
    {
        if (newText.StartsWith(oldText))
        {
            return newText.Substring(oldText.Length);
        }
        return newText; // 如果不是简单的追加（例如删除或修改），返回整个新文本
    }

    string InsertTextIntoTemplate(string newText)
    {
        return template.Replace("<link={1}></link>", $"<link={{1}}>{newText}</link>");
    }
    string RemoveTextInsideLink(string text)
    {
        // 使用正则表达式匹配 <link={1}> 和 </link> 之间的文字，并将其替换为空字符串
        string pattern = @"<link=\{1\}>.*?</link>";
        string replacement = "<link={1}></link>";
        string result = Regex.Replace(text, pattern, replacement);
        return result;

    }
    public string RemoveUnderscoreFromLink(string input)
    {
        // 找到 <link={1}> 和 </link> 的位置
        string startTag = "<link={1}>";
        string endTag = "</link>";
      
        int startIndex = input.IndexOf(startTag) + startTag.Length;
        currentStartIndex = startIndex;
        int endIndex = input.IndexOf(endTag);

        if (startIndex >= startTag.Length && endIndex > startIndex)
        {
            // 获取两标签之间的部分
            string middleContent = input.Substring(startIndex, endIndex - startIndex);

            // 移除下划线
            middleContent = middleContent.Replace("_", "");

            // 重建字符串
            string result = input.Substring(0, startIndex) + middleContent + input.Substring(endIndex);
            return result;
        }

        return input; // 如果标签不匹配，返回原始字符串
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

    
        // 确保下一帧更新光标位置
        StartCoroutine(MoveCaretToEndNextFrame());
        //bool isClickLink = false;
        //int linkStartIndex = -1;
        //int linkEndIndex = -1;
        // // 将点击位置转换为文本坐标
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(questionText.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        // // 遍历链接检查点击位置
        // TMP_TextInfo textInfo = questionText.textInfo;
        // for (int i = 0; i < textInfo.linkCount; i++)
        // {
        //     TMP_LinkInfo linkInfo = textInfo.linkInfo[i];
        //     linkStartIndex = linkInfo.linkTextfirstCharacterIndex;
        //     linkEndIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1;

        //     // 获取链接的范围
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
        //         // 计算链接标签的内容的结束索引
        //         int contentEndIndex = endIndex - 1;
        //         // 提取链接标签的内容
        //         string content = questionText.text.Substring(contentStartIndex + 3, contentEndIndex - contentStartIndex - 2);
        //         string spaces = new string(' ', content.Length); // 创建由 N 个空格组成的字符串
        //         questionText.text = questionText.text.Remove(contentStartIndex + 3, contentEndIndex - contentStartIndex - 2);
        //         questionText.text = questionText.text.Insert(contentStartIndex + 3, spaces);
        //         inputField.text = content;
        //         currentEndIndex = contentEndIndex;
        //         // 处理链接点击事件
        //         isClickLink = true;
        //         break;
        //     }
        // }

        //// 点击下划线判断
        // if (isClickLink)
        // {

        //     inputField.gameObject.SetActive(true);
        //     //inputField.text = ""; // 清空输入框内容
        //     inputField.ActivateInputField(); // 激活输入框
        //     TMP_CharacterInfo lastCharacterInfo = questionText.textInfo.characterInfo[currentStartIndex];
        //     Vector3 lastCharacterPosition = lastCharacterInfo.bottomRight;
        //     // 将世界坐标转换为本地坐标
        //     Vector3 lastCharPosition = questionText.transform.TransformPoint(lastCharacterPosition);
        //     inputField.transform.position = new Vector3(lastCharPosition.x - 0.20f, lastCharPosition.y + 0.3f, 0);
        //     new_bie.SetActive(false);
        // }
        // else
        // {
        //     // 如果点击的位置没有下划线，则隐藏输入框
        //     inputField.gameObject.SetActive(false);
        //     currentStartIndex = -1;
        //     currentEndIndex = -1;
        // }



    }
    // 在下一帧将光标移动到文本末尾
    private System.Collections.IEnumerator MoveCaretToEndNextFrame()
    {
        // 等待到下一帧
        yield return null;

        // 将光标位置设置为文本的长度，即末尾
        inputField.caretPosition = inputField.text.Length;
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }

    void OnInputFieldEndEdit(string userAnswer)
    {
        //// 更新下划线为用户输入的内容
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
         //   previousText = currentText; // 更新 previousText 以备下次比较
            //string newText = userAnswer;

            // 更新 TextMeshProUGUI 组件的文本
            //    yourTMPComponent.text = newText;
            questionText.text = result;

            // 隐藏输入框
            inputField.gameObject.SetActive(false);
            questionText.gameObject.SetActive(true);
            currentStartIndex = -1;
            currentEndIndex = -1;
       // }
    }
}