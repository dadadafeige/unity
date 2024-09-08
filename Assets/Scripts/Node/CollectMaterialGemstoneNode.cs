using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ����һ��ί�����ͣ�������һ����������
// ����һ���¼���ʹ�������ί��������Ϊ�¼�������

public class CollectMaterialGemstoneNode : MonoBehaviour
{
    // Start is called before the first frame update
    private int itemId;

    public GemstoneEventHandler GemstoneNodeEvent;
    void Start()
    {
        
    }
    public void UpdateGemstone(int itemId)
    {
        this.itemId = itemId;
        Image image = gameObject.GetComponent<Image>();
        itmeconfigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item",itemId);
        image.sprite = UiManager.LoadSprite("item_icon", cfg.icon);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������������巢����ײʱִ�е��߼�
        // �������������赲Ч���Ĵ���
        GemstoneNodeEvent.Invoke(itemId,this, collision);
        Debug.Log("Collision occurred!");

    }
    private void OnDestroy()
    {
       

    }
}
