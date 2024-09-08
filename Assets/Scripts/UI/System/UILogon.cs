using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class PointNode
{
    public RectTransform mTrans;
    public Vector3 linePos;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public int index;

    private RenderTexture renderTexture;
    public PointNode(RectTransform mTrans, int index,Vector3 diffVe3,int targetIndex)
    {
        this.index = index;
        Updata(mTrans, diffVe3,targetIndex);
   
    }

    public void Updata(RectTransform mTrans, Vector3 diffVe3, int targetIndex)
    {
        
        Vector3 tempPoint = UiManager.uiCamera.WorldToScreenPoint(mTrans.transform.position);
        tempPoint = UiManager.lineCamera.ScreenToWorldPoint(new Vector3(tempPoint.x, tempPoint.y, 100));
        this.mTrans = mTrans;
        linePos = tempPoint;

        float diffVe3X;
        float diffVe3Y;
        if (targetIndex < 0) 
        {
            float fl = 1f / 2f;
            diffVe3X = Math.Abs(diffVe3.x * fl );
            diffVe3Y = Math.Abs(diffVe3.y * fl);
        
         
        }
        else
        {
            if (targetIndex + 3 == index || targetIndex - 3 == index )
            {
                float fl = 1f / 3f;
                diffVe3X = Math.Abs(diffVe3.x * fl);
                diffVe3Y = Math.Abs(diffVe3.y * 0.5f);
            }
            else if (targetIndex + 1 == index || targetIndex - 1 == index)
            {
                float fl = 1f / 3f;
                diffVe3X = Math.Abs(diffVe3.x * 0.5f);
                diffVe3Y = Math.Abs(diffVe3.y * fl);
            }
            else if (targetIndex == index)
            {
                float fl = 1f / 2f;
                diffVe3X = Math.Abs(diffVe3.x * fl);
                diffVe3Y = Math.Abs(diffVe3.y * fl);
            }
            else
            {
                float fl = 2f / 3f;
                diffVe3X = Math.Abs(diffVe3.x * fl);
                diffVe3Y = Math.Abs(diffVe3.y * fl);

            }

        }
        
       
        Vector3 tempMaxPoint = UiManager.uiCamera.WorldToScreenPoint(mTrans.transform.position + new Vector3(diffVe3X, diffVe3Y, 0));
        tempMaxPoint = UiManager.lineCamera.ScreenToWorldPoint(new Vector3(tempMaxPoint.x, tempMaxPoint.y, 100));

        Vector3 tempMinPoint = UiManager.uiCamera.WorldToScreenPoint(mTrans.transform.position - new Vector3(diffVe3X, diffVe3Y, 0));
        tempMinPoint = UiManager.lineCamera.ScreenToWorldPoint(new Vector3(tempMinPoint.x, tempMinPoint.y, 100));
        maxX = tempMaxPoint.x;
        minX = tempMinPoint.x;
        maxY = tempMaxPoint.y;
        minY = tempMinPoint.y;
       

    }

}
public delegate bool IsSuccessCallBack(PointNode[] points);
public class UILogon : UIBase
{
    LineRenderer lineRenderer;
    RectTransform lineTran;
    RenderTexture renderTexture;
    public RawImage rawImage;
    public RectTransform rawRoot;
    public GameObject drawbg;
    public Button closeBtn;
    public Transform word_root;
    public GameObject mask;
    // 使用List储存前几个位置的坐标点
    // 由于绘制过程中需要遍历坐标,所以不能直接使用队列
    // 而是用List进行模拟
    List<Vector3> pointList = new List<Vector3>();
    // 需要记录的坐标点的数量
    [SerializeField] int pointSize = 20;
    bool isDown = false;
    public List<RectTransform> dotTranList;
    public int needDot = 3;
    List<PointNode> pointNodeList = new List<PointNode>();
    Vector3 diffVe3;
    MissionNode missionNode;
    Action<PointNode[]> callBack;
    Camera mainCamera;
    public DragonBonesController bonesController;
    int curNum = 1;

    PointNode currNode = null;
    List<int> needIndexList = new List<int>();
    PointNode[] pointArr;
    List<Tween> pointTween = new List<Tween>();
    Transform hangPoint;
    public RectTransform new_bie;
    Tween tween1;
    Tween tween2;
    int endInde = -1;
    public IsSuccessCallBack IsSuccessCallBack;
    public RectTransform draw_tally_bg;
    private int skillId;
    Action nieBeCallback;
    public GameObject mask_2;
    private bool isLock = false;
    public bool isSelf = false;
    public override void OnStart()
    {
        needDot = GameManage.userData.needDot;
        pointArr = new PointNode[needDot];
        //if (newBieIndexList.Count == 0)
        //{
        //    pointArr = new PointNode[needDot];
        //}
        //else
        //{
        //    pointArr = new PointNode[newBieIndexList.Count];
        //}

        //  ChangeRenderTextureSize();
        diffVe3 = dotTranList[2].transform.position - dotTranList[4].transform.position;
        InitPointList();
        closeBtn.onClick.AddListener(CloseSelf);
        Image image = closeBtn.transform.GetComponent<Image>();
        image.sprite = UiManager.LoadSprite("common", "draw_tally10");
        new_bie.position = dotTranList[3].position;
   
        new_coroutine = DelayedActionProvider.Instance.DelayedAction(() =>
        {
            new_coroutine = null;
            NewBieTween();
        }, 0.5f);
      
        //mainCamera = UiManager.lineCamera;
        //  AdjustCameraViewport();
    }
    public void SetIsSuccess(IsSuccessCallBack action = null)
    {
        IsSuccessCallBack = action;


    }
    public void SetData(MissionNode missionNode, Action<PointNode[]> callBack,int skillId)
    {
        this.missionNode = missionNode;
        this.callBack = callBack;
        this.skillId = skillId;
    }
    public void SetData(MissionNode missionNode, Action<PointNode[]> callBack)
    {
        this.missionNode = missionNode;
        this.callBack = callBack;
   
    }
    public void SetHangPoint(Transform hangPoint)
    {
        this.hangPoint = hangPoint;

    }
    void CreateRenderTexture(int width, int height)
    {
        // 创建 RenderTexture
        renderTexture = new RenderTexture(width, height, 24);
        renderTexture.name = "MyRenderTexture";
        renderTexture.Create();
    }
    void ChangeRenderTextureSize()
    {

        // 创建新大小的 RenderTexture
        CreateRenderTexture(Screen.width, Screen.height);

        // 更新相机的 targetTexture
        UiManager.lineCamera.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
        

    }
    DelayedCoroutineData new_coroutine;
    private List<int> newBieIndexList = new List<int>();
    private int newBieIndex = 0;
    public void SetnewBieDotList(List<int> ints,Action nieBeCallback = null)
    {
        needDot = GameManage.userData.needDot;
        newBieIndexList = ints;
      
        if (nieBeCallback != null)
        {
            new_bie.gameObject.SetActive(false);
            this.nieBeCallback = nieBeCallback;
        
            mask_2.SetActive(true);
            isLock = true;
        }
    }
    private int curNewBieIndex = 0;
    private void NewBieTween()
    {
        if (newBieIndexList.Count == 0)
        {
            return;
        }
    
        if (curNewBieIndex == 0)
        {

            lineRenderer.positionCount = 0;
        }
        if (isDown)
        {
            return;
        }
        curNewBieIndex++;
        new_bie.position = dotTranList[newBieIndexList[newBieIndex]].position;
        newBieIndex++;
        tween1 = new_bie.DOMove(dotTranList[newBieIndexList[newBieIndex]].position, 0.5f);
       // newBieIndex++;
        tween1.onComplete = () =>
        {
            tween1 = null;
            if (curNewBieIndex + 1 < newBieIndexList.Count)
            {
                NewBieTween();
            }
            else
            {
              
                new_coroutine = DelayedActionProvider.Instance.DelayedAction(() =>
                {
                    if (nieBeCallback != null)
                    {
                        nieBeCallback.Invoke();
                        CloseSelf();
                    }
                    else
                    {
                        lineRenderer.positionCount = 0;
                        curNewBieIndex = 0;
                        new_coroutine = null;
                        newBieIndex = 0;
                        NewBieTween();
                    }
              
                }, 1f);
            }
           
            //    if (isDown)
            //    {
            //        return;
            //    }
            //    tween2 = new_bie.DOMove(dotTranList[newBieIndexList[newBieIndex]].position, 0.5f);
            //    newBieIndex = 0;
            //    tween2.onComplete = ()=> {
            //        new_coroutine = DelayedActionProvider.Instance.DelayedAction(() =>
            //        {
            //            new_coroutine = null;
            //            NewBieTween();
            //        }, 1f);

            //    };
            //    tween2.onUpdate = () =>
            //    {
            //        Vector3 tempMaxPoint = UiManager.uiCamera.WorldToScreenPoint(new_bie.transform.position);
            //        tempMaxPoint = UiManager.lineCamera.ScreenToWorldPoint(new Vector3(tempMaxPoint.x, tempMaxPoint.y, 100));
            //        int positionCount = lineRenderer.positionCount++;
            //        // lineRenderer.SetVertexCount(positionCount);
            //        lineRenderer.SetPosition(positionCount, tempMaxPoint);
            //    };
        };
            tween1.onUpdate = () =>
            {
                Vector3 tempMaxPoint = UiManager.uiCamera.WorldToScreenPoint(new_bie.transform.position);
                tempMaxPoint = UiManager.lineCamera.ScreenToWorldPoint(new Vector3(tempMaxPoint.x , tempMaxPoint.y, 100));
                int positionCount = lineRenderer.positionCount++;
                // lineRenderer.SetVertexCount(positionCount);
                lineRenderer.SetPosition(positionCount, tempMaxPoint);
            };


    }
     

    private void AdjustCameraViewport()
    {
        if (mainCamera != null)
        {
            // 获取屏幕的宽高比
            float screenAspect = Screen.width / (float)Screen.height;

            // 设置相机的宽高比
            mainCamera.aspect = screenAspect;

            // 计算新的 orthographicSize 以确保视口与屏幕一样大
            float targetOrthographicSize = mainCamera.orthographicSize;

            if (screenAspect < 1.0f)
            {
                // 如果屏幕更矮，则根据屏幕高度调整 orthographicSize
                targetOrthographicSize = mainCamera.orthographicSize * (1.0f / screenAspect);
            }

            // 应用新的 orthographicSize
            mainCamera.orthographicSize = targetOrthographicSize;
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }
    
    private void Update()
    {
        if (isLock)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (IsOnClickCanvas())
            {
                isDown = true;
                if (tween1 != null)
                {
                    tween1.Kill();
                }

                if (tween2 != null)
                {
                    tween2.Kill();
                }
                if (new_coroutine != null)
                {
                    new_coroutine.isStop = true;
                }
                new_bie.gameObject.SetActive(false);
                lineRenderer.positionCount = 0;
               
            }
            //   lineRenderer.positionCount = 0;

        }
        if (Input.GetKeyUp(KeyCode.A)) 
        {

           lineRenderer.positionCount = 0;
          

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isDown)
            {
                isDown = false;
                EndLineRen();
          
            }
          //  lineRenderer.positionCount = 0;
        

        }

    }
    private bool IsOnClickCanvas()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.name == "draw_tally_bg")
            {
                return true;
            }
        }
        return false;

    }
    private void FixedUpdate()
    {
       
        if (isDown)
        {
  
            UpdatePointList();
            // 更新坐标点列表
        }

    }


    // 初始化坐标点列表
    void InitPointList()
    {
        lineRenderer = transform.Find("Image").GetComponent<LineRenderer>();
        lineTran = gameObject.GetComponent<RectTransform>();
        renderTexture = new RenderTexture(Screen.width, Screen.height, 8);

        UiManager.lineCamera.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
        //rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        //   rawImage.rectTransform.anchoredPosition = new Vector2(drawbg.rectTransform.anchoredPosition.x + drawbg.rectTransform.anchoredPosition.x/2, rawImage.rectTransform.anchoredPosition.y);
        // 重置坐标点数量
        rawImage.rectTransform.offsetMax = Vector2.zero;
        rawImage.rectTransform.offsetMin = Vector2.zero;
        rawImage.transform.SetParent(draw_tally_bg.transform);
        rawImage.transform.SetAsFirstSibling();
        lineRenderer.positionCount = 0;

        for (int i = 0; i < dotTranList.Count; i++)
        {
            Image image = dotTranList[i].GetComponent<Image>();
            int index = i + 1;
            image.sprite = UiManager.LoadSprite("draw_tally", "draw_tally0" + index);
            PointNode pointNode = new PointNode(dotTranList[i], i, diffVe3, -1);
            pointNodeList.Add(pointNode);
        }

    }

    // 更新坐标点列表 (模拟队列方式)
    Vector3 uiLocalPos;
    public bool IsValueInArray(PointNode targetValue)
    {
        for (int i = 0; i < pointArr.Length; i++)
        {
            if (pointArr[i] == targetValue)
            {
                return true; // 如果找到目标值，返回true
            }
        }
        return false; // 如果未找到目标值，返回false
    }
    void UpdatePointList()
    {
        if (uiLocalPos == UiManager.lineCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100)))
        {
            return;
        }
        uiLocalPos = UiManager.lineCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        if (currNode == null)
        {
            float distance = 10000;
            for (int i = 0; i < pointNodeList.Count; i++)
            {
                Vector3 tempPoint = uiLocalPos;

                float temp = Vector3.Distance(tempPoint, pointNodeList[i].linePos);
                if (distance > Math.Abs(temp))
                {
                    distance = Math.Abs(temp);
                    currNode = pointNodeList[i];
                    needIndexList.Add(i);
                }

            }
            Tween tween = currNode.mTrans.DOScale(new Vector3(1.5f,1.5f , 1.5f), 0.3f);
            pointTween.Add(tween);
            tween.onComplete = () =>
            {
                pointTween.Remove(tween);

            };
         
            pointArr[0] = currNode;
        }
        else
        {
            if (uiLocalPos.x > currNode.maxX || uiLocalPos.x < currNode.minX
                || uiLocalPos.y > currNode.maxY || uiLocalPos.y < currNode.minY)
            {
                PointNode temNode = GetPointNodeByPoint(uiLocalPos, currNode);
                List<int> a = new List<int>();
                
                if (IsValueInArray(temNode))
                {
                    return;
                }
                if (temNode != null)
                {
                    curNum++;
                  
                    if (curNum > needDot)
                    {
                        if (endInde < 0)
                        {
                            endInde = pointList.Count;

                        }

                    }
                    else
                    {
                        currNode = temNode;
                        pointArr[curNum - 1] = currNode;
                        Tween tween = currNode.mTrans.DOScale(new Vector3(2f, 2f, 2f), 0.3f);
                        pointTween.Add(tween);
                        tween.onComplete = () =>
                        {
                            pointTween.Remove(tween);

                        };
                      
                    }
                }
            }

        }


        int positionCount = lineRenderer.positionCount++;
        // lineRenderer.SetVertexCount(positionCount);
        pointList.Add(uiLocalPos);
        lineRenderer.SetPosition(positionCount, uiLocalPos);
        

    }
    /// <summary>
    /// Texture转换成Texture2D...
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }


    void EndLineRen()
    {
        if (isSelf == true)
        {
            callBack(pointArr);
            return;
        }
        new_bie.gameObject.SetActive(false);
        lineRenderer.positionCount = 0;
        if (curNum < newBieIndexList.Count)
        {
            for (int i = 0; i < pointTween.Count; i++)
            {
                pointTween[i].Kill();
            }
            pointTween.Clear();
            for (int i = 0; i < dotTranList.Count; i++)
            {
                dotTranList[i].localScale = Vector3.one;
            }
            pointList.Clear();
            for (int i = 0; i < pointArr.Length; i++)
            {
                pointArr[i] = null;

            }
            currNode = null;
            uiLocalPos = Vector3.zero ;
            curNum = 1;
            return;
        }
        //print(pointNodeList[1].linePos.x - pointNodeList[0].linePos.x);
        int needIndex = 0;
        float diff = 999999;
        if (endInde < 0)
        {
            endInde = pointList.Count;

        }
        for (int i = 0; i < endInde; i++)
        {
            float temp = Vector3.Distance(pointList[i], currNode.linePos);
            if (diff > Math.Abs(temp))
            {
                diff = temp;
                needIndex = i;
            }

        }
        for (int i = 0; i < needIndex; i++)
        {
            int positionCount = lineRenderer.positionCount++;
            lineRenderer.SetPosition(positionCount, pointList[i] );
        }
        closeBtn.gameObject.SetActive(false);
        if (IsSuccessCallBack == null || IsSuccessCallBack(pointArr) == true)
        {
            skillcnofigData skillcfg = GetCfgManage.Instance.GetCfgByNameAndId<skillcnofigData>("skill",skillId);
            DragonBonesController wordBones = UiManager.LoadBonesByNmae("fuwenzi_bones");
            wordBones.transform.SetParent(word_root);
            wordBones.transform.localPosition = Vector3.zero;
            wordBones.transform.localScale = Vector3.one;
            mask.SetActive(true);
            wordBones.PlayAnimation(skillcfg.effect_name, false, () =>
            {
                bonesController.SwitchAndPlayAnimation("Attack", false, () =>
                {
                    mask.SetActive(false);
                    wordBones.gameObject.SetActive(false);
                    callBack(pointArr);
                    CloseSelf();

                });
            });
            if (hangPoint != null)
            {
                DelayedActionProvider.Instance.DelayedAction(() =>
                {
                    bonesController.transform.DOMove(hangPoint.position, 0.4f);


                }, 3.2f);
            }

        }
        else
        {
            UITips gui = UiManager.OpenUI<UITips>("UITips");
            curNum = 1;
            gui.SetLabel("绘制符文失败");
            callBack(pointArr);
            CloseSelf();
        }
    }
   
    PointNode GetPointNodeByPoint(Vector3 point,PointNode currNode)
    {

        for (int z = 0; z < dotTranList.Count; z++)
        {
            if (pointNodeList[z] != null)
            {
                pointNodeList[z].Updata(dotTranList[z], diffVe3, currNode.index);
            }

        }

        for (int i = 0; i < pointNodeList.Count; i++)
        {
            //print("inde：" + pointNodeList[i].index + " maxX:" + pointNodeList[i].maxX + "minX:" + pointNodeList[i].minX + "maxY:" + pointNodeList[i].maxY + "minY:" + pointNodeList[i].minY);

            if (point.x < pointNodeList[i].maxX && point.x > pointNodeList[i].minX
                && point.y < pointNodeList[i].maxY && point.y > pointNodeList[i].minY)
            {
                
             

                return pointNodeList[i];
            }

        }
        return null;

    }
    public override void OnDestroyImp()
    {
        
        if (new_coroutine != null)
        {
            new_coroutine.isStop = true;
            

        }
      
        
    }


}






