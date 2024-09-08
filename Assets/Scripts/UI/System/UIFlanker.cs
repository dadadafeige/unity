
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class FlankerOneTimer
{
    public string m_name;
    private List<string> itemImagePool = new List<string>() { "flanker_image1", "flanker_image2", "flanker_image3", "flanker_image4" };
    private List<string> itemWordPool = new List<string>() { "flanker_word1", "flanker_word2", "flanker_word3", "flanker_word4" };
    private List<int> imageNums = new List<int>() { 1, 3, 5 };
    private int wordMaxNum = 4;
    public List<string> items = new List<string>();
    public List<string> words = new List<string>();
    private int rightIndex;
    // 创建 Random 对象
    System.Random random = new System.Random();
    public FlankerOneTimer(int diffLv)
    {
        if (diffLv == 1)
        {
            int randomNumber = UnityEngine.Random.Range(0, 3);
            rightIndex = UnityEngine.Random.Range(0, 4);
            for (int i = 0; i < imageNums[randomNumber]; i++)
            {
                items.Add(itemImagePool[rightIndex]);
            }

        }
        else if (diffLv == 2)
        {
            int randomNumber = UnityEngine.Random.Range(0, 2);
            rightIndex = UnityEngine.Random.Range(0, 4);

            int len = imageNums[randomNumber]/2;
            if (len != 0)
            {
                for (int i = 0; i < len; i++)
                {
                    int randomNumber1 = UnityEngine.Random.Range(0, 3);
                    items.Add(itemImagePool[randomNumber1]);
                    items.Add(itemImagePool[randomNumber1]);
                }
                items.Insert(len, itemImagePool[rightIndex]);
            }
            else
            {
                items.Add(itemImagePool[rightIndex]);
            }


        }
        else if (diffLv == 3)
        {
            int randomNumber = UnityEngine.Random.Range(0, 3);
            rightIndex = UnityEngine.Random.Range(0, 4);

            int len = imageNums[randomNumber]/2;
            if (len != 0)
            {
                for (int i = 0; i < len; i++)
                {
                    int randomNumber1 = UnityEngine.Random.Range(0, 4);
                    items.Add(itemImagePool[randomNumber1]);
                }
                int itemsLen = items.Count;
                for (int i = itemsLen - 1; i > -1; i--)
                {
                    items.Add(items[i]);
                }
                items.Insert(len, itemImagePool[rightIndex]);
            }
            else
            {
                items.Add(itemImagePool[rightIndex]);
            }
        }
        int wordRandomLen = UnityEngine.Random.Range(2, 5);
        wordRandomLen--;
       
        List<int> wordsLen = GenerateRandomNumbers(wordRandomLen, rightIndex);
        for (int i = 0; i < wordsLen.Count; i++)
        {
            words.Add(itemWordPool[wordsLen[i]]);
        }


    }
    public static List<int> GenerateRandomNumbers(int length,int rightIndex)
    {
        List<int> numbers = new List<int>();
        if (length <= 0)
        {
            return numbers;
        }

        // 添加数字 0 到 3 到列表中
        for (int i = 0; i <= 3; i++)
        {
            if (rightIndex != i)
            {
                numbers.Add(i);
            }
           
        }

        // 创建 Random 对象
        System.Random random = new System.Random();

        // 使用 Fisher-Yates 洗牌算法打乱数字列表
        for (int i = 0; i < numbers.Count; i++)
        {
            int randomIndex = random.Next(i, numbers.Count);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }
        // 截取指定长度的随机数列表
        List<int> result = numbers.GetRange(0, Math.Min(length, numbers.Count));
        int index = random.Next(0, result.Count + 1); // 随机生成插入位置的索引，范围是[0, list.Count]
        result.Insert(index, rightIndex);
        return result;
    }
    public bool CheckIsTrue(string str)
    {
        if (itemWordPool[rightIndex] == str)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
public class UIFlanker : UIBase
{
    public Image diff;
    public RectTransform group;
    public RectTransform btn_group;
    public Image pro;
    public TextMeshProUGUI flanker_right_num;
    public TextMeshProUGUI flanker_wrong_num;
    public TextMeshProUGUI schedule;
    public Button close_btn;
    public GameObject btn_item;
    public GameObject icon_item;
    private FlankerOneTimer oneTimer;
    private int currPace = 0;
    private int rightNum = 0;
    private int maxSubject = 10;
    private int wrongNum = 0;
    private int currDiff = 0;
    private float oneTime;
    private List<GameObject> iconItemObjList = new List<GameObject>();
    private List<GameObject> btnItemObjList = new List<GameObject>();
    private int condition = 5; //答对 condition 通关
    Tween tween;
    public Button rule_btn;
    public override void OnAwake()
    {

    }

    // Start is called before the first frame update
    public override void OnStart()
    {
        rule_btn.onClick.AddListener(() =>
        {
            if (tween != null)
            {
                tween.Pause();

            }

            MissionManage.ShowDescription(() =>
            {
                if (tween != null)
                {
                    tween.Play();

                }

            });

        });
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                UpdataDiff();
            });
        });
        close_btn.onClick.AddListener(CloseSelf);

    }
    private void CreateOneTimer(bool isAdd = true)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        if (currPace == maxSubject)
        {
            if (condition > rightNum)
            {
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(currDiff), () =>
                {
                    ReStart();


                }, () => { CloseSelf(); }, () =>
                {
                 

                });

                return;
            }
            UpdataDiff(isAdd);
            return;
        }
        currPace++;
        oneTimer = new FlankerOneTimer(3);
        ShowUI();
    }
    private void ReStart()
    {
        currPace = 0;
        rightNum = 0;
        maxSubject = 10;
        wrongNum = 0;
        if (currDiff == 1)
        {
            oneTime = 3;
            diff.sprite = UiManager.LoadSprite("common", "diff_1");
        }
        else if (currDiff == 2)
        {
            oneTime = 2;
            diff.sprite = UiManager.LoadSprite("common", "diff_3");
        }
        else if (currDiff == 3)
        {
            oneTime = 1.5f;
            diff.sprite = UiManager.LoadSprite("common", "diff_5");

        }
        ShowData();
    }
    private void ShowUI()
    {
        
        List<string> items = oneTimer.items;
        for (int i = 0; i < btnItemObjList.Count; i++)
        {
            btnItemObjList[i].SetActive(false);
        }
        for (int i = 0; i < iconItemObjList.Count; i++)
        {
            iconItemObjList[i].SetActive(false);
        }
        for (int i = 0; i < items.Count; i++)
        {
            GameObject go;

            if (iconItemObjList.Count > i)
            {
                go = iconItemObjList[i];
            }
            else
            {
                go = GameObject.Instantiate(icon_item);
                iconItemObjList.Add(go);
            }
            go.transform.SetParent(group);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            Image image = go.GetComponent<Image>();
            image.sprite = UiManager.LoadSprite("Flanker", items[i]);
        }
 
        List<string> words = oneTimer.words;
        for (int i = 0; i < words.Count; i++)
        {
            GameObject go;
            if (btnItemObjList.Count > i)
            {
                go = btnItemObjList[i];
            }
            else
            {
                go = GameObject.Instantiate(btn_item);
                btnItemObjList.Add(go);
            }
            go.transform.SetParent(btn_group);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            UpdateBtn(go, words[i]);
        }
        pro.fillAmount = 1;
        tween = pro.DOFillAmount(0, oneTime);
        tween.onComplete = () =>
        {
            wrongNum++;
            ShowData();
        };
    }
    private void UpdataDiff(bool isAdd = true)
    {
        if (currDiff == 3)
        {
          
            Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(currDiff), () =>
            {

                ReStart();


            }, () => { CloseSelf(); });
            return;
        }
        currPace = 0;
        rightNum = 0;
        maxSubject = 10;
        wrongNum = 0;
        if (isAdd)
        {
            currDiff++;

        }
     
      
        if (currDiff == 1)
        {
            ShowData();
            oneTime = 3;
            diff.sprite = UiManager.LoadSprite("common", "diff_1");
        }
        else if (currDiff == 2)
        {
            //Common.Instance.ShowBones("nandutisheng_bones", () =>
            //{
            //});
            
            Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(currDiff), () =>
            {

                ReStart();


            }, () => { CloseSelf(); }, () =>
            {
                oneTime = 2;
                diff.sprite = UiManager.LoadSprite("common", "diff_3");
                ShowData();

            });

        }
        else if (currDiff == 3) 
        {
            Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(currDiff), () =>
            {

                ReStart();

            }, () => { CloseSelf(); }, () =>
            {
                oneTime = 1.5f;
                diff.sprite = UiManager.LoadSprite("common", "diff_5");
                ShowData();

            });


        }
     

}
    private void UpdateBtn(GameObject go,string word)
    {
        Image image = go.transform.Find("word").GetComponent<Image>();
        Button btn = go.GetComponent<Button>();
        image.sprite = UiManager.LoadSprite("Flanker", word);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            bool isTrue = oneTimer.CheckIsTrue(word);
            if (isTrue)
            {
                rightNum++;
            }
            else
            {
                wrongNum++;
            }
            ShowData();
        });

    }
    private void ShowData(bool isAdd = true)
    {
        flanker_right_num.text = rightNum.ToString();
        flanker_wrong_num.text = wrongNum.ToString();
        schedule.text = currPace + "/" + maxSubject;
        CreateOneTimer(isAdd);
    }


}