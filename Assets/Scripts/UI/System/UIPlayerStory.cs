using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

enum ROLEINDEX {
    LEFT = 1,
    RIGHT = 2,
    MIDDLE = 3


}
class SpineData
{
    public string spineNmae;
    public string roleName;
    public int index;
    public SpineData(string spineStructure)
    {
        string[] strArr = spineStructure.Split(",");
        Debug.Log(spineStructure);
        spineNmae = strArr[0];
        roleName = strArr[1];
        index = int.Parse(strArr[2]) - 1;
    }



}

public class UIPlayerStory : UIBase, IPointerClickHandler
{
    public TextMeshProUGUI talkText;
    public Button clickBtn;
    public RawImage[] mSpine;
    public RectTransform[] mSpineRoot;
    public TextMeshProUGUI mName;
    public TextMeshProUGUI mName2;
    public GameObject option;
    public RawImage bg;
    public GameObject optionGroup;
    public MissionNode missioNode;
    private ChapterNode curChapter;
    public PlotNode curPlot;
    private List<GameObject> optionObjList = new List<GameObject>();
    private Dictionary<string, DragonBonesController> bonesMap = new Dictionary<string, DragonBonesController>();
    public Button quitBtn;
    public RectTransform textBgTran;
    private RectTransform mTran;
    public GameObject mask;
    public GameObject inputRoot;
    // public InputField inputField;
    public Button inputBtn;
    public TextMeshProUGUI inputLabel;
    public GameObject tipsLabel;
    public GameObject hang;
    public GameObject spineRoot;
    private List<DragonBonesController> spineList = new List<DragonBonesController>();
    private List<string> spineNameList = new List<string>();
    public Button bag_btn;
    public Button role_btn;
    public Button map_btn;
    public Button addBtn;
    public Button chapter_btn;
    public Button gameList;
    public Button draw_btn;
    public Button daily_btn;
    public Button logs_btn;
    public Button setup_btn;
    public Button psychological_btn;
    public PlayerTopNode top_item;
    public override void OnAwake()
    {
        Debug.Log(GameManage.curMissionId);
        GameManage.curMissionId = GameManage.userData.unlockMissionId;
        GameManage.curGameMissionId = GameManage.curMissionId;
        GameManage.curChapter = GameManage.userData.unlockChapter;
        int missionId = GameManage.userData.unlockMissionId;
        List<PlotNode> plots;
        for (; ; )
        {
            missioNode = MissionManage.GetMissionNodeById(missionId);
            plots = missioNode.GetPlotList();
            if (plots.Count > 0)
            {
                break;
            }
            else
            {
                missionId++;
            }
        }
        GameManage.userData.is_first = false;
        curPlot = plots[0];
        missioNode.curPlot = curPlot;
        mTran = gameObject.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        AdapterAnim();

        ShowUI();
        clickBtn.onClick.AddListener(ClickOnButton);
        quitBtn.onClick.AddListener(() => { CloseSelf(); });
        bag_btn.onClick.AddListener(() => { Common.Instance.ShowBag(); });
        map_btn.onClick.AddListener(OpenMap);
        draw_btn.onClick.AddListener(() => { UiManager.OpenUI<UIGuide>("UIGuide"); });
        role_btn.onClick.AddListener(() =>
        {
            UiManager.OpenUI<UIRoleInfo>("UIRoleInfo");

        });
        logs_btn.onClick.AddListener(() =>
        {
            UiManager.OpenUI<UIMainLog>("UIMainLog");

        });
        daily_btn.onClick.AddListener(() =>
        {
            UiManager.OpenUI<UIGameList>("UIGameList");

        });
        setup_btn.onClick.AddListener(() =>
        {
            UISetUp uIMain = UiManager.OpenUI<UISetUp>("UISetUp");

        });
        psychological_btn.onClick.AddListener(() =>
        {
            Common.Instance.ShowTips("敬请期待");

        });


        chapter_btn.onClick.AddListener(() =>
        {
            UIChapterList mui =  UiManager.OpenUI<UIChapterList>("UIChapterList");
            mui.SetData(this);
        });
        addBtn.onClick.AddListener(() =>
        {
            Dictionary<int, itmeconfigData> itemCfg = GetCfgManage.Instance.GetCfgByName<itmeconfigData>("item");
            foreach (itmeconfigData item in itemCfg.Values)
            {
                string str = item.id.ToString() + ",10000";
                BagManage.Instance.Add(str);
            }

        });
        gameList.onClick.AddListener(() =>
        {

            UiManager.OpenUI<UIGameList>("UIGameList");

        });
        if (!GameManage.userData.is_first)
        {
            InitBtn();
        }
    }
    public void InitBtn()
    {
        UiManager.uIPlayer.map_btn.gameObject.GetComponent<Image>().sprite = UiManager.getTextureSpriteByNmae("map_btn_texture", "map_btn" + GameManage.userData.unlockChapter);

        bag_btn.gameObject.SetActive(GameManage.userData.unlockMissionId > 3);
        role_btn.gameObject.SetActive(GameManage.userData.unlockMissionId > 15);
        top_item.gold.gameObject.SetActive(GameManage.userData.unlockMissionId > 2);
        draw_btn.gameObject.SetActive(GameManage.userData.unlockMissionId > 14);
        logs_btn.gameObject.SetActive(GameManage.userData.unlockMissionId > 25);
        daily_btn.gameObject.SetActive(GameManage.userData.unlockMissionId > 4);
        
        //   role_btn.gameObject.SetActive(GameManage.userData.unlockMissionId > 13);
    }
    private void OpenMap()
    {

        UIMap uIMap = UiManager.OpenUI<UIMap>("UIMap");
        uIMap.SetDataEx(this);
    }
    private void ClickOnButton()
    {
        if (curPlot != null)
        {
            curPlot = curPlot.nextPlotNode;
            missioNode.curPlot = curPlot;
        }
        ShowUI();

    }
    private void AdapterAnim()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float pro = screenHeight / 1080;
        for (int i = 0; i < mSpineRoot.Length; i++)
        {
            mSpineRoot[i].anchoredPosition = new Vector2(mSpineRoot[i].anchoredPosition.x * pro, mSpineRoot[i].anchoredPosition.y);
            mSpineRoot[i].localScale = new Vector3(pro, pro, pro);
        }
       
    }
    public void ShowUI()
    {

        if (curPlot != null)
        {
            plotconfigData cfg = curPlot.plotCfg;
            scenecnofigData sceneCfg = missioNode.sceneNode.mCfg;
            string str = cfg.words;
            str = ReplacePlaceholder(str, "#主角#", GameManage.userData.userName);
            if (GameManage.userData.userGender == Gender.Boy)
            {
                str = ReplacePlaceholder(str, "哥哥/姐姐", "哥哥");
            }
            else
            {
                str = ReplacePlaceholder(str, "哥哥/姐姐", "姐姐");
            }
            talkText.text = str;
            talkText.ForceMeshUpdate();
            Vector3 vector = GetLastCharacterPosition();
            tipsLabel.transform.position = vector;
           
            for (int i = 0; i < mSpineRoot.Length; i++)
            {

                mSpineRoot[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < spineList.Count; i++)
            {
                PushSpinePool(spineList[i], spineNameList[i]); 
            }

            spineList.Clear();
            spineNameList.Clear();
            if (cfg.spineName != string.Empty)
            {
                string[] rolArr = cfg.spineName.Split("|");
                for (int i = 0; i < rolArr.Length; i++)
                {
                    SpineData spineData = new SpineData(rolArr[i]);
                    DragonBonesController dragon = PopSpinePool(spineData.spineNmae, mSpineRoot[spineData.index]);
                    if (dragon == null)
                    {
                        string spineNmae = spineData.spineNmae;
                        if (spineNmae == "main"&& GameManage.userData.userGender == Gender.Girl)
                        {
                            spineNmae = "heroine";
                        }
                        if (spineNmae == "main_young" && GameManage.userData.userGender == Gender.Girl)
                        {
                            spineNmae = "heroine_young";
                        }
                        Texture texture = UiManager.getTextureByNmae(spineNmae);
                        float screenHeight = Screen.height;
                        float needHeight = 1080 / 2;
                        float pro = needHeight / texture.height;
                        float needWidth = texture.width * pro;
                        mSpine[spineData.index].texture = texture;
                        mSpine[spineData.index].gameObject.name = spineData.spineNmae;
                        RectTransform rect = mSpine[spineData.index].GetComponent<RectTransform>();
                       // rect.sizeDelta = new Vector2(needWidth, needHeight);
                        //  mSpine[spineData.index].SetNativeSize();
                        mSpine[spineData.index].gameObject.SetActive(true);
                    }
                    else
                    {
                        spineList.Add(dragon);
                        mSpine[spineData.index].gameObject.SetActive(false);
                        spineNameList.Add(spineData.spineNmae);
                    }
                    string roleName = spineData.roleName;
                    if (roleName == "主角")
                    {
                        roleName = GameManage.userData.userName;
                    }
                    mName.text = roleName;
                    mName2.text = roleName;
                    mSpineRoot[spineData.index].gameObject.SetActive(true);
                }





            }
            else
            {
                mName.text = "旁白";
                mName2.text = "旁白";
            }

         
            //  PopSpinePool("spine_1", mSpine.transform);
            bg.texture = UiManager.getTextureByNmae(cfg.bjName);
            AudioManager.Instance.PlaySound(cfg.sound);
            if (sceneCfg.BMG != "" && sceneCfg.BMG != AudioManager.Instance.currBGMName)
            {

                AudioManager.Instance.currBGMName = sceneCfg.BMG;
                AudioManager.Instance.PlayBGM(sceneCfg.BMG);

            }
        }
        else
        {
            if (missioNode.missionState == MISSIONSTATE.RECEIVE)
            {
                missioNode.EnterWork(() =>
                {

                    List<PlotNode> plots = missioNode.GetPlotList();
                    if (plots.Count > 0)
                    {
                        curPlot = plots[0];
                        missioNode.curPlot = curPlot;
                    }
                    ShowUI();
                },this);
            
            }
            else if (missioNode.missionState == MISSIONSTATE.FINISH)
            {
                missioNode.missionState = MISSIONSTATE.DAILY;
                MissionManage.SaveItems();
                if (missioNode.mCfg.reward != "")
                {
                    BagManage.Instance.Add(missioNode.mCfg.reward);
                }
                
                GameManage.curMissionId++;
                missioNode = MissionManage.GetMissionNodeById(GameManage.curMissionId);
                GameManage.curChapter = missioNode.mCfg.chapter;
                GameManage.curGameMissionId = GameManage.curMissionId;
                List<PlotNode> plots = missioNode.GetPlotList();
                curPlot = plots[0];
                missioNode.curPlot = curPlot;
                GameManage.userData.SetUnlock(GameManage.curMissionId, GameManage.curChapter, () =>
                {
                    ShowUI();

                    
                });
             
            }
            else if (missioNode.missionState == MISSIONSTATE.UNFINISH)
            {
                missioNode.EnterWork(() =>
                {
                    List<PlotNode> plots = missioNode.GetPlotList();
                    if (plots.Count > 0)
                    {
                        curPlot = plots[0];
                        missioNode.curPlot = curPlot;
                    }
                    ShowUI();
                },this);
            }
            else if (missioNode.missionState == MISSIONSTATE.DAILY)
            {
                if (missioNode.mCfg.isGame > 0)
                {
                    missioNode.EnterWork(() =>
                    {
                        List<PlotNode> plots = new List<PlotNode>();
                        for (; ; )
                        {
                            if (GameManage.isMap)
                            {
                                GameManage.isMap = false;
                                GameManage.curMissionId = GameManage.userData.unlockMissionId;
                            }
                            else
                            {
                                if (missioNode.mCfg.id == GameManage.curMissionId)
                                {
                                    GameManage.curMissionId++;
                                    GameManage.curGameMissionId = GameManage.curMissionId;
                                }

                            }
                            GameManage.curGameMissionId = GameManage.curMissionId;
                            missioNode = MissionManage.GetMissionNodeById(GameManage.curMissionId);
                            plots = missioNode.GetPlotList();
                            if (plots.Count>0)
                            {
                                break;
                            }
                        }
                        
                        curPlot = plots[0];
                        missioNode.curPlot = curPlot;
                        curPlot = missioNode.curPlot;
                        ShowUI();
                    }, this);
                }
                else
                {
                
                    List<PlotNode> plots = new List<PlotNode>();
                    for (; ; )
                    {
                        if (missioNode.mCfg.id == GameManage.curMissionId)
                        {
                            GameManage.curMissionId++;
                            GameManage.curGameMissionId = GameManage.curMissionId;
                        }
                        missioNode = MissionManage.GetMissionNodeById(GameManage.curMissionId);
                        plots = missioNode.GetPlotList();
                        if (plots.Count>0)
                        {
                            break;
                        }
                    }
                    curPlot = plots[0];
                    missioNode.curPlot = curPlot;
                    curPlot = missioNode.curPlot;
                    ShowUI();
                }
                
                
            }
        };
       
   //     mSpine.SetNativeSize();

    }
    string ReplacePlaceholder(string originalText, string placeholder, string replacement)
    {
        // 使用 Replace 方法进行替换
        return originalText.Replace(placeholder, replacement);
    }
    Vector3 GetLastCharacterPosition()
    {
        TMP_TextInfo textInfo = talkText.textInfo;

        if (textInfo.characterCount == 0)
        {
            Debug.LogWarning("Text is empty!");
            return Vector3.zero;
        }

        int lastCharacterIndex = textInfo.characterCount - 1;

        TMP_CharacterInfo lastCharacterInfo = textInfo.characterInfo[lastCharacterIndex];
        Vector3 lastCharacterPosition = lastCharacterInfo.bottomRight;
        // 获取最后一个字符的中心位置
        Vector3 lastCharCenter = (lastCharacterInfo.bottomLeft + lastCharacterInfo.topRight) * 0.5f;

        // 将世界坐标转换为本地坐标
        Vector3 lastCharPosition = talkText.transform.TransformPoint(lastCharacterPosition);
        string text = talkText.text;
        if (!string.IsNullOrEmpty(text) && text[text.Length - 1] == '”')
        {
            lastCharPosition = new Vector3(lastCharPosition.x, lastCharPosition.y - 0.15f, lastCharPosition.z);
        }
        return new Vector3(lastCharPosition.x + 0.1f, lastCharPosition.y, lastCharPosition.z);
    }
    private DragonBonesController PopSpinePool(string spineName, RectTransform parent)
    {
      
     
        DragonBonesController dragonBonesController;
        if (bonesMap.ContainsKey(spineName))
        {
            dragonBonesController = bonesMap[spineName];
            dragonBonesController.gameObject.SetActive(true);
            dragonBonesController.PlayAnimation(dragonBonesController.armatureComponent.animation.animationNames[0], true);

        }
        else
        {
            dragonBonesController = UiManager.LoadBonesByNmae(spineName);
        
            if (dragonBonesController == null)
            {
                return null;
            }
            dragonBonesController.gameObject.name = spineName;

        }
        GameObject go = dragonBonesController.gameObject;
        go.transform.SetParent(parent);
        RectTransform rect = go.GetComponent<RectTransform>();
        go.transform.localScale = new Vector3(1, 1, 1);
        rect.anchoredPosition = Vector2.zero;

        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;
        //float pro = screenWidth / 1920;
        //RectTransform rect = go.GetComponent<RectTransform>();
        //rect.anchorMax = parent.anchorMax;
        //rect.anchorMin = parent.anchorMin;
        //rect.pivot = parent.pivot;
     
    
        go.transform.SetAsFirstSibling();
        // bonesMap.Add(spineName, go);
        return dragonBonesController;

    }
    private void PushSpinePool(DragonBonesController go, string spineName)
    {
        if (!bonesMap.ContainsKey(spineName))
        {
            bonesMap.Add(spineName, go);
        }
        go.gameObject.SetActive(false);
    }

    public override void GoInUI()
    {
    
         textBgTran.gameObject.SetActive(true);
        talkText.ForceMeshUpdate();
        Vector3 vector = GetLastCharacterPosition();
        tipsLabel.transform.position = vector;
        //spineRoot.SetActive(true);
    }
    public override void OutUI()
    {
        textBgTran.gameObject.SetActive(false);
        AudioManager.Instance.StopSound();
       // spineRoot.SetActive(false);
    }
    public class optionData
    {
        public int languageId;
        public int linkId;
        public optionData(int languageId,int linkId)
        {
            this.languageId = languageId;
            this.linkId = linkId;


        }


    }
    private Color defaultLinkColor = Color.blue;
    // 实现 IPointerClickHandler 接口
    public void OnPointerClick(PointerEventData eventData)
    {


        TMP_Text tMP_ = talkText.GetComponent<TMP_Text>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tMP_, eventData.position, eventData.pressEventCamera);

        if (linkIndex != -1)
        {   // 处理链接点击事件

            TMP_TextInfo textInfo = talkText.textInfo;
            UINotice notice = UiManager.OpenUI<UINotice>("UINotice");
            string pattern = @"\d+";
            Match match = Regex.Match(textInfo.linkInfo[0].GetLinkID(), pattern);
            Debug.Log($"Clicked on link: {match.Value}. URL: {textInfo.linkInfo[0].GetLinkText()}");
            notice.SetLinkWords(int.Parse(match.Value));
            return; // 可以根据需要中断循环

        }



        clickBtn.onClick.Invoke();




        return;
        //// 将点击位置转换为文本坐标
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(talkText.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        //// 遍历链接检查点击位置
        //TMP_TextInfo textInfo = talkText.textInfo;
        //for (int i = 0; i < textInfo.linkCount; i++)
        //{
        //    TMP_LinkInfo linkInfo = textInfo.linkInfo[i];
        //    int linkStartIndex = linkInfo.linkTextfirstCharacterIndex;
        //    int linkEndIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1;

        //    // 获取链接的范围
        //    TMP_CharacterInfo charInfoStart = textInfo.characterInfo[linkStartIndex];
        //    TMP_CharacterInfo charInfoEnd = textInfo.characterInfo[linkEndIndex];

        //    float linkWidth = charInfoEnd.bottomRight.x - charInfoStart.topLeft.x;
        //    float linkHeight = charInfoStart.topLeft.y - charInfoStart.bottomLeft.y;

        //    Rect linkRect = new Rect(charInfoStart.topLeft.x, charInfoStart.bottomLeft.y, linkWidth, linkHeight);

        //    if (linkRect.Contains(localPoint))
        //    {
        //        // 处理链接点击事件
            
        //        UINotice notice =  UiManager.OpenUI<UINotice>("UINotice");
        //        string pattern = @"\d+";
        //        Match match = Regex.Match(linkInfo.GetLinkID(), pattern);
        //        Debug.Log($"Clicked on link: {match.Value}. URL: {linkInfo.GetLinkText()}");
        //        notice.SetLinkWords(int.Parse(match.Value));
        //        return; // 可以根据需要中断循环
        //    }
        //}
       // clickBtn.onClick.Invoke();
    }
}
    // Update is called once per frame

