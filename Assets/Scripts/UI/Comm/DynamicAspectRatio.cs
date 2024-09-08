using UnityEngine;
using UnityEngine.UI;

public class DynamicAspectRatio : MonoBehaviour
{
    private Image rawImage;

    // 设置的宽高比例（例如16:9）
    public float aspectRatio = 16f / 9f;

    void Start()
    {
       
        // 获取场景的宽高
        float sceneWidth = Screen.width;
        float sceneHeight = Screen.height;
        float pro = sceneWidth / sceneHeight * aspectRatio;
        float rawImageHeight = sceneWidth / aspectRatio;
        // 计算RawImage的宽度和高度
        float rawImageWidth = sceneWidth;
        // 获取RawImage的RectTransform组件
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        // 设置RawImage的宽高
        rectTransform.sizeDelta = new Vector2(rawImageWidth, rawImageHeight);
    }
}