using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 定义一个委托类型，它接受一个整数参数
// 定义一个事件，使用上面的委托类型作为事件的类型

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
        // 当物体与其他物体发生碰撞时执行的逻辑
        // 在这里可以添加阻挡效果的代码
        GemstoneNodeEvent.Invoke(itemId,this, collision);
        Debug.Log("Collision occurred!");

    }
    private void OnDestroy()
    {
       

    }
}
