using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 
public class UniformScaleUIContainer: MonoBehaviour
{
        
    private void Awake()
    {
          
          
    }
 
    private void OnRectTransformDimensionsChange()
    {
        RectTransform  selfRectTransform = GetComponent<RectTransform>();
        GameObject uiRoot = GameObject.Find("UIRoot");
        RectTransform uiRootRectTransform = uiRoot.GetComponent<RectTransform>();
            
        float rate = (float)Screen.width / Screen.height;
        // UnityEngine.UI.CanvasScaler scaler = uiRoot.GetComponent<UnityEngine.UI.CanvasScaler>();
        // float org = scaler.referenceResolution.x / scaler.referenceResolution.y;
        float org = selfRectTransform.sizeDelta.x / selfRectTransform.sizeDelta.y;
        if (rate <= org)
        {
            float height = selfRectTransform.sizeDelta.y;
            float scale = uiRootRectTransform.rect.height / height;
            selfRectTransform.sizeDelta = new Vector2(selfRectTransform.rect.width* scale,
                uiRootRectTransform.rect.height);
        }else
        {
            float width = selfRectTransform.sizeDelta.x;
            float scale = uiRootRectTransform.rect.width / width;
            selfRectTransform.sizeDelta = new Vector2(uiRootRectTransform.rect.width,
                selfRectTransform.rect.height* scale);
        }
    }
    //
    // private void RefreshContent ()
    // {
    //     if (!isInitOver) return;
    //     //计算自身当前的高宽比
    //     float selfAspectRatio = selfRectTransform.rect.height / selfRectTransform.rect.width;
    //     float scaleValue = 1;
    //     //适配标准高宽比
    //     if (selfAspectRatio > aspectRatio)
    //     {
    //         scaleValue = selfRectTransform.rect.width * aspectRatio / contentReferenceResolution.y;
    //     } else if (selfAspectRatio < aspectRatio)
    //     {
    //         scaleValue = selfRectTransform.rect.height / aspectRatio / contentReferenceResolution.x;
    //     }
    //     //计算内容scale
    //     contentRectTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    // }
}
