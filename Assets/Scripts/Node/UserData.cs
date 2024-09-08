using DragonBones;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UserData:IGameData
{
    public string userName; //�������
    public Gender userGender;//����Ա�
    public int level = 1;//��ҵȼ�
    public int exp = 0;
    public int gold = 0;
    public int unlockChapter = 1;
    public int unlockMissionId = 1;
    public bool is_first = true;
    public int needDot = 3;

    public delegate void UpdeaUserExp();
    // �����ж�����
    [NonSerialized]
    public UpdeaUserExp updeaUserExp;
    public delegate void UpdeaGoldExp();
    // �����ж�����
    [NonSerialized]
    public UpdeaGoldExp updeaGoldExp;
    [NonSerialized]
    public player_attributecnofigData player_cfg;

    // ������������û���Ϣ���ֶ�
    public UserData()
    {
      //  player_cfg = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", level);

    }
    // ���캯�����ڳ�ʼ��
    public bool SetAddExpValue(int value)
    {
        player_attributecnofigData next_player_cfg = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", level + 1);
       
        exp += value;
        if (next_player_cfg.exp <= exp)
        {
            updeaUserExp?.Invoke();
            level++;
    
            return true;
        }
        GameManage.SaveUserData();
        updeaUserExp?.Invoke();
        return false;
    }

    public void SetUnlock(int unlockMissionId , int unlockChapter,Action action)
    {

        if (unlockChapter > this.unlockChapter)
        {
            this.unlockChapter = unlockChapter;
            chapterconfigData cfg = GetCfgManage.Instance.GetCfgByNameAndId<chapterconfigData>("chapter", unlockChapter);
            List<DropNode> itemIdList = DropManager.Instance.GetDropItemId2(cfg.drop_id);
            bool isUp = SetAddExpValue(itemIdList[0].count);
            needDot++;
            if (isUp)
            {
                UILevelUp lvUI = UiManager.OpenUI<UILevelUp>("UILevelUp");
                lvUI.SetData(level);
                lvUI.SetDestroyCallback(() =>
                {
                    UISwitchChapters uISwitch = UiManager.OpenUI<UISwitchChapters>("UISwitchChapters");
                    if (action != null)
                    {
                        uISwitch.SetDestroyCallback(() =>
                        {
                            action.Invoke();

                        });
                    }

                });
            }
            else
            {
                UISwitchChapters uISwitch = UiManager.OpenUI<UISwitchChapters>("UISwitchChapters");
                if (action != null)
                {
                    uISwitch.SetDestroyCallback(() =>
                    {
                        action.Invoke();

                    });
                }
            }
            
            UiManager.uIPlayer.map_btn.gameObject.GetComponent<Image>().sprite = UiManager.getTextureSpriteByNmae("map_btn_texture", "map_btn" + unlockChapter);
        }
        else
        {
            action.Invoke();
        }
        if (unlockMissionId > this.unlockMissionId)
        {
            this.unlockMissionId = unlockMissionId;
        }

        GameManage.SaveUserData();

    }

    public void SetAddGoldValue(int value)
    {
        gold += value;
        updeaGoldExp?.Invoke();
        GameManage.SaveUserData();
    }
}


// �����Ա��ö��
[System.Serializable]
public enum Gender
{
    Boy,
    Girl,
    Other
}