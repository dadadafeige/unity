using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleInfo : UIBase
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI hp;
    //public TextMeshProUGUI magic;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI exp;
    public TextMeshProUGUI magic_resistance;
    public TextMeshProUGUI fighting;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI explain;
    public TextMeshProUGUI explain_title;
    public Button close_btn;
    public Button tab1;
    public Button tab2;
    public RectTransform pet1;
    public RectTransform pet2;
    public GameObject player_root;
    public GameObject pet_root;
    public TextMeshProUGUI player_name;
    public RawImage role_head;
    // Start is called before the first frame update
    public override void OnStart()
    {
        tab1.onClick.AddListener(() => SwitchTab(1));
        tab2.onClick.AddListener(() => SwitchTab(2));
        InfoPlayer();
        // Initialize to show the first tab
        SwitchTab(1);
        PetNode pet11 = GameManage.GetPetById(1);
        PetNode pet22 = GameManage.GetPetById(2);
        LvUpate(pet11);
        LvUpate(pet22);
        pet22.updeaPetLvExp += LvUpate;
        pet11.updeaPetLvExp += LvUpate;
        close_btn.onClick.AddListener(CloseSelf);
    }
    void SwitchTab(int tabIndex)
    {
        // Hide all panels
        player_root.SetActive(false);
        pet_root.SetActive(false);


        // Show the selected panel
        switch (tabIndex)
        {
            case 1:
                player_root.SetActive(true);
                break;
            case 2:
                pet_root.SetActive(true);
                break;
    
        }
    }
    //public TextMeshProUGUI level;
    //public TextMeshProUGUI hp;
    //public TextMeshProUGUI magic;
    //public TextMeshProUGUI attack;
    //public TextMeshProUGUI faqiang;
    //public TextMeshProUGUI magic_resistance;
    //public TextMeshProUGUI fighting;
    //public TextMeshProUGUI defense;
    //public TextMeshProUGUI explain;
    void InfoPlayer()
    {
        UserData userData = GameManage.userData;
    
        player_attributecnofigData player_ = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", userData.level);
        player_attributecnofigData next_player_ = GetCfgManage.Instance.GetCfgByNameAndId<player_attributecnofigData>("player_attribute", userData.level + 1);
        hp.text = player_.hp.ToString();
        level.text = userData.level.ToString() + "  " + player_.title;
        attack.text = player_.attack.ToString();
        fighting.text = player_.fight.ToString();
        defense.text = player_.defense.ToString();
        player_name.text = userData.userName;
        exp.text = (userData.exp - player_.exp) + "/" + (next_player_.exp - player_.exp);
        explain.text = player_.title_explain;
        explain_title.text = player_.title;
        explain.gameObject.SetActive(true);
        magic_resistance.text = player_.magic_resistance.ToString();
        if (userData.userGender == Gender.Boy)
        {
            role_head.texture = UiManager.getTextureByNmae("info_texture", "PC_Man");
        }
        else
        {
            role_head.texture = UiManager.getTextureByNmae("info_texture", "PC_FeMale");
        }

    }
    void LvUpate(PetNode pet)
    {
        if (pet.id == 1)
        {
            InfoPet(pet, pet1);
        }
        else
        {
            InfoPet(pet, pet2);
        }
      


    }

    void InfoPet(PetNode pet, RectTransform petRe)
    {
      
        TextMeshProUGUI pet_lv = petRe.Find("pet_lv").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pet_attack = petRe.Find("pet_attack").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pet_magic_resistance = petRe.Find("pet_magic_resistance").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pet_faqiang = petRe.Find("pet_faqiang").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pet_defense = petRe.Find("pet_defense").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI jinbi = petRe.Find("upgrade_btn/jinbi").GetComponent<TextMeshProUGUI>();
        Button upgrade_btn = petRe.Find("upgrade_btn").GetComponent<Button>();
        Image upgrade_ima = petRe.Find("upgrade_btn").GetComponent<Image>();
        DragonBonesController dragon = petRe.Find("pet_lvup_bones").GetComponent<DragonBonesController>();
        pet_lv.text = pet.level.ToString();
        pet_attack.text = pet.petLvCfg.attack.ToString();
        pet_magic_resistance.text = pet.petLvCfg.magic_resistance.ToString();
        pet_faqiang.text = pet.petLvCfg.faqiang.ToString();
        pet_defense.text = pet.petLvCfg.defense.ToString();
        if (pet.level < 5)
        {
            pet_lvconfigData petLvCfg = GetCfgManage.Instance.GetCfgByNameAndId<pet_lvconfigData>("pet_lv", pet.id * 10000 + pet.level + 1);
            if (petLvCfg.exp > GameManage.userData.gold)
            {

                jinbi.text = "<color=#FF2222>" + petLvCfg.exp.ToString() + "</color>";
                upgrade_ima.sprite = UiManager.LoadSprite("info_atlas", "info_5");
                //info_atlas    info_5
            }
            else
            {
                jinbi.text = petLvCfg.exp.ToString();
            }
            upgrade_btn.onClick.RemoveAllListeners();
            upgrade_btn.onClick.AddListener(() =>
            {

                if (petLvCfg.exp > GameManage.userData.gold)
                {
                    return;
                }

                UITipsBoard tipsBoard = UiManager.OpenUI<UITipsBoard>("UITipsBoard");

                tipsBoard.SetData("升级" + pet.petconfig.name + "需要", petLvCfg.exp, () =>
                {
                    pet.UpLv();
                    GameManage.userData.SetAddGoldValue(0 - petLvCfg.exp);
                    Common.Instance.ShowTips("升级成功");
                    dragon.gameObject.SetActive(true);
                    dragon.PlayAnimation("01_ShengJi", false, () =>
                    {
                        dragon.gameObject.SetActive(false);
                    });

                });
            });
        }
      
        if (pet.level >= 5)
        {
            upgrade_btn.gameObject.SetActive(false);
        }
        







    }
    public override void OnDestroyImp()
    {
        PetNode pet11 = GameManage.GetPetById(1);
        PetNode pet22 = GameManage.GetPetById(2);
        pet11.updeaPetLvExp -= LvUpate;
        pet22.updeaPetLvExp -= LvUpate;
    }

    // Update is called once per frame

}
