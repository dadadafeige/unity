using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIBag : UIBase
{
    public LoopScrollView loopScrollView;
    private List<ItemNode> items;
    public Button close_btn;
    public override void OnAwake()
    {
        items = BagManage.Instance.items;
        BagManage.Instance.onItemChangedCallback += UpdataChange;
    }
    public override void OnStart()
    {
        // ���ûص�����
        loopScrollView.F_SetOnItemLoadHandler(OnItemLoad);
        // ��ʼ��������Ԫ�ظ���
        loopScrollView.F_Init();
        loopScrollView.F_SetItemCount(40); // �����б��������
        close_btn.onClick.AddListener(CloseSelf);
    }
    private void UpdataChange()
    {
        items = BagManage.Instance.items;
        loopScrollView.F_UpdateItem();

    }

    private void OnItemLoad(GameObject obj, int index)
    {
        // ��������غ������б��������
        // ����Ҫ���� index ���ض�Ӧ������
        // obj ��������б���� GameObject����������������������ʾ����
        // ���磺obj.GetComponentInChildren<Text>().text = "Item " + index;
        Transform item_root = obj.transform.Find("item_root");
        if (items.Count > index &&  items[index] != null)
        {
            item_root.gameObject.SetActive(true);
            Image icon = item_root.Find("icon").GetComponent<Image>();
            TextMeshProUGUI num = item_root.Find("num").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI name = item_root.Find("name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI desc = item_root.Find("desc").GetComponent<TextMeshProUGUI>();
            Image use_btn_image = item_root.Find("use_btn").GetComponent<Image>();
            Button sell_btn = item_root.Find("sell_btn").GetComponent<Button>();
            num.text =Common.Instance.SplitNumber(items[index].count);
            if (items[index].mCfg == null)
            {
                items[index].mCfg = GetCfgManage.Instance.GetCfgByNameAndId<itmeconfigData>("item", items[index].id);
            }
            name.text = items[index].mCfg.name;
            desc.text = items[index].mCfg.desc;
            icon.sprite = UiManager.LoadSprite("item_icon", items[index].mCfg.icon);
            sell_btn.gameObject.SetActive(items[index].mCfg.price > 0);
            use_btn_image.gameObject.SetActive(items[index].mCfg.type == 1);
            if (items[index].mCfg.type == 1) //����Ʒ
            {
                use_btn_image.sprite = UiManager.LoadSprite("common", "use_btn");
                use_btn_image.raycastTarget = true;
            }
            else if (items[index].mCfg.price > 0)
            {
                sell_btn.onClick.RemoveAllListeners();
                sell_btn.onClick.AddListener(() =>
                {

                    UITipsBoard tipsBoard = UiManager.OpenUI<UITipsBoard>("UITipsBoard");
                    tipsBoard.SetData("�Ƿ����" + items[index].mCfg.name, items[index].mCfg.price * items[index].count, () => {
                        GameManage.userData.SetAddGoldValue(items[index].mCfg.price * items[index].count);
                        BagManage.Instance.SubItemByItemAndNum(items[index], items[index].count);

                    });


               

                });
            }
            else
            {
                use_btn_image.sprite = UiManager.LoadSprite("common", "grey_use_btn");
                use_btn_image.raycastTarget = false;
            }
        }
        else
        {
            item_root.gameObject.SetActive(false);
        }

    }
    public override void OnDestroyImp()
    {
        BagManage.Instance.onItemChangedCallback -= UpdataChange;
    }


}
