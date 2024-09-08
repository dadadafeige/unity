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
    public int direction = 0;//0 = �� 1== ��  2==�� 3==��
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������������巢����ײʱִ�е��߼�
        // �������������赲Ч���Ĵ���
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
