using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectMaterialEnemyNode : MonoBehaviour
{

    public EnemyEventHandler EnemyNodeEvent;
    public CollectMaterialGridNode gridNode;
    public Tween tween;
    public float speed = 1;
    public int direction = 0;//0 = 上 1== 下  2==左 3==右
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当物体与其他物体发生碰撞时执行的逻辑
        // 在这里可以添加阻挡效果的代码
        EnemyNodeEvent.Invoke(collision,this);
      
    }
    public void UpdateEnemy()
    {
        int randomNumber = UnityEngine.Random.Range(1, 4);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = UiManager.LoadSprite("collect_material", "collect_material_enemy"+ randomNumber);

    }
    private void OnDestroy()
    {
        //System.Delegate[] dels = EnemyNodeEvent.GetInvocationList();
        //for (int i = 0; i < dels.Length; i++)
        //{
        //    EnemyNodeEvent -= dels[i] as EnemyEventHandler;
        //}

    }

}
