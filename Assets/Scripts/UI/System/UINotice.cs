using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UINotice : UIBase
{
    public Button close_btn;
    public TextMeshProUGUI words;
    public TextMeshProUGUI title;
    // Start is called before the first frame update
    public override void OnAwake()
    {
   
    }
    // Start is called before the first frame update
    public override void OnStart()
    {
        close_btn.onClick.AddListener(CloseSelf);
    }
    public void SetLinkWords(int linkId)
    {
        linkconfigData m_cfg = GetCfgManage.Instance.GetCfgByNameAndId<linkconfigData>("link", linkId);
        words.text = m_cfg.link_words.Replace("\\n", "\n");
        title.text = m_cfg.title;
        words.ForceMeshUpdate();

    }
}
