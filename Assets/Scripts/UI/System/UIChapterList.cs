using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIChapterList : UIBase
{
    public RectTransform group;
    public RectTransform grid;
    public Button left_btn;
    public Button right_btn;
    private int currIndex = 0;
    Tween tween;
    public Button make;
    public List<RectTransform> rects;
    private UIPlayerStory uIPlayer;
    private int tabIndex = 1;
    int[] array = { 0,1,2};
    chapterconfigData[] cfgArr = new chapterconfigData[9];
    public override void OnAwake()
    {
        InitData();
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        InitUI();
        grid.sizeDelta = new Vector2((703 + 10) * 9, 630);
        left_btn.onClick.AddListener(OnClickLeft);
        right_btn.onClick.AddListener(OnClickRight);
        make.onClick.AddListener(CloseSelf);
    }
    public void SetData(UIPlayerStory uIPlayer)
    {
        this.uIPlayer = uIPlayer;
    }
   private void InitData()
    {

        for (int i = 0; i < cfgArr.Length; i++)
        {
            chapterconfigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<chapterconfigData>("chapter", i + 1);
            cfgArr[i] = cfg;
        }

    }
    private void InitUI()
    {
        RawImage image = rects[0].GetComponent<RawImage>();
        TextMeshProUGUI proUGUI = rects[0].Find("lock").GetComponent<TextMeshProUGUI>();
        Button btn = rects[array[0]].GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            if (1 == GameManage.curChapter)
            {

                CloseSelf();
                return;
            }
            GameManage.curMissionId = 1;
            GameManage.curGameMissionId = GameManage.curMissionId;
            GameManage.curChapter = 1;
            uIPlayer.missioNode = MissionManage.GetMissionNodeById(1);
            List<PlotNode> plots = uIPlayer.missioNode.GetPlotList();
            uIPlayer.curPlot = plots[0];
            // uIPlayer.missioNode.curPlot = uIPlayer.curPlot;
            uIPlayer.ShowUI();
            CloseSelf();
            UiManager.uIPlayer.map_btn.gameObject.GetComponent<Image>().sprite = UiManager.getTextureSpriteByNmae("map_btn_texture", "map_btn" + 1);


        });
        if (1 > GameManage.userData.unlockChapter)
        {
            proUGUI.gameObject.SetActive(true);
            proUGUI.text = Common.Instance.big_digit_word_map[1];
            image.texture = UiManager.getTextureByNmae("chapter_list_texture", "chapter_list_lock");
        }
        else
        {
            proUGUI.gameObject.SetActive(false);
            image.texture = UiManager.getTextureByNmae("chapter_list_texture", cfgArr[0].chapter_list_image);

        }


        RawImage image1 = rects[1].GetComponent<RawImage>();
        TextMeshProUGUI proUGUI1 = rects[1].Find("lock").GetComponent<TextMeshProUGUI>();
        Button btn1 = rects[array[1]].GetComponent<Button>();
        btn1.onClick.AddListener(() =>
        {
            if (2 > GameManage.userData.unlockChapter)
            {
                CloseSelf();
                Common.Instance.ShowTips("章节未解锁");
                return;
            }
            if (2 == GameManage.curChapter)
            {
                CloseSelf();
                return;
            }

            GameManage.curChapter = 2;
            List<PlotNode> plots;
            int missionId = 0;
            int needChapter;
            for (; ; )
            {
                missionId++;

                MissionNode mission = MissionManage.GetMissionNodeById(missionId);
                plots = mission.GetPlotList();
                needChapter = mission.mCfg.chapter;
                if (needChapter == 2 && plots.Count > 0)
                {
                    uIPlayer.missioNode = MissionManage.GetMissionNodeById(missionId);
                    GameManage.curMissionId = missionId;
                    GameManage.curGameMissionId = GameManage.curMissionId;
                    break;

                }
            }
            uIPlayer.curPlot = plots[0];
            UiManager.uIPlayer.map_btn.gameObject.GetComponent<Image>().sprite = UiManager.getTextureSpriteByNmae("map_btn_texture", "map_btn" + 2);
            // uIPlayer.missioNode.curPlot = uIPlayer.curPlot;
            uIPlayer.ShowUI();
            CloseSelf();

        });
        if (2 > GameManage.userData.unlockChapter)
        {
            
            proUGUI1.gameObject.SetActive(true);
            proUGUI1.text = Common.Instance.big_digit_word_map[2];
            image1.texture = UiManager.getTextureByNmae("chapter_list_texture", "chapter_list_lock");
        }
        else
        {
            proUGUI1.gameObject.SetActive(false);
            image1.texture = UiManager.getTextureByNmae("chapter_list_texture", cfgArr[1].chapter_list_image);
            
        }
      

        Image btnImg1 = left_btn.transform.GetComponent<Image>();
        btnImg1.sprite = UiManager.LoadSprite("chapter_list", "chapter_list_left_grey");

        btnImg1.raycastTarget = false;

    }
    private void UpdataUI(int index)
    {
        tween = grid.DOLocalMoveX(-index * (703 + 10), 0.5f);
        for (int i = 0; i < array.Length; i++)
        {
            Button btn = rects[array[i]].GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
        }
      
        tween.onComplete = () =>
        {
            for (int i = 0; i < array.Length; i++)
            {
                Button btn = rects[array[i]].GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    if (index + 1 > GameManage.userData.unlockChapter)
                    {
                        CloseSelf();
                        Common.Instance.ShowTips("章节未解锁");
                        return;
                    }
                    if (index + 1 == GameManage.curChapter)
                    {
                        CloseSelf();
                        return;
                    }
                    int needChapter = 1;
                    int missionId = 0;
                    GameManage.curChapter = index + 1;

                    List<PlotNode> plots;
                    for (; ; )
                    {
                        missionId++;

                        MissionNode mission = MissionManage.GetMissionNodeById(missionId);
                        plots = mission.GetPlotList();
                        needChapter = mission.mCfg.chapter;
                        if (needChapter == index + 1 && plots.Count > 0)
                        {
                            uIPlayer.missioNode = MissionManage.GetMissionNodeById(missionId);
                            GameManage.curMissionId = missionId;
                            GameManage.curGameMissionId = GameManage.curMissionId;
                            break;
                        
                        }
                    }

                    uIPlayer.curPlot = plots[0];
                    // uIPlayer.missioNode.curPlot = uIPlayer.curPlot;
                    uIPlayer.ShowUI();
                    UiManager.uIPlayer.map_btn.gameObject.GetComponent<Image>().sprite = UiManager.getTextureSpriteByNmae("map_btn_texture", "map_btn" + (index + 1));
                    CloseSelf();

                });
            }
            
            Image btnImg1 = left_btn.transform.GetComponent<Image>();
            Image btnImg2 = right_btn.transform.GetComponent<Image>();
            if (index == 0)
            {

                btnImg1.sprite = UiManager.LoadSprite("chapter_list", "chapter_list_left_grey");

                btnImg1.raycastTarget = false;
                tween = null;
                return;
            }
            else
            {
                btnImg1.sprite = UiManager.LoadSprite("chapter_list", "chapter_list_left");
                btnImg1.raycastTarget = true;
            }
            if (index == 8)
            {
                btnImg2.sprite = UiManager.LoadSprite("chapter_list", "chapter_list_right_grey");
                btnImg2.raycastTarget = false;
                tween = null;
                return;
            }
            else
            {
                btnImg2.sprite = UiManager.LoadSprite("chapter_list", "chapter_list_right");
                btnImg2.raycastTarget = true;
            }
           
            rects[array[0]].localPosition = new Vector3((index - 1) * (703 + 10), rects[array[0]].localPosition.y);
            rects[array[2]].localPosition = new Vector3((index + 1) * (703 + 10), rects[array[0]].localPosition.y);
            RawImage image = rects[array[0]].GetComponent<RawImage>();
        
            TextMeshProUGUI proUGUI = rects[array[0]].Find("lock").GetComponent<TextMeshProUGUI>();
            if (index  >  GameManage.userData.unlockChapter)
            {
                proUGUI.gameObject.SetActive(true);
                proUGUI.text = Common.Instance.big_digit_word_map[index];
                image.texture = UiManager.getTextureByNmae("chapter_list_texture", "chapter_list_lock");
            }
            else
            {
                proUGUI.gameObject.SetActive(false);
                image.texture = UiManager.getTextureByNmae("chapter_list_texture", cfgArr[index - 1].chapter_list_image);
                
            }
            RawImage image1 = rects[array[2]].GetComponent<RawImage>();
            TextMeshProUGUI proUGUI1 = rects[array[2]].Find("lock").GetComponent<TextMeshProUGUI>();
            if (index + 2 > GameManage.userData.unlockChapter)
            {
                proUGUI1.gameObject.SetActive(true);
                proUGUI1.text = Common.Instance.big_digit_word_map[index + 2];
                image1.texture = UiManager.getTextureByNmae("chapter_list_texture", "chapter_list_lock");
            }
            else
            {
                proUGUI1.gameObject.SetActive(false);
                image1.texture = UiManager.getTextureByNmae("chapter_list_texture", cfgArr[index + 1].chapter_list_image);
            }
            tween = null;
        };

    }
   private void OnClickLeft()
    {
        if (tween != null)
        {
            return;
        }
        currIndex--;
        if (currIndex != 0 && currIndex != 7)
        {
            ShiftArrayValuesBackward(array);
     
        }
        UpdataUI(currIndex);

    }
    private void OnClickRight()
    {

        if (tween != null)
        {
            return;
        }
        currIndex++;
        if (currIndex != 8&& currIndex != 1)
        {
            ShiftArrayValues(array);
        }
      
        UpdataUI(currIndex);
    }
    void ShiftArrayValues(int[] array)
    {
        // 获取数组的第一个元素
        int firstElement = array[0];

        // 将数组中的所有元素向前移动一位
        for (int i = 0; i < array.Length - 1; i++)
        {
            array[i] = array[i + 1];
        }

        // 将第一个元素移到数组的末尾
        array[array.Length - 1] = firstElement;
    }
    void ShiftArrayValuesBackward(int[] array)
    {
        // 获取数组的最后一个元素
        int lastElement = array[array.Length - 1];

        // 将数组中的所有元素向后移动一位
        for (int i = array.Length - 1; i > 0; i--)
        {
            array[i] = array[i - 1];
        }

        // 将最后一个元素移到数组的第一个位置
        array[0] = lastElement;
    }

}
