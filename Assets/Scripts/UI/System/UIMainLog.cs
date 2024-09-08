
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIMainLog : UIBase
{
    public Button bookshelf_btn;
    public Button record_btn;
    public Button close_btn;
    private DateTime currentDate;
    private bool isNewBie = false;
    public GameObject new_bie;
    public int isClickIndex = -1;
    public List<RectTransform> btnList = new List<RectTransform>();


    public override void OnAwake()
    {

    }
    public void OpenNewBie()
    {

        isNewBie = true;
        new_bie.SetActive(true);


    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        bookshelf_btn.onClick.AddListener(() =>
        {
          
          
            UILogEdit gui = UiManager.OpenUI<UILogEdit>("UILogEdit");
            if (isNewBie)
            {
                isClickIndex = 1;
                gui.OpenNewBie();
                new_bie.SetActive(false);
            }
     
            currentDate = DateTime.Today;
            DateTime currdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            DateStringStorage storage = new DateStringStorage("DateStringStorage.txt");
            string str = storage.RetrieveString(currdate);
            string words;
            int currId = -1;
            if (str != null)
            {
                words = str;
            }
            else
            {
                DateTime date1 = new DateTime(1, 1, 1); // 今天的日期
                string str1 = storage.RetrieveString(date1);
                if (str1 == null)
                {
                    log_wordscnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<log_wordscnofigData>("log_words", 1);
                    words = cfg.words;
                    currId = 1;
                }
                else
                {
                    int id = int.Parse(str1);
                    id++;
                    log_wordscnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<log_wordscnofigData>("log_words", id);
                    if (cfg != null)
                    {
                        words = cfg.words;
                        currId = id;
                    }
                    else
                    {
                        words = "";
                    }
                }
            }
            gui.SetData(currdate, words, currId, null);
     
        });
        close_btn.onClick.AddListener(CloseSelf);
        record_btn.onClick.AddListener(() =>
        {
         
            if (isNewBie)
            {
                if (isClickIndex == 1)
                {
                    isClickIndex = 0;
                }
                else
                {
                    isClickIndex = -1;
                }
                new_bie.SetActive(false);
            }
            UiManager.OpenUI<UILogList>("UILogList");


        });
        //等待配表
        UpdateBookMark();
    }
    private void UpdateBookMark()
    {
        List<int> ints = GetUniqueRandomNumbers(1, 20, 5);
        for (int i = 0; i < ints.Count; i++)
        {
            ShowBookMark(btnList[i], ints[i]);
        }
    }
    private void ShowBookMark(RectTransform rect,int cfgId)
    {
        TextMeshProUGUI title = rect.Find("logs_book_bones/Armature/BiaoQian/title").GetComponent<TextMeshProUGUI>();
        Button btn = rect.GetComponent<Button>();
        log_wordscnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<log_wordscnofigData>("log_words", 100 + cfgId);
        title.text = cfg.title;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            DateTime currdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            UILogEdit gui = UiManager.OpenUI<UILogEdit>("UILogEdit");
            gui.SetData(currdate, cfg.words, -1, null,false);
            gui.finish_btn.gameObject.SetActive(false);

        });
    }
    List<int> GetUniqueRandomNumbers(int min, int max, int count)
    {
        // 创建一个包含范围内所有数字的列表
        List<int> numbers = new List<int>();
        for (int i = min; i <= max; i++)
        {
            numbers.Add(i);
        }

        // 创建一个随机数生成器
        System.Random random = new System.Random();
        List<int> result = new List<int>();

        // 随机选出不重复的数字
        for (int i = 0; i < count; i++)
        {
            int index = random.Next(numbers.Count);
            result.Add(numbers[index]);
            numbers.RemoveAt(index);
        }

        return result;
    }
    public override void GoInUI()
    {
        if (isNewBie)
        {
            if (isClickIndex == 1)
            {
                new_bie.gameObject.SetActive(true);
                new_bie.transform.localPosition = new Vector3(record_btn.transform.localPosition.x + 50, record_btn.transform.localPosition.y-50,0);
            }
            else if (isClickIndex == -1)
            {
                new_bie.gameObject.SetActive(true);
            }
            else
            {

            }
        }
        this.gameObject.SetActive(true);
    }
    public override void OutUI()
    {
        this.gameObject.SetActive(false);
    }
}