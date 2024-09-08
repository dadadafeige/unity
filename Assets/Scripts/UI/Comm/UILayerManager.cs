using UnityEngine;

// 节点类
public class UILayerNode
{
    public UIBase uiObject;
    public UILayerNode nextNode;

    public UILayerNode(UIBase obj)
    {
        uiObject = obj;
        nextNode = null;
    }
}

// 管理器类
public class UILayerManager
{
    private UILayerNode headNode; // 链表头节点
    private static UILayerManager instance;
    private int uiNum = 0;
    private int uiTopNum = 1000;
    public static UILayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UILayerManager();
            }
            return instance;
        }
    }
    // 添加UI对象到链表中
    public void AddUIObject(UIBase uiObject)
    {
        UILayerNode newNode = new UILayerNode(uiObject);
        newNode.nextNode = headNode;
        if (headNode != null)
        {
            headNode.uiObject.OutUI();
        }
        headNode = newNode;
        if (newNode.uiObject.UIOrder == UIOrderType.UITop)
        {
            uiTopNum++;
        }
        else
        {
            uiNum++;
        }
      
        UpdateUILayer(); // 更新UI层级
    }

    // 移除UI对象从链表中
    public void RemoveUIObject(UIBase uiObject)
    {
        UILayerNode currentNode = headNode;
        UILayerNode prevNode = null;
        uiNum--;
        while (currentNode != null)
        {
            if (currentNode.uiObject == uiObject)
            {
                if (prevNode != null)
                {
                    prevNode.nextNode = currentNode.nextNode;
                }
                else
                {
                    headNode = currentNode.nextNode;
                }
                headNode.uiObject.GoInUI();
                UpdateUILayer(); // 更新UI层级
                return;
            }

            prevNode = currentNode;
            currentNode = currentNode.nextNode;
        }
    }

    // 更新UI层级
    private void UpdateUILayer()
    {
        int layer = uiNum;
        int top_layer = uiTopNum;
        UILayerNode currentNode = headNode;

        while (currentNode != null)
        {
            if (currentNode.uiObject != null)
            {
                Canvas canvas = currentNode.uiObject.GetComponent<Canvas>();
           
                if (canvas != null)
                {
                    if (currentNode.uiObject.UIOrder == UIOrderType.UITop)
                    {
                        canvas.sortingOrder = top_layer--;
                    }
                    else
                    {
                        canvas.sortingOrder = layer--;
                    }
                  
                }
            }
         
            currentNode = currentNode.nextNode;
        }
    }
}
