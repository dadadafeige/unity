using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public Action destroyCallback = null;
    private void Awake()
    {
       

        OnAwake();
    }
    private void OnEnable()
    {
        
    }
    private void Start()
    {
        OnStart();
    }
    public float ratio = 1;
    public void SearchForImages(Transform parent)
    {
       
        if (Screen.height < 1080 && UIOrder == UIOrderType.UINoFull)
        {
            ratio = (float)Screen.height / 1080;
            if (this.name != "UIGuessingPuzzle" && this.name != "UIBalanceBall")
            {

                if (ratio < 0.7)
                {
                    ratio = 0.7f;
                }
            }

        }
        
        // 遍历子节点
        foreach (Transform child in parent)
        {
            // 获取当前子节点的Image组件（如果存在）
            Image image = child.GetComponent<Image>();

            if (image != null)
            {
                Sprite sprite = UiManager.GetImageReference(gameObject.name, image.name);
                if (sprite != null)
                {
                    image.sprite = sprite;
                }
                // 找到了Image组件，可以在这里进行相应的操作
        //        Debug.Log("Found Image component: " + image.name);
                if (Screen.height < 1080 && UIOrder == UIOrderType.UINoFull)
                {
                    if (image.rectTransform.anchorMin == Vector2.zero && image.rectTransform.anchorMax == Vector2.one)
                    {
                        image.transform.localScale = new Vector3(1 / ratio, 1 / ratio, 1 / ratio);
                    }

                }
            }

            // 继续递归搜索当前子节点的子节点
            SearchForImages(child);
        }
    }

    public void InitImage()
    {
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            Sprite sprite = UiManager.GetImageReference(gameObject.name, images[i].name);
            if (sprite != null)
            {
                images[i].sprite = sprite;
            }

        }

    }
    // Start is called before the first frame update
    public UIOrderType UIOrder = UIOrderType.UIDefault;
   public GameObject InstantiateUI(GameObject myPrefab)
    {
        GameObject obj = Instantiate(myPrefab);
        return obj; 

    }
    public virtual void OnAwake()
    {



    }
    public virtual void OnStart()
    {



    }
    public GameObject LoadSelf(GameObject myPrefab)
    {
        GameObject obj = InstantiateUI(myPrefab);
        Canvas cav = obj.GetComponent<Canvas>();
        cav.overrideSorting = true;
        
        // cav.sortingLayerName = sortingLayerName; 
        cav.sortingLayerName = "Default";
        return obj;

    }
    public void CloseSelf()
    {

        UiManager.CloseUI(this);


    }
    public virtual void OutUI()
    {



    }
    public virtual void GoInUI()
    {



    }
    public virtual void OnDestroyImp()
    {


    }
    public void OnDestroy()
    {
        OnDestroyImp();
        if (destroyCallback != null)
        {
            destroyCallback.Invoke();
        }
    }
    public void SetDestroyCallback(Action destroyCallback)
    {
        this.destroyCallback = destroyCallback;
    }
}
