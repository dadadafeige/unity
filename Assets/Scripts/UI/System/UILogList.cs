
using DG.Tweening;
using Spine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UILogList : UIBase
{
    public RectTransform calendar_group;
    public GameObject calendar_item;
    private DateTime currentDate;
    public TextMeshProUGUI questionText;
    public TMP_InputField inputField;
    private int currentStartIndex = -1;
    private int currentEndIndex = -1;
    public Button edit_btn;
    private string words = null;
    private DateTime currdate;
    public int currId = -1;
    public Action<string> changeStr;
    public Button close_btn;
    public Button previous_btn;
    public Button next_btn;
    public TextMeshProUGUI date;
    private int addMonth = 0;
    private GameObject curSele;
    public override void OnAwake()
    {

    }

    // Start is called before the first frame update
    public override void OnStart()
    {
        inputField.characterLimit = 5;
        // 初始化当前日期为今天
        currentDate = DateTime.Today;
        inputField.gameObject.SetActive(false);
        // 更新日历界面
        UpdateCalendar();
        //InitTextUI();
        close_btn.onClick.AddListener(CloseSelf);
        edit_btn.onClick.AddListener(() =>
        {

            UILogEdit gui = UiManager.OpenUI<UILogEdit>("UILogEdit");
            gui.SetData(currdate, words, currId,changeStr);

        });
        changeStr = (string str) =>
        {
            questionText.text = str;
        };
        previous_btn.onClick.AddListener(() =>
        {
            addMonth--;
            SwitchMonth();

        });
        next_btn.onClick.AddListener(() =>
        {
            addMonth++;
            SwitchMonth();

        });
    }
    void SwitchMonth()
    {

        // 遍历组下的所有子对象，并销毁它们
        foreach (Transform child in calendar_group)
        {
            Destroy(child.gameObject);
        }
        UpdateCalendar();

    }
    public bool IsPastToday(DateTime dateTime)
    {

        // 检查给定日期是否在今天之前
        return dateTime.Date < currentDate;
    }
    void UpdateCalendar()
    {
        // 更新月份和年份的显示
        //  monthYearText.text = currentDate.ToString("MMMM yyyy");
        DateTime previousMonth = currentDate.AddMonths(addMonth);
        date.text = previousMonth.ToString("MMMM yyyy");
        // 获取当前月的第一天
        DateTime firstDayOfMonth = new DateTime(previousMonth.Year, previousMonth.Month, 1);
        // 计算第一天是星期几
        int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;

        // 获取当前月的最后一天
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        // 清空已有的单元格
        //ClearGrid();

        // 生成日历单元格
        int dayCounter = 1;
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                // 实例化单元格预制体
                GameObject dayCell = Instantiate(calendar_item, calendar_group);
                dayCell.SetActive(true);
                // 设置单元格的文本
                CalendarNode itemNode = dayCell.GetComponentInChildren<CalendarNode>();
                if (row == 0 && col < dayOfWeek)
                {
                    // 如果是第一行，并且当前列在第一天之前，不显示文本
                    itemNode.root.SetActive(false);
                }
                else if (dayCounter <= lastDayOfMonth.Day)
                {
                    DateTime counterDayOfMonth = new DateTime(previousMonth.Year, previousMonth.Month, dayCounter);
                    DateStringStorage storage = new DateStringStorage("DateStringStorage.txt");
                    string str = storage.RetrieveString(counterDayOfMonth);
                    // 显示当前日期
                    itemNode.date.text = dayCounter.ToString();
                    itemNode.checkmark.SetActive(IsPastToday(counterDayOfMonth) && str != null);
                    itemNode.red.SetActive(counterDayOfMonth == currentDate);
                    itemNode.select.SetActive(false);
                    itemNode.btn.onClick.RemoveAllListeners();
                    itemNode.day = dayCounter;
                    itemNode.counterDayOfMonth = counterDayOfMonth;
                    itemNode.btn.onClick.AddListener(() =>{
                        int day = itemNode.day;
                        if (curSele != null)
                        {
                            curSele.SetActive(false);
                        }
                       
                        itemNode.select.SetActive(true);
                        curSele = itemNode.select;
                        if (!IsPastToday(itemNode.counterDayOfMonth) && counterDayOfMonth != currentDate)
                        {
                            Common.Instance.ShowTips("时间未到");
                            return;
                        }
                        DateStringStorage storage = new DateStringStorage("DateStringStorage.txt");
                        DateTime date = itemNode.counterDayOfMonth; // 今天的日期
                        currdate = date;
                        string str = storage.RetrieveString(date);
                        if (str == null)
                        {
                            if (day < currentDate.Day)
                            {
                                questionText.text = "";
                            }
                            else
                            {
                                DateTime date1 = new DateTime(1, 1, 1); // 今天的日期
                                string str1 = storage.RetrieveString(date1);
                                if (str1 == null)
                                {
                                    log_wordscnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<log_wordscnofigData>("log_words", 1);
                                    currId = 1;
                                    questionText.text = cfg.words;
                                }
                                else
                                {
                                    int id = int.Parse(str1);
                                    id++;
                                    log_wordscnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<log_wordscnofigData>("log_words", id);
                                    if (cfg != null)
                                    {
                                        questionText.text = cfg.words;
                                        currId = id;
                                    }
                                    else
                                    {
                                        questionText.text = "";
                                    }
                                }



                     
                            }
                            
                        }
                        else
                        {
                            questionText.text = str;
                        }
                        words = questionText.text;


                    });
                    if (counterDayOfMonth == currentDate)
                    {
                        itemNode.btn.onClick.Invoke();
                    }
                    dayCounter++;
                   
                }
                else
                {
                    // 如果当前日期已经超过了当前月份的最后一天，不显示文本
                    itemNode.root.SetActive(false);
                }
            }
        }
    }

}