using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMap : UIBase
{
    private List<GameObject> needHides = new List<GameObject>();
    public RectTransform canvasRectTransform; // ���ڻ�ȡ Canvas �� RectTransform
    public List<MapSceneNode> mapSceneNodes = new List<MapSceneNode>();
    public RectTransform map_root;
    private MapSceneNode selectNode;
    private UIPlayerStory uIPlayer;
    private bool isMousePressed;
    public Button close_btn;
    public override void OnAwake()
    {
        uiCamera = UiManager.uiCamera;
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
       
        close_btn.onClick.AddListener(CloseSelf);
    }
    private Camera uiCamera;
    private void LoadChapterMap()
    {
        GameObject go = UiManager.LoaChapterMapByNmae("map_chapter"+GameManage.curChapter);
        ChapterMap chapter = go.GetComponent<ChapterMap>();
        go.transform.SetParent(map_root);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        mapSceneNodes = chapter.mapSceneNodes;


    }
    bool IsCheck()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer)
        {
            return Input.GetMouseButtonUp(0) || Input.touchCount == 0;
          
        }
        else
        {
            return Input.GetMouseButtonDown(0) || Input.touchCount == 1;
        }


    }
    void Update()
    {
        // �����������ʱִ��
        CheckMousePosition();

        if (Input.GetMouseButtonDown(0) || Input.touchCount == 1)
        {
            // ��갴��ʱִ�еĲ���
            isMousePressed = true;
        }
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
        {

            // ���̧��ʱִ�еĲ���
            if (isMousePressed)
            {
                if (selectNode != null)
                {
                    bool isOk = selectNode.ClickNode();
                    if (isOk)
                    {
                        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>isOk");
                        CloseSelf();
                    }
                    
                }
            }
            // ����״̬
            isMousePressed = false;
        }
    }
    public void SetDataEx(UIPlayerStory uIPlayer)
    {
        LoadChapterMap();
        this.uIPlayer = uIPlayer;
        for (int i = 0; i < mapSceneNodes.Count; i++)   
        {
            mapSceneNodes[i].SetData(uIPlayer);
        }

    }
    void CheckMousePosition()
    {
    
        if (uiCamera == null)
        {
            Debug.LogError("UI Camera is not assigned!");
            return;
        }
      
        // ����Ƿ�����UIԪ����

        if (EventSystem.current.IsPointerOverGameObject()|| Application.platform != RuntimePlatform.WindowsEditor)
        {
            // ��ȡ�������Ļ�ϵ�λ��
            Vector3 mousePosition = Input.mousePosition;

            // ����һ������
            Ray ray = uiCamera.ScreenPointToRay(mousePosition);

         
      
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < mapSceneNodes.Count; i++)
            {
                needHides.Add(mapSceneNodes[i].obj);
            }
            selectNode = null;
            foreach (RaycastHit hit in hits)
            {
                // �����ײ���������Ƿ���Renderer���
                Image renderer = hit.collider.GetComponent<Image>();
                MapSceneNode mapScene = hit.collider.gameObject.GetComponent<MapSceneNode>();
                if (renderer != null)
                {
                    // ��ȡ���ָ��λ�õ���������

                    // ��ȡ����
                    Texture2D texture = renderer.sprite.texture;

                    if (texture != null)
                    {
                        Vector2 screenPos = Input.mousePosition;

                        // ����Ļ����ת��Ϊ Canvas �еı�������
                        Vector2 localPos;

                        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapScene.rect, screenPos, uiCamera, out localPos);
                        RectTransform imageRectTransform = renderer.rectTransform;
                        localPos.x = Mathf.Clamp(localPos.x, -imageRectTransform.rect.width / 2, imageRectTransform.rect.width / 2);
                        localPos.y = Mathf.Clamp(localPos.y, -imageRectTransform.rect.height / 2, imageRectTransform.rect.height / 2);
                        Vector2 pixelPos = new Vector2(
                                (localPos.x + imageRectTransform.rect.width / 2) / imageRectTransform.rect.width,
                                (localPos.y + imageRectTransform.rect.height / 2) / imageRectTransform.rect.height
                        );

                        Debug.Log(pixelPos);
                        // ת��Ϊ��������
                        int texX = Mathf.FloorToInt(pixelPos.x * texture.width);
                        int texY = Mathf.FloorToInt(pixelPos.y * texture.height);

                        // ��ȡ������ɫ
                        Color pixelColor = texture.GetPixel(texX, texY);
                        Color[] pixelsColor = texture.GetPixels();
                        //for (int i = 0; i < pixelsColor.Length; i++)
                        //{
                        //    Debug.Log(texture.name + pixelsColor[i]);

                        //}
                        Debug.Log(texture.name + pixelColor + pixelsColor.Length);

                        // ���͸����
                        if (pixelColor.a > 0)
                        {
                            selectNode = mapScene;
                            mapScene.obj.SetActive(true);
                            needHides.Remove(mapScene.obj);
                      
                            break;
                        }
                    }
                 
                }
              
              
               

            }
          
             HideSelectedObj();
        }
    }
    private void HideSelectedObj(GameObject obj =null)
    {
        for (int i = 0; i < needHides.Count; i++)
        {
            needHides[i].SetActive(false);
        }
        needHides.Clear();
    }
}
