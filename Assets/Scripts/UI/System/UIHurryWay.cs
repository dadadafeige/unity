using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HurryWayLevelData
{
    public float proTime;
    public Image levelGo;
    public bool isPass = false;
    public bool isColorPass = false;
    public DragonBonesController dragon = null;
    public RectTransform root = null;
    public bool isCurrColor = false;
    public RectTransform colorRoot = null;
    public GameObject color_go;
    public string colorName;
    public RectTransform color_lastScens;
    public int color_isToIndex;
}

public class UIHurryWay : UIBase
{
    public Image pro;
    public Button mask;
    private float duration = 60f;

    private float targetFillAmount;
    private float currentFillAmount;
    private float startTime;
    private List<float> eventTimes = new List<float>();
    public List<RectTransform> sceneList = new List<RectTransform>();
    public List<RectTransform> sceneList2 = new List<RectTransform>();
    private float moveSpeed = 100f;
    private float resetDistance = 1098f; // 假设这是屏幕右侧的位置  
    RectTransform isTo = null;
    private int isToIndex = 3;
    RectTransform lastScens = null;
    public Image progressBar;
    private List<HurryWayLevelData> levelDatas = new List<HurryWayLevelData>();
    public List<RectTransform>  wayList = new List<RectTransform>();
    public HurryWayPlayer player;
    public Button up_btn;
    public Button down_btn;
    public RectTransform color_root;
    public bool isFinish = false;
    private bool isPause = false;
    Dictionary<string, DragonBonesController> bonesMap = new Dictionary<string, DragonBonesController>();
    public RectTransform new_bie_root;
    public Button new_bie_btn;
    private bool is_new_bie = false;
    public TextMeshProUGUI new_bie_label;
    private float pause_time;
    private float diff_pause;
    public Button close_btn;
    private string[] colorNames = new string[7]
    {
        "hurry_way_logo1",
        "hurry_way_logo2",
        "hurry_way_logo3",
        "hurry_way_logo2",
        "hurry_way_logo3",
        "hurry_way_logo1",
        "hurry_way_logo2"

    };
    private List<GameObject> flagGoList = new List<GameObject>();
    public Button rule_btn;

    enum CharacterArea
    {
        Top,
        Middle,
        Bottom
    }
    public void Reset()
    {
        is_new_bie = true;
        for (int i = 0; i < levelDatas.Count; i++)
        {
            if (levelDatas[i].color_go != null)
            {
                GameObject.Destroy(levelDatas[i].color_go);

            }
            if (levelDatas[i].dragon != null)
            {
                GameObject.Destroy(levelDatas[i].dragon.gameObject);
            }
           
        }
        levelDatas.Clear();
        bonesMap.Clear();
        GenerateRandomEventTimes(3);
        progressBar.fillAmount = 0;
        startTime = Time.time;
        diff_pause = 0;
        targetFillAmount = 1;
        Shuffle<string>(ref colorNames);
        InitFlag();
        isFinish = false;

    }

    public override void OnStart()
    {
        // Generate three random event times
        GenerateRandomEventTimes(3);
        isPause = true;
        close_btn.onClick.AddListener(CloseSelf);
        if (GameManage.curChapter == 6)
        {
            for (int i = 0; i < sceneList.Count; i++)
            {

                if (i == 3)
                {
                    Image image = sceneList[i].GetComponent<Image>();
                    image.sprite = UiManager.getTextureSpriteByNmae("hurry_way_texture", "hurry_way_scene" + 5);
                }
                else
                {
                    Image image = sceneList[i].GetComponent<Image>();
                    image.sprite = UiManager.getTextureSpriteByNmae("hurry_way_texture", "hurry_way_scene" + (4 + i));
                }

            }
            Image[] images = color_root.GetComponentsInChildren<Image>();
            images[0].sprite = UiManager.LoadSprite("hurry_way", "hurry_way_color" + 6);
            images[1].sprite = UiManager.LoadSprite("hurry_way", "hurry_way_color" + 5);
            images[2].sprite = UiManager.LoadSprite("hurry_way", "hurry_way_color" + 4);
        }
       
       
        progressBar.fillAmount = 0;

        // 设置目标填充量
        targetFillAmount = 1;
        lastScens = sceneList2[3];
        up_btn.onClick.AddListener(MoveUp);
        down_btn.onClick.AddListener(MoveDown);
        Shuffle<string>(ref colorNames);
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                startTime = Time.time;
                isPause = false;
                InitFlag();
            });
        });
        //startTime = Time.time;
        //isPause = false;
        //InitFlag();
        rule_btn.onClick.AddListener(() =>
        {
            isPause = true;

            MissionManage.ShowDescription(() =>
            {
                isPause = false;

            });

        });

        mask.onClick.AddListener(CloseSelf);
        if (!is_new_bie)
        {
            new_bie_root.gameObject.SetActive(false);
            new_bie_btn.onClick.AddListener(() =>
            {
                diff_pause += Time.time - pause_time;
                isPause = false;
                new_bie_root.gameObject.SetActive(false);
            });

        }
       
    }
    void InitFlag()
    {
        foreach (KeyValuePair<string,DragonBonesController> item in bonesMap)
        {
            item.Value.gameObject.SetActive(false);
        }
        for (int i = 0; i < flagGoList.Count; i++)
        {
            flagGoList[i].SetActive(false);
        }
        for (int i = 0; i < eventTimes.Count; i++)
        {
            float time = eventTimes[i];
            // Calculate the position of the event on the progress bar
            float eventPosition = Mathf.Lerp(0f, 1f, time);
            RectTransform progressBarRect = progressBar.rectTransform;
            GameObject newObject;
            Image newImage;
            if (flagGoList.Count > i)
            {
                newObject = flagGoList[i];
                newImage = newObject.GetComponent<Image>();
                newImage.gameObject.SetActive(true);
            }
            else
            {
                newObject = new GameObject("NewImageObject");
                newImage = newObject.AddComponent<Image>();
                flagGoList.Add(newObject);
            }
            newObject.transform.SetParent(progressBarRect);
            newObject.transform.localScale = Vector3.one;
            // 添加 Image 组件到新的游戏对象上
            newImage.rectTransform.anchorMax = new Vector2(0, 0.5f);
            newImage.rectTransform.anchorMin = new Vector2(0, 0.5f);
            newImage.rectTransform.pivot = new Vector2(0, 0.5f);
            Vector2 newPosition = new Vector2(eventPosition * 1100, 0);
            newImage.sprite = UiManager.LoadSprite("hurry_way", "hurry_way_logo3");
            newImage.SetNativeSize();
            newImage.rectTransform.anchoredPosition = newPosition;
            newImage.sprite = UiManager.LoadSprite("hurry_way", colorNames[i]);
            HurryWayLevelData levelData = new HurryWayLevelData();
            levelData.proTime = time;
            levelData.levelGo = newImage;
            levelData.colorName = colorNames[i];
            levelDatas.Add(levelData);
        }
    }
    void Update()
    {
        if (isFinish)
        {
            return;
        }

        if (isPause)
        {
            return;
        }
        
        for (int i = 0; i < sceneList.Count; i++)
        {
            RectTransform scene = sceneList[i];
            if (scene.anchoredPosition.x <= -resetDistance)
            {
                // 将场景重置到屏幕右侧，确保位置是整数  
                scene.anchoredPosition = new Vector2(resetDistance, scene.anchoredPosition.y);
                isTo = scene;
                isToIndex = i;
                lastScens = sceneList2[i];
                for (int z = 0; z < levelDatas.Count; z++)
                {
                    if (levelDatas[z].isCurrColor)
                    {
                        RectTransform rect = scene.transform.GetComponent<RectTransform>();

                        if (levelDatas[z].color_isToIndex == isToIndex)
                        {
                            levelDatas[z].colorRoot = null;
                            levelDatas[z].isCurrColor = false;
                            levelDatas[z].color_go.SetActive(false);
                        }
                    }
                    if (levelDatas[z].dragon != null)
                    {
                        if (levelDatas[z].root == isTo)
                        {
                            levelDatas[z].dragon.gameObject.SetActive(false);
                            levelDatas[z].dragon = null;
                        }
                    }
                }
                scene.transform.SetAsLastSibling();
            }
        }
        
        // 计算当前时间相对于起始时间的插值
        float elapsedTime = Time.time - startTime - diff_pause;
        // 计算当前填充量的插值
        currentFillAmount = Mathf.Lerp(0f, targetFillAmount, elapsedTime / duration);
        if (currentFillAmount >= 1)
        {
            Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () =>
            {
                Reset();
            }, () => { CloseSelf(); }, () =>
            {


            });
            isFinish = true;
            return;
        }
        // 更新UI的填充量
        progressBar.fillAmount = currentFillAmount;
        for (int i = 0; i < levelDatas.Count; i++)
        {
            if (!levelDatas[i].isPass)
            {
                if (levelDatas[i].proTime <= elapsedTime / duration)
                {
                    if (i == 0 && !is_new_bie)
                    {
                        new_bie_label.text = "主角需要跟阻挡物同样的颜色方可通过";
                        DelayedActionProvider.Instance.DelayedAction(() => { new_bie_root.gameObject.SetActive(true); isPause = true; pause_time = Time.time; }, 4);
                    }
                    string bonesName = "hong_hurry_way";
                    if (levelDatas[i].colorName == "hurry_way_logo1")
                    {
                        bonesName = "an_hurry_way";
                    }
                    else if (levelDatas[i].colorName == "hurry_way_logo3")
                    {
                        bonesName = "jin_hurry_way";
                    }
                    DragonBonesController dragon;
                    //if (bonesMap.ContainsKey(bonesName))
                    //{
                    //    dragon = bonesMap[bonesName];
                    //}
                    //else
                    //{
                    //    dragon = UiManager.LoadBonesByNmae(bonesName);
                    //    bonesMap.Add(bonesName, dragon);
                    //}
                    dragon = UiManager.LoadBonesByNmae(bonesName);
                   // bonesMap.Add(bonesName, dragon);
                    dragon.gameObject.SetActive(true);
                    dragon.transform.SetParent(transform);
                    dragon.transform.localPosition = new Vector3(1122, 0, 0);
                    dragon.transform.localScale = Vector3.one;
                    levelDatas[i].isPass = true;
                    dragon.transform.SetParent(lastScens.transform);
                    levelDatas[i].root = sceneList[isToIndex].transform.GetComponent<RectTransform>();
                    levelDatas[i].dragon = dragon;

                }
            }
            if (!levelDatas[i].isColorPass)
            {
               
                if (levelDatas[i].proTime - 0.02f <= elapsedTime / duration)
                {
                    if (i == 0 && !is_new_bie)
                    {
                        new_bie_label.text = "接取不同颜色的道具，主角团会变成对应的颜色";
                        DelayedActionProvider.Instance.DelayedAction(() => { new_bie_root.gameObject.SetActive(true); isPause = true; pause_time = Time.time; }, 2);
                        
                    }
                    GameObject go = GameObject.Instantiate(color_root.gameObject);
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(905, 0, 0);
                    go.transform.localScale = Vector3.one;
                    levelDatas[i].isColorPass = true;
                    go.transform.SetParent(lastScens.transform);
                    go.gameObject.SetActive(true);
                    levelDatas[i].isCurrColor = true;
                    levelDatas[i].colorRoot = sceneList[isToIndex].transform.GetComponent<RectTransform>();
                    levelDatas[i].color_go = go;
                    levelDatas[i].color_lastScens = lastScens;
                    levelDatas[i].color_isToIndex = isToIndex;
                    
                }
            }
            
            if (levelDatas[i].dragon != null)
            {
                //if (levelDatas[i].root == isTo)
                //{
                //    levelDatas[i].dragon.gameObject.SetActive(false);
                //    levelDatas[i].dragon = null;
                //}
            }
            if (levelDatas[i].isCurrColor)
            {
                if (levelDatas[i].colorRoot == isTo)
                {
                  
                }
            }
        }
        // 如果当前填充量达到目标填充量，重新设置目标填充量和起始时间
    

    }
    public void Shuffle<T>(ref T[] array)
    {
        System.Random rng = new System.Random();
    
        // 从最后一个元素开始，依次向前遍历数组
        for (int i = array.Length - 1; i > 0; i--)
        {
            // 生成一个随机索引，范围是[0, i+1)
            int randomIndex = rng.Next(i + 1);

            // 交换当前位置的元素与随机位置的元素
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    void GenerateRandomEventTimes(int count)
    {
        eventTimes.Clear();
        float range1 = Random.Range(2, 12); // 第一份范围为10到24
        float range2 = Random.Range(17, 27); // 第二份范围为25到39
        float range3 = Random.Range(32, 42); // 第三份范围为40到54
        float range4 = Random.Range(47, 57); // 第四份范围为55到69
        float range5 = Random.Range(62, 72); // 第五份范围为70到84
        float range6 = Random.Range(77, 87); // 第六份范围为85到89
        float range7 = Random.Range(92, 97); // 第六份范围为85到89

        // 从每个范围中生成一个随机数
        float randomNumber1 = Random.Range(range1, range1 + 1);
        float randomNumber2 = Random.Range(range2, range2 + 1);
        float randomNumber3 = Random.Range(range3, range3 + 1);
        float randomNumber4 = Random.Range(range4, range4 + 1);
        float randomNumber5 = Random.Range(range5, range5 + 1);
        float randomNumber6 = Random.Range(range6, range6 + 1);
        float randomNumber7 = Random.Range(range7, range7 + 1);
        eventTimes.Add(randomNumber1/100);
        eventTimes.Add(randomNumber2/100);
        eventTimes.Add(randomNumber3/100);
        eventTimes.Add(randomNumber4 / 100);
        eventTimes.Add(randomNumber5 / 100);
        eventTimes.Add(randomNumber6 / 100);
        eventTimes.Add(randomNumber7 / 100);
        //  eventTimes.Add(0.85f);

        //for (int i = 0; i < count; i++)
        //{
        //    float randomTime = Random.Range(0.1f, 0.75f); // Random time between 0.2 and 1 (representing start and end of timeline)
        //    while (eventTimes.Exists(t => Mathf.Abs(t - randomTime) < 0.15f))
        //    {
        //        randomTime = Random.Range(0.1f, 0.75f); // Ensure unique event times with a minimum interval of 0.15
        //    }

        //}
        eventTimes.Sort(); // Sort event times in ascending order
    }
    private void LateUpdate()
    {
        if (isFinish)
        {
            return;
        }
        if (isPause)
        {
            return;
        }
        MoveScenes();
    }
    void MoveScenes()
    {
        foreach (RectTransform scene in sceneList)
        {
            //if (isTo == null)
            //{
                // 计算当前帧应该移动的距离  
                float moveDistance = moveSpeed * Time.deltaTime*4;

                // 将移动距离转换为整数，确保没有小数部分  
                float moveDistanceInt = moveDistance;

                // 向左移动场景，并确保位置是整数  
                scene.anchoredPosition += new Vector2(-moveDistanceInt, 0);
          //  }
          
           
            //// 检查场景是否移出屏幕左侧  
            //if (scene.anchoredPosition.x <= -resetDistance)
            //{
            //    // 将场景重置到屏幕右侧，确保位置是整数  
            //    scene.anchoredPosition = new Vector2(resetDistance, scene.anchoredPosition.y);
            //}
        }
        for (int i = 0; i < sceneList2.Count; i++)
        {
            sceneList2[i].anchoredPosition = sceneList[i].anchoredPosition;
        }
       
        if (isTo != null)
        { // 计算当前帧应该移动的距离  
            RectTransform last = null;
            for (int i = 0; i < sceneList.Count; i++)
            {
                if (isTo == sceneList[i])
                {
                    if (last == null)
                    {
                        last = sceneList[3];
                    }
                    break;
                }
                last = sceneList[i];
            }
            float moveDistance = moveSpeed * Time.deltaTime;

            // 将移动距离转换为整数，确保没有小数部分  
            float moveDistanceInt = moveDistance;
            float dis = isTo.localPosition.x - last.localPosition.x - 549;
            // 向左移动场景，并确保位置是整数  
            isTo.anchoredPosition += new Vector2(-dis, 0);
            isTo = null;
        }
    }


    CharacterArea currentArea = CharacterArea.Middle; // 记录当前角色所在的区域

    // 角色向上移动的方法
    public void MoveUp()
    {
        if (currentArea != CharacterArea.Top)
        {
            currentArea = (CharacterArea)((int)currentArea - 1);
            UpdateUI();
        }
    }

    // 角色向下移动的方法
    public void MoveDown()
    {
        if (currentArea != CharacterArea.Bottom)
        {
            currentArea = (CharacterArea)((int)currentArea + 1);
            UpdateUI();
        }
    }

    // 更新 UI 显示
    void UpdateUI()
    {
        switch (currentArea)
        {
            case CharacterArea.Top:
                player.transform.SetParent(wayList[0]);
                break;
            case CharacterArea.Middle:
                player.transform.SetParent(wayList[1]);
                break;
            case CharacterArea.Bottom:
                player.transform.SetParent(wayList[2]);
                break;
        }
        player.transform.localPosition = Vector3.zero;
    }
}
