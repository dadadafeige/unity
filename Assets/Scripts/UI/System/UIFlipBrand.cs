using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFlipBrand : UIBase
{
    public delegate void DataChangedEventHandler(FlipBrandGridNode gridNode);
    
    private int rowValue;
    private int columnValue;
    private int caoyaoNum;
    private int dusheNum;
    private int qiuyinNum;
    private int baoxiaoNum;
    private List< FlipBrandRowNode> rowList  = new List<FlipBrandRowNode>();
  
    private Queue<int> gridTypeQueue = new Queue<int>();
    public GameObject rowItme;
    public RectTransform gridRoot;
    public TextMeshProUGUI mTick;
    public Button close_btn;
    public Image diff_icon;
    public Button bag_btn;
    public Image box_icon;
    public Image QiuYin_Icon;
    public RectTransform topTrans;
    public TextMeshProUGUI diff_word;
    public TextMeshProUGUI qiuyinNumText;
    public TextMeshProUGUI qiuyinNumMaxText;
    public CanvasGroup injured;
    public Transform hanging_point;
    private CustomTimer.TimerInfo timerInfo;
    public GameObject mask;
    public RectTransform grid_bg;
    Action callBack;
    private int currQiuyinNum = 0;
    private int currSheNum = 0;
    private int currCaoyaoNum = 0;
    private int currBaoxiangNum = 0;
    private int curLevel = 1;
    DragonBonesController caoyao_bones;
    DragonBonesController baoxiang_bones;
    int currHaoshi = 0;
    public RectTransform root;
    public Button rule_btn;
    public override void OnAwake()
    {
        RefreshData(curLevel);
        FlipBrandGridNode.DataChanged += ShowGridUI;
      
 
    }
    public void SetCallBack(Action callBack)
    {
        this.callBack = callBack;
       

    }
   
    // Start is called before the first frame update
    public override void OnStart()
    {
        InitGrid();
        InitImage();
        HideMask();
        close_btn.onClick.AddListener(() => {

            CloseSelf();
        });
        RefreshQiuyinText();
        AdapterUI();

        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                RefreshTime();
            });
        });
        rule_btn.onClick.AddListener(() =>
        {
            if (timerInfo != null)
            {
                timerInfo.isPaused = true;
            }
            MissionManage.ShowDescription(() =>
            {
                if (timerInfo != null)
                {
                    timerInfo.isPaused = false;
                }

            });



        });
        bag_btn.onClick.AddListener(() => { Common.Instance.ShowBag(); });
     
    }

    private void InitImage()
    {
        box_icon.sprite = UiManager.LoadSprite("flip_brand", "BaoXiang");
        if (GameManage.curGameMissionId == 75)
        {
            QiuYin_Icon.sprite = UiManager.LoadSprite("flip_brand", "ChongCao");
        }
        else
        {
            QiuYin_Icon.sprite = UiManager.LoadSprite("flip_brand", "QiuYin");
        }


    }
    private void ShowMask()
    {
        mask.SetActive(true);

    }
    private void HideMask()
    {
        mask.SetActive(false);

    }
    private void RefreshTime()
    {
        if (timerInfo != null)
        {
            timerInfo.StopCoroutie();
            timerInfo = null;
        }
        timerInfo = UiManager.customTimer.RegisterTimer(TcikFun, 1, 5, () =>
        {
            UpdataAllGridIsReady(false);
            timerInfo = UiManager.customTimer.RegisterTimer(TcikFun1, 1, 60, () =>
            {
                timerInfo.StopCoroutie();
                timerInfo = null;
                UIFlipBrandSettle gui = UiManager.OpenUI<UIFlipBrandSettle>("UIFlipBrandSettle");
                gui.SetNum(currQiuyinNum, currSheNum, currCaoyaoNum, currBaoxiangNum, 60);
                gui.SetData(false, () =>
                {
                    currQiuyinNum = 0;
                    RefreshData(curLevel);
                    InitGrid();
                    RefreshTime();
                    RefreshQiuyinText();

                }, () =>
                {
                    if (callBack != null)
                    {
                        callBack();

                    }
                    CloseSelf();
                });

            }
            );
            TcikFun1(-1);

        });
        TcikFun(-1);
    }
    public override void OutUI()
    {
        if (timerInfo != null)
        {
            timerInfo.isPaused = true;
        }
    }
    public override void GoInUI()
    {
        if (timerInfo != null)
        {
            timerInfo.isPaused = false;
        }
    }
    private void RefreshQiuyinText()
    {
        qiuyinNumText.text = "<sprite=" + currQiuyinNum + ">";
        qiuyinNumMaxText.text = "<sprite=" + qiuyinNum + ">";

    }
    private void AdapterUI()
    {
        float screenHeight = Screen.height;
        float pro = screenHeight / 1080;
        root.localScale = new Vector3(pro, pro, pro);
    }
    private void RefreshData(int cfgId)
    {
        flipbrand_listcnofigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<flipbrand_listcnofigData>("flipbrand_list", cfgId);
        if (cfg == null)
        {
            callBack();
            CloseSelf();
            return;
        }
        rowValue = cfg.row;
        columnValue = cfg.column;
        caoyaoNum = cfg.caoyaoNum;
        dusheNum = cfg.dusheNum;
        qiuyinNum = cfg.qiuyinNum;
        baoxiaoNum = cfg.baoxiaoNum;
        diff_icon.sprite = UiManager.LoadSprite("common", cfg.diff_icon);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TcikFun(int second)
    {
        //if (second == 3)
        //{
        //    DragonBonesController dragon = UiManager.LoadBonesByNmae("daojishi_bones");
        //    dragon.transform.SetParent(mask.transform);
        //    dragon.transform.localScale = Vector3.one;
        //    dragon.transform.localPosition = Vector3.zero;

        //}
        int fl = 5 - second - 1;
        mTick.text = Common.Instance.SplitNumber(fl);


    }
    private void UpdataAllGridIsReady(bool isReady)
    {
        for (int i = 0; i < rowList.Count; i++)
        {
            List<FlipBrandGridNode> gridNodes = rowList[i].GridNodes;
            for (int z = 0; z < gridNodes.Count; z++)
            {
                gridNodes[z].UpdataReady(isReady);
            }
        }
    }
    public void TcikFun1(int second)
    {
        currHaoshi += 1;
        int fl = 60 - second - 1;
        mTick.text = Common.Instance.SplitNumber(fl);
    }
    public void TcikFun2(int second)
    {
        int fl = 3 - second - 1;
        mTick.text = Common.Instance.SplitNumber(fl);
      


    }
    private void InitGrid()
    {
        /// 需要随机的格子类型的数量初始化
        gridTypeQueue.Clear();
        for (int i = 0; i < caoyaoNum; i++)
        {
            if (GameManage.curChapter == 6)
            {
                gridTypeQueue.Enqueue(7);
            }
            else
            {
                gridTypeQueue.Enqueue(2);
            }
           
        }
        for (int i = 0; i < dusheNum; i++)
        {
            if (GameManage.curChapter == 6)
            {
                gridTypeQueue.Enqueue(8);
            }
            else
            {
                gridTypeQueue.Enqueue(3);
            }

         
        }
        for (int i = 0; i < qiuyinNum; i++)
        {
            if (GameManage.curChapter == 6)
            {
                gridTypeQueue.Enqueue(6);
            }
            else
            {
                gridTypeQueue.Enqueue(4);
            }
        }
        
        for (int i = 0; i < baoxiaoNum; i++)
        {
            ItemNode item = BagManage.Instance.GetItemById(19);
            if (item == null || item.count < 1)
            {
                gridTypeQueue.Enqueue(5);
            }
        }
       
       
        for (int i = 0; i < rowList.Count; i++)
        {
            rowList[i].go.SetActive(false);
        }
        //初始化所有格子
        for (int i = 0; i < rowValue; i++)
        {
            if (rowList.Count - 1 < i)
            {
                GameObject go = GameObject.Instantiate(rowItme);
                FlipBrandRowNode flipBrandRowNode = new FlipBrandRowNode(columnValue,go);
                go.SetActive(true);
                go.transform.SetParent(gridRoot);
                go.transform.localScale = Vector3.one;
                ShowRowUI(flipBrandRowNode, go);
                rowList.Add(flipBrandRowNode);
            }
            else
            {
                if (rowList[i].GridNodes != null)
                {
                    for (int z = 0; z < rowList[i].GridNodes.Count; z++)
                    {
                        rowList[i].GridNodes[z].gridObj.SetActive(false);
                    }
                }
                rowList[i].UpdataRowNode(columnValue);
               
                rowList[i].go.SetActive(true);
                ShowRowUI(rowList[i], rowList[i].go);
            }
         
        }
        RectTransform rect = gridRoot.GetComponent<RectTransform>();
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        //将格子类型随机分配到 格子上
        List<int> randomList = GenerateUniqueRandomNumbers(0, rowValue * columnValue - 1, gridTypeQueue.Count);
        for (int i = 0; i < randomList.Count; i++)
        {
            int index = randomList[i] / columnValue;
            FlipBrandRowNode updataRow = rowList[index];
            Debug.Log(index + "," + randomList[i] % columnValue);
            updataRow.UpdateGridByType(randomList[i] % columnValue, gridTypeQueue.Dequeue());
        }
        if (rowList.Count > 0)
        {
            grid_bg.sizeDelta = new Vector2(rowList[0].GridNodes.Count*96 + 70,rowList.Count* 96 + 70);
        }
        
      //  LayoutRebuilder.ForceRebuildLayoutImmediate(gridRoot);
       
    }
    public void ShowRowUI(FlipBrandRowNode flipBrandRowNode,GameObject go)
    {
        ComponentItme componentItme = go.GetComponent<ComponentItme>();
        for (int i = 0; i < flipBrandRowNode.GridNodes.Count; i++)
        {
            FlipBrandGridNode gridNode = flipBrandRowNode.GridNodes[i];
            if (flipBrandRowNode.GridNodes[i].gridObj == null)
            {
                GameObject itemObj = GameObject.Instantiate(componentItme.itemObj);
                itemObj.SetActive(true);
                itemObj.transform.SetParent(go.transform);
                itemObj.transform.localScale = Vector3.one;
                gridNode.BindGrid(itemObj);
                ShowGridUI(gridNode);
            }
            else
            {
                GameObject itemObj = flipBrandRowNode.GridNodes[i].gridObj;
                itemObj.SetActive(true);
                ShowGridUI(gridNode);
            }
           
        }

    }
    public void ShowGridUI(FlipBrandGridNode flipBrandGridNode)
    {
        GameObject go = flipBrandGridNode.gridObj;
        UnityEngine.UI.Image image = go.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();
        image.gameObject.SetActive(!flipBrandGridNode.isReady || flipBrandGridNode.cfg.id != 1);
        if (flipBrandGridNode.isReady)
        {
            UnityEngine.UI.Image bg = go.transform.Find("bg").GetComponent<UnityEngine.UI.Image>();
            Button btn = go.transform.Find("click").GetComponent<Button>();
            bg.sprite = UiManager.LoadSprite("flip_brand", "MuGe");
            if (flipBrandGridNode.cfg.id != 1)
            {
                image.sprite = UiManager.LoadSprite("flip_brand", flipBrandGridNode.cfg.icon);
            }
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>{ ClickGrid(flipBrandGridNode); });
           // 
        }
        else
        {
            if (flipBrandGridNode.isClick)
            {
                image.sprite = UiManager.LoadSprite("flip_brand", flipBrandGridNode.cfg.icon);
            }
            else
            {
                image.sprite = UiManager.LoadSprite("flip_brand", "Gai");

            }


        }
        

    }
    private void ClickGrid(FlipBrandGridNode flipBrandGridNode)
    {
        if (flipBrandGridNode.isReady)
        {
            return;
        }
        if (flipBrandGridNode.isClick)
        {
            return;
        }
        flipBrandGridNode.UpdataClick(true);
        DragonBonesController dragonBonesController = UiManager.LoadBonesByNmae("mugai_bones");
        dragonBonesController.transform.SetParent(flipBrandGridNode.gridObj.transform);
        dragonBonesController.transform.localScale = new Vector3(2, 2, 2);
        dragonBonesController.transform.localPosition = Vector3.zero;
        if (flipBrandGridNode.cfg.type == 3)
        {
            currSheNum += 1;
            ShowMask();
            injured.gameObject.SetActive(true);
            Tween tween = injured.DOFade(0, 1.5f);
            tween.SetEase(Ease.InOutCirc);
            ShowTipsUI("受到毒蛇攻击时间减半");
            tween.onComplete = () =>
            {
                injured.alpha = 1;
                injured.gameObject.SetActive(false);
                HideMask();

            };
            Transform image = flipBrandGridNode.gridObj.transform.Find("Image");
            image.SetParent(gameObject.transform);
            image.DOMove(hanging_point.position, 0.5f);
            Tween tween1 = image.DOScale(3, 0.5f);
            ShowMask();
            // ShowTipsUI("恭喜获得蚯蚓");
            tween1.onComplete = () =>
            {
                DelayedActionProvider.Instance.DelayedAction(() =>
                {
                    image.DOMove(flipBrandGridNode.gridObj.transform.position, 0.5f);
                    Tween tween1 = image.DOScale(0, 0.5f);
                    tween1.onComplete = () =>
                    {
                        image.SetParent(flipBrandGridNode.gridObj.transform);
                        image.localPosition = Vector3.zero;
                        image.localScale = Vector3.one;
                        HideMask();

                    };
                }, 0.2f);
                timerInfo.currentExecutionCount += (60 - timerInfo.currentExecutionCount) / 2;
                TcikFun1(timerInfo.currentExecutionCount);
            };
         }
        else if (flipBrandGridNode.cfg.type == 2)
        {

            currCaoyaoNum += 1;
            PlayAnimation(caoyao_bones, "caoyao_bones", null, "获得神奇草药,可重复查看草药3秒");
            timerInfo.isPaused = true;
            UpdataAllGridIsReady(true);
            UiManager.customTimer.RegisterTimer(TcikFun2, 1, 3, () =>
            {
                timerInfo.isPaused = false;
                UpdataAllGridIsReady(false);

            });
            TcikFun2(-1);
        }
        else if (flipBrandGridNode.cfg.type == 4)
        {
          
            currQiuyinNum += 1;
            if (GameManage.curChapter == 6)
            {
                BagManage.Instance.Add(9);
            }
            else
            {
                BagManage.Instance.Add(7);
            }
            RefreshQiuyinText();
            Transform image = flipBrandGridNode.gridObj.transform.Find("Image");
            image.SetParent(gameObject.transform);
            image.DOMove(hanging_point.position, 0.5f);
            Tween tween = image.DOScale(3, 0.5f);
            ShowMask();
            // ShowTipsUI("恭喜获得蚯蚓");
            tween.onComplete = () =>
            {
                DelayedActionProvider.Instance.DelayedAction(() =>
                {
                    image.DOMove(QiuYin_Icon.transform.position, 0.5f);
                    Tween tween1 = image.DOScale(0, 0.5f);
                    tween1.onComplete = () =>
                    {
                        image.SetParent(flipBrandGridNode.gridObj.transform);
                        image.localPosition = Vector3.zero;
                        image.localScale = Vector3.one;
                        ShowTweenTopItem(QiuYin_Icon.transform);
                        HideMask();
                        if (currQiuyinNum == qiuyinNum)
                        {
                            timerInfo.StopCoroutie();
                            timerInfo = null;
                            
                      
                            if (curLevel == 5)
                            {
                                Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(curLevel), () =>
                                {

                                    currQiuyinNum = 0;
                                    RefreshData(curLevel);
                                    InitGrid();
                                    RefreshTime();
                                    RefreshQiuyinText();
                                }, () =>
                                {
                                    if (callBack != null)
                                    {
                                        callBack();

                                    }
                                    CloseSelf();

                                });
                            }
                            else
                            {
                                Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(curLevel), () =>
                                {

                                    currQiuyinNum = 0;
                                    RefreshData(curLevel);
                                    InitGrid();
                                    RefreshTime();
                                    RefreshQiuyinText();
                                }, () => { }, () =>
                                {
                                    currQiuyinNum = 0;
                                    curLevel++;
                                    RefreshData(curLevel);
                                    InitGrid();
                                    RefreshTime();
                                    RefreshQiuyinText();

                                });
                            }



                            //UIFlipBrandSettle gui = UiManager.OpenUI<UIFlipBrandSettle>("UIFlipBrandSettle");
                            //gui.SetNum(currQiuyinNum, currSheNum, currCaoyaoNum, currBaoxiangNum, currHaoshi - 1);

                            //gui.SetData(true, () =>
                            //{
                            //    currQiuyinNum = 0;
                            //    curLevel++;
                            //    RefreshData(curLevel);
                            //    InitGrid();
                            //    RefreshTime();
                            //    RefreshQiuyinText();

                            //}, () =>
                            //{
                            //    if (callBack != null)
                            //    {
                            //        callBack();

                            //    }
                            //    CloseSelf();
                            //});
                            //if (curLevel == 5)
                            //{
                            //    gui.next_btn.gameObject.SetActive(false);
                            //}
                        }

                    };
                }, 0.2f);


            };

        }
        else if (flipBrandGridNode.cfg.type == 5)
        {
            currBaoxiangNum += 1;
            BagManage.Instance.Add(19);
            PlayAnimation(baoxiang_bones, "baoxiang_bones", box_icon.transform, "恭喜获得神秘宝箱次日可开启",
                () =>
                {
                    ShowTweenTopItem(box_icon.transform);
                });



        }
        

    }
    public void ShowTweenTopItem(Transform tran)
    {
        Tween tween = tran.DOScale(1.2f,0.2f);
        tween.onComplete = () =>
        {
            tran.DOScale(1, 0.2f);
        };


    }
    public void ShowTipsUI(string str)
    {
        UITips gui = UiManager.OpenUI<UITips>("UITips");
        gui.SetLabel(str);

    }
    private void PlayAnimation(DragonBonesController ani,string bones_name,Transform goTran = null,string str = null,Action action = null)
    {
        float time = 0.5f;
        if (str != null)
        {
            ShowTipsUI(str);
        
        }
        if (ani == null)
        {
            ShowMask();
            ani = UiManager.LoadBonesByNmae(bones_name);
            ani.transform.SetParent(gameObject.transform);
            ani.transform.localScale = new Vector3(10, 10, 10);
            ani.transform.localPosition = Vector3.zero;
            ani.PlayAnimation(ani.armatureComponent.animation.animationNames[0], false, () =>
            {
                if (goTran != null)
                {
                    ani.transform.DOMove(goTran.position, time);
                    Tween tween = ani.transform.DOScale(0, time);
                    tween.onComplete = () =>
                    {
                        ani.gameObject.SetActive(false);
                        if (action != null)
                        {
                            action();
                        }
                        if (str!=null)
                        {
                            HideMask();
                        }
                    };
                }
                else
                {
                    ani.gameObject.SetActive(false);
                    if (action != null)
                    {
                        action();
                    }
                    if (str != null)
                    {
                        HideMask();
                    }
                }

               // 
            });
        }
        else
        {
            ani.gameObject.SetActive(true);
            ani.transform.localPosition = Vector3.zero;
            ani.PlayAnimation(ani.armatureComponent.animation.animationNames[0], false, () =>
            {
                if (goTran != null)
                {
                    ani.transform.DOMove(goTran.position, time);
                    Tween tween = ani.transform.DOScale(0, time);
                    tween.onComplete = () =>
                    {
                        ani.gameObject.SetActive(false);
                        if (action != null)
                        {
                            action();
                        }
                        HideMask();
                    };
                }
                else
                {
                    ani.gameObject.SetActive(false);
                    if (action != null)
                    {
                        action();
                        HideMask();
                    }
                }
            });
        }


    }
    public List<int> GenerateUniqueRandomNumbers(int N, int M, int I)
    {
        if (N > M || I > M - N + 1)
        {
            throw new ArgumentException("Invalid input parameters.");
        }

        List<int> randomNumbers = new List<int>();
        HashSet<int> usedNumbers = new HashSet<int>();

        System.Random random = new System.Random();

        while (randomNumbers.Count < I)
        {
            int randomNumber = random.Next(N, M + 1);

            if (!usedNumbers.Contains(randomNumber))
            {
                randomNumbers.Add(randomNumber);
                usedNumbers.Add(randomNumber);
            }
        }

        return randomNumbers;
    }
    public override void OnDestroyImp()
    {
        if (timerInfo != null)
        {
            timerInfo.StopCoroutie();
            timerInfo = null;
        }
        if (callBack != null)
        {
            callBack();

        }

    }
}