using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTopNode : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI expLabel;
    public TextMeshProUGUI goldLabel;
    public TextMeshProUGUI levelLabel;
    public RectTransform exp;
    public RectTransform gold;
    public RectTransform level;
    void Start()
    {
        GameManage.userData.updeaUserExp += UpdateExpValue;
        GameManage.userData.updeaGoldExp += UpdateGoldValue;
        UpdateExpValue();
        UpdateGoldValue();
    }

    private void UpdateExpValue()
    {
        expLabel.text = GameManage.userData.exp.ToString();
        levelLabel.text = GameManage.userData.level.ToString();
    }
    private void UpdateGoldValue()
    {
        goldLabel.text = GameManage.userData.gold.ToString();
    }
}
