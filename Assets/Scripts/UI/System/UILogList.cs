
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
        // ��ʼ����ǰ����Ϊ����
        currentDate = DateTime.Today;
        inputField.gameObject.SetActive(false);
        // ������������
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

        // �������µ������Ӷ��󣬲���������
        foreach (Transform child in calendar_group)
        {
            Destroy(child.gameObject);
        }
        UpdateCalendar();

    }
    public bool IsPastToday(DateTime dateTime)
    {

        // �����������Ƿ��ڽ���֮ǰ
        return dateTime.Date < currentDate;
    }
    void UpdateCalendar()
    {
        // �����·ݺ���ݵ���ʾ
        //  monthYearText.text = currentDate.ToString("MMMM yyyy");
        DateTime previousMonth = currentDate.AddMonths(addMonth);
        date.text = previousMonth.ToString("MMMM yyyy");
        // ��ȡ��ǰ�µĵ�һ��
        DateTime firstDayOfMonth = new DateTime(previousMonth.Year, previousMonth.Month, 1);
        // �����һ�������ڼ�
        int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;

        // ��ȡ��ǰ�µ����һ��
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        // ������еĵ�Ԫ��
        //ClearGrid();

        // ����������Ԫ��
        int dayCounter = 1;
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                // ʵ������Ԫ��Ԥ����
                GameObject dayCell = Instantiate(calendar_item, calendar_group);
                dayCell.SetActive(true);
                // ���õ�Ԫ����ı�
                CalendarNode itemNode = dayCell.GetComponentInChildren<CalendarNode>();
                if (row == 0 && col < dayOfWeek)
                {
                    // ����ǵ�һ�У����ҵ�ǰ���ڵ�һ��֮ǰ������ʾ�ı�
                    itemNode.root.SetActive(false);
                }
                else if (dayCounter <= lastDayOfMonth.Day)
                {
                    DateTime counterDayOfMonth = new DateTime(previousMonth.Year, previousMonth.Month, dayCounter);
                    DateStringStorage storage = new DateStringStorage("DateStringStorage.txt");
                    string str = storage.RetrieveString(counterDayOfMonth);
                    // ��ʾ��ǰ����
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
                            Common.Instance.ShowTips("ʱ��δ��");
                            return;
                        }
                        DateStringStorage storage = new DateStringStorage("DateStringStorage.txt");
                        DateTime date = itemNode.counterDayOfMonth; // ���������
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
                                DateTime date1 = new DateTime(1, 1, 1); // ���������
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
                    // �����ǰ�����Ѿ������˵�ǰ�·ݵ����һ�죬����ʾ�ı�
                    itemNode.root.SetActive(false);
                }
            }
        }
    }

}