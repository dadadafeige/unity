using System;
using UnityEngine;
using UnityEngine.UI;

namespace TT
{
    public class RawAutoScaleBehaviour:MonoBehaviour
    {
        private void Start()
        {
            updateSize();
        }

        public void updateSize()
        {
            RectTransform  selfRectTransform = GetComponent<RectTransform>();
            GameObject uiRoot = GameObject.Find("UIRoot");
            RectTransform uiRootRectTransform = uiRoot.GetComponent<RectTransform>();
            
            // UnityEngine.UI.CanvasScaler scaler = uiRoot.GetComponent<UnityEngine.UI.CanvasScaler>();
            // float org = scaler.referenceResolution.x / scaler.referenceResolution.y;
            // float org1 = uiRootRectTransform.rect.width / uiRootRectTransform.rect.height;
            
            float width = selfRectTransform.sizeDelta.x;
            float height = selfRectTransform.sizeDelta.y;
            if (uiRootRectTransform.rect.height > height)
            {
                float scale = uiRootRectTransform.rect.height / height;
                selfRectTransform.sizeDelta = new Vector2(selfRectTransform.rect.width* scale,
                    uiRootRectTransform.rect.height);
            }else if (uiRootRectTransform.rect.width > width)
            {
                float scale = uiRootRectTransform.rect.width / width;
                selfRectTransform.sizeDelta = new Vector2(uiRootRectTransform.rect.width,
                    selfRectTransform.rect.height* scale);
            }
        }
    }
}