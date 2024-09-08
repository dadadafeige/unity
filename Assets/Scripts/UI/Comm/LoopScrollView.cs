using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScrollView : MonoBehaviour
{
    public enum Direction
    {
        None,
        Vertical,
        Horizontal,
    }

    private enum ItemDirection
    {
        Top,
        Center,
        Bottom,
    }

    private class ScrollItem
    {
        public int V_Index;
        public GameObject gameObject;
        /// <summary>
        /// 是否可见
        /// </summary>
        private bool m_bVisible = false;
        public bool V_bVisible
        {
            get => m_bVisible;
            set
            {
                if (gameObject != null && gameObject.activeSelf != value)
                    gameObject.SetActive(value);
                m_bVisible = value;
            }
        }
        public ScrollItem(GameObject obj) => gameObject = obj;
    }

    public delegate void ItemLoad(GameObject obj, int index);

    [SerializeField] private ScrollRect m_ScrollRect;
    [SerializeField, Header("为空时默认为 ScrollRect 的 Content")] private RectTransform m_Content;
    [SerializeField, Header("顶部边距")] private float m_TopDistance = 0;
    [SerializeField, Header("格子大小")] private Vector2 m_CellSize;
    [SerializeField, Header("间距")] private Vector2 m_Spacing;
    [SerializeField, Header("一行个数")] private int m_SplitNum = 1;
    [SerializeField, Header("默认和ScrollRect 相同")] private Direction m_Direction = Direction.None;
    [SerializeField, Header("最后一行是否铺满")] private bool m_bBespread = false;
    [SerializeField, Header("物品")] private GameObject m_Item;

    private bool m_bInit = false;
    /// <summary>
    /// 最大创建元素个数
    /// </summary>
    private int m_MaxItem = 0;
    /// <summary>
    /// 元素个数
    /// </summary>
    private int m_Count;
    /// <summary>
    /// scrollRect content 展示范围 滑动方向的长度
    /// </summary>
    private float m_ScrollLength;
    private float m_ContentLength;

    /// <summary>
    /// 屏幕未满时不需要滚动
    /// </summary>
    private bool m_bNeedScroll = false;
    /// <summary>
    /// 最后一次展示的索引范围
    /// </summary>
    private int m_StartIndex = 0;
    private int m_EndIndex = -1;

    private ItemLoad OnItemLoadHandler;

    private List<ScrollItem> m_ItemList;
    private void Awake()
    {
        if (m_ScrollRect == null)
            m_ScrollRect = GetComponent<ScrollRect>();
        if (m_Content == null && m_ScrollRect != null)
            m_Content = m_ScrollRect.content;
    }
    private void OnEnable()
    {
        if (m_ScrollRect)
            m_ScrollRect.onValueChanged.AddListener(OnUpdate);
    }
    private void OnDisable()
    {
        if (m_ScrollRect)
            m_ScrollRect.onValueChanged.RemoveListener(OnUpdate);
    }

    #region 接口
    public void F_Init()
    {
        if (m_bInit)
            return;
        if (m_ScrollRect == null || m_Item == null)
            return;
        if (m_Direction == Direction.None)
            m_Direction = m_ScrollRect.vertical ? Direction.Vertical : Direction.Horizontal;
        Vector2 size = m_ScrollRect.GetComponent<RectTransform>().sizeDelta;
        m_ScrollLength = Math.Abs(m_Direction == Direction.Vertical ? size.y : size.x);
        m_MaxItem = m_SplitNum * (Mathf.CeilToInt(m_ScrollLength / (m_Direction == Direction.Vertical ? (m_CellSize.y + m_Spacing.y) : (m_CellSize.x + m_Spacing.x))) + 1);
        m_ItemList = new List<ScrollItem>(m_MaxItem);
        m_bInit = true;
    }

    /// <summary>
    /// 设置加载回调
    /// </summary>
    /// <param name="load"></param>
    public void F_SetOnItemLoadHandler(ItemLoad load)
    {
        OnItemLoadHandler = load;
    }

    public void F_SetItemCount(int count)
    {
        m_Count = count;
        SetContentSize();
        MoveItem(0, ItemDirection.Top);
        OnUpdate(true);
    }
    public void F_AddItem(int count = 1)
    {
        m_Count += count;
        SetContentSize();
        OnUpdate(true);
    }
    public void F_DelItem(int count = 1)
    {
        m_Count -= count;
        SetContentSize();
        float pos = m_Direction == Direction.Vertical ? m_Content.anchoredPosition.y : -m_Content.anchoredPosition.x;
        if (m_ContentLength <= m_ScrollLength)
            SetContentPos(0);
        else if (pos > m_ContentLength - m_ScrollLength)
            SetContentPos(m_ContentLength - m_ScrollLength);
        OnUpdate(true);
    }

    public void F_SetItemTop(int index)
    {
        if (MoveItem(index, ItemDirection.Top))
            OnUpdate();
    }
    public void F_SetItemCenter(int index)
    {
        if (MoveItem(index, ItemDirection.Center))
            OnUpdate();
    }
    public void F_SetItemBottom(int index)
    {
        if (MoveItem(index, ItemDirection.Bottom))
            OnUpdate();
    }

    /// <summary>
    /// 全部更新
    /// </summary>
    public void F_UpdateItem()
    {
        UpdateVisible(m_StartIndex, m_EndIndex, true);
    }

    public void F_Clear()
    {
        UpdateVisible(0, -1, false);
        MoveItem(0, ItemDirection.Top);
        m_Count = 0;
    }
    public void F_StopMovement()
    {
        if (m_ScrollRect)
            m_ScrollRect.StopMovement();
    }

    /// <summary>
    /// 返回所有显示的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> F_GetAllItem<T>() where T : MonoBehaviour
    {
        List<T> list = new List<T>();
        ScrollItem item;
        for (int i = 0; i < m_ItemList.Count; i++)
        {
            item = m_ItemList[i];
            if (item.V_bVisible && item.gameObject.GetComponent<T>() != null)
                list.Add(item.gameObject.GetComponent<T>());
        }
        return list;
    }

    /// <summary>
    /// 遍历执行事件
    /// 不想全部更新的时候用
    /// 
    /// 道具全部更新可能会更新道具id，图标，数量等
    /// 这个方法直接返回脚本，里面的方法任意选择调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    public void F_ForeachAllItem<T>(System.Action<T, int> action)
    {
        if (action == null)
            return;
        ScrollItem item;
        for (int i = 0; i < m_ItemList.Count; i++)
        {
            item = m_ItemList[i];
            if (item.V_bVisible && item.gameObject.GetComponent<T>() != null)
                action(item.gameObject.GetComponent<T>(), item.V_Index);
        }
    }
    #endregion

    /// <summary>
    /// 滑动时更新
    /// </summary>
    /// <param name="v"></param>
    private void OnUpdate(Vector2 v)
    {
        if (m_bNeedScroll)
            OnUpdate();
    }

    private void OnUpdate(bool isReset = false)
    {
        if (!m_bInit)
            return;
        int start, end;
        int last = m_Count % m_SplitNum;
        if (m_Count != 0 && m_bNeedScroll)
        {
            float pos, cellSize, spacingSize;
            if (m_Direction == Direction.Vertical)
            {
                pos = m_Content.anchoredPosition.y;
                cellSize = m_CellSize.y;
                spacingSize = m_Spacing.y;
            }
            else
            {
                pos = -m_Content.anchoredPosition.x;
                cellSize = m_CellSize.x;
                spacingSize = m_Spacing.x;
            }
            // 滑到最上面
            if (pos <= 0 || pos - m_TopDistance <= 0)
                start = 0;
            // 滑到最下面 最后一行如果不满 就不需要显示最上面的几个
            else if (pos >= m_ContentLength - m_ScrollLength)
                start = Mathf.Max(0, m_Count - m_MaxItem + (last == 0 ? 0 : m_SplitNum - last));
            // 滑动在中间
            else
                start = Mathf.FloorToInt((pos - m_TopDistance) / (cellSize + spacingSize)) * m_SplitNum;
        }
        else
        {
            start = 0;
        }
        end = Mathf.Min(start + m_MaxItem, m_Count + (!m_bBespread || last == 0 ? 0 : m_SplitNum - last)) - 1;
        UpdateVisible(start, end, isReset);
    }

    /// <summary>
    /// 更新可见
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void UpdateVisible(int start, int end, bool isReset)
    {
        ScrollItem item;
        int add = end - start + 1 - m_ItemList.Count;
        // 创建的 item 不够用 先补上
        if (add > 0)
        {
            for (int i = 0; i < add; i++)
            {
                m_ItemList.Add(CreateItem());
            }
        }

        int index = start;
        if (index >= m_StartIndex && index <= m_EndIndex)
            index = m_EndIndex + 1;
        for (int i = 0; i < m_ItemList.Count; i++)
        {
            item = m_ItemList[i];
            if (item.V_bVisible && item.V_Index >= start && item.V_Index <= end)
            {
                if (isReset)
                    OnItemLoadHandler?.Invoke(item.gameObject, item.V_Index);
            }
            else
            {
                if (index <= end)
                {
                    item.V_Index = index;
                    item.V_bVisible = true;
                    SetItemPos(item);
                    OnItemLoadHandler?.Invoke(item.gameObject, item.V_Index);
                    index++;
                    if (index >= m_StartIndex && index <= m_EndIndex)
                        index = m_EndIndex + 1;
                }
                else
                {
                    item.V_bVisible = false;
                }
            }
        }
        m_StartIndex = start;
        m_EndIndex = end;
    }

    private ScrollItem CreateItem()
    {
        GameObject obj = GameObject.Instantiate(m_Item);
        obj.transform.SetParent(m_Content);
        obj.transform.localScale = m_Item.transform.localScale;
        obj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        return new ScrollItem(obj);
    }

    /// <summary>
    /// 设置 滑动高度
    /// </summary>
    private void SetContentSize()
    {
        int len = m_Count / m_SplitNum + (m_Count % m_SplitNum != 0 ? 1 : 0);
        if (m_Direction == Direction.Vertical)
        {
            m_ContentLength = m_TopDistance + len * m_CellSize.y + (len - 1) * m_Spacing.y;
            m_Content.sizeDelta = new Vector2(m_Content.sizeDelta.x, Mathf.Max(m_ScrollLength, m_ContentLength));
        }
        else
        {
            m_ContentLength = m_TopDistance + len * m_CellSize.x + (len - 1) * m_Spacing.x;
            m_Content.sizeDelta = new Vector2(Mathf.Max(m_ScrollLength, m_ContentLength), m_Content.sizeDelta.y);
        }
        m_bNeedScroll = m_ContentLength > m_ScrollLength;
    }

    /// <summary>
    /// 滑动到 item 位置
    /// </summary>
    /// <param name="index"></param>
    /// <param name="itemDirection"></param>
    /// <returns></returns>
    private bool MoveItem(int index, ItemDirection itemDirection)
    {
        if (!m_bInit)
            return false;
        Vector2 v = GetItemPos(index);
        float p = 0;
        if (m_ContentLength >= m_ScrollLength)
        {
            // 竖向 y 坐标是负值
            switch (itemDirection)
            {
                case ItemDirection.Top:
                    p = m_Direction == Direction.Vertical ? -v.y - m_CellSize.y / 2 : v.x - m_CellSize.x / 2;
                    break;
                case ItemDirection.Center:
                    p = m_Direction == Direction.Vertical ? -v.y - m_ScrollLength / 2 : -v.x - m_ScrollLength / 2;
                    break;
                case ItemDirection.Bottom:
                    p = m_Direction == Direction.Vertical ? -v.y + m_CellSize.y / 2 - m_ScrollLength : v.x + m_CellSize.x / 2 - m_ScrollLength;
                    break;
                default:
                    p = 0;
                    break;
            }
            p = Mathf.Clamp(p, 0, m_ContentLength - m_ScrollLength);
        }
        SetContentPos(p);
        return true;
    }

    /// <summary>
    /// 滑动到某个位置
    /// </summary>
    /// <param name="pos"></param>
    private void SetContentPos(float pos)
    {
        if (m_Direction == Direction.Vertical)
            m_Content.anchoredPosition = new Vector2(m_Content.anchoredPosition.x, pos);
        else
            m_Content.anchoredPosition = new Vector2(-pos, m_Content.anchoredPosition.y);
        F_StopMovement();
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="item"></param>
    private void SetItemPos(ScrollItem item)
    {
        item.gameObject.GetComponent<RectTransform>().anchoredPosition = GetItemPos(item.V_Index);
    }
    private Vector2 GetItemPos(int index)
    {
        // 第 line 行 第 num 个
        int line = index / m_SplitNum;
        int num = index % m_SplitNum;
        float x, y;
        bool bOdd = m_SplitNum % 2 == 1;
        float w;
        if (m_Direction == Direction.Vertical)
        {
            w = m_CellSize.x + m_Spacing.x;
            // 竖着 y 坐标递减，x 坐标从左往右递增
            y = -(m_TopDistance + m_CellSize.y / 2 + line * m_CellSize.y + line * m_Spacing.y);
            if (bOdd)
                x = (num - m_SplitNum / 2) * w;
            else
                x = (num - (m_SplitNum - 1) / 2f) * w;
        }
        else
        {
            w = m_CellSize.y + m_Spacing.y;
            // 横着 x 坐标递增，y 坐标从上到下递减
            x = m_TopDistance + m_CellSize.x / 2 + line * m_CellSize.x + line * m_Spacing.x;
            if (bOdd)
                y = (m_SplitNum / 2 - num) * w;
            else
                y = ((m_SplitNum - 1) / 2f - num) * w;
        }
        return new Vector2(x, y);
    }

    void OnValidate()
    {
        if (!Application.isPlaying && gameObject.activeInHierarchy && m_Content != null)
        {
            List<RectTransform> list = new List<RectTransform>();
            for (int i = 0; i < m_Content.childCount; i++)
            {
                RectTransform t = (RectTransform)m_Content.GetChild(i);
                if (t && t.gameObject.activeInHierarchy)
                    list.Add(t);
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].anchoredPosition = GetItemPos(i);
            }
        }
    }
}
