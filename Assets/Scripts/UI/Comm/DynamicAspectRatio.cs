using UnityEngine;
using UnityEngine.UI;

public class DynamicAspectRatio : MonoBehaviour
{
    private Image rawImage;

    // ���õĿ�߱���������16:9��
    public float aspectRatio = 16f / 9f;

    void Start()
    {
       
        // ��ȡ�����Ŀ��
        float sceneWidth = Screen.width;
        float sceneHeight = Screen.height;
        float pro = sceneWidth / sceneHeight * aspectRatio;
        float rawImageHeight = sceneWidth / aspectRatio;
        // ����RawImage�Ŀ�Ⱥ͸߶�
        float rawImageWidth = sceneWidth;
        // ��ȡRawImage��RectTransform���
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        // ����RawImage�Ŀ��
        rectTransform.sizeDelta = new Vector2(rawImageWidth, rawImageHeight);
    }
}