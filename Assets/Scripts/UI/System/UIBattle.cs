

using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBattle : UIBase
{
    public Image player_hp;
    public Image head;
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI level;
    public RectTransform energy_grid_root;
    public GameObject enemy_item;
    public RectTransform enemy_root;
    public Button battle_attack_btn;
    public Button battle_bag_btn;
    public Button battle_skill_btn;
    public Button battle_escape_btn;
    public Button battle_auto_btn;
    public TextMeshProUGUI round_num;
    private List<GameObject> enemyObjList = new List<GameObject>();
    List<Enemy> enemies;
    List<Pet> pets;
    PlayertCharacterNode playert;
    public List<RectTransform>  enemy_bones_list = new List<RectTransform>();
    public List<RectTransform> pet_bones_list = new List<RectTransform>();
    public RectTransform player_bones_root;
    public TextMeshProUGUI player_damage;
    private DragonBonesController player_bones;
    public RectTransform player_bones_base;
    public GameObject make;
    public GameObject operate_root;
    public RectTransform beaten_effect_root;
    public List<GameObject> energy_grid_list;
    private Dictionary<string, DragonBonesController> beaten_effect_map = new Dictionary<string, DragonBonesController>();
    private List<MapSceneNode> btnImageList = new List<MapSceneNode>();
    private bool isGoOthen = false;
    private bool isMousePressed = false;
    private MapSceneNode selectNode;
    public override void OnAwake()
    {
        playert = BattleManager.Instance.GetPlayerNode();
        enemies = BattleManager.Instance.GetEnemyNodeList();
        pets = BattleManager.Instance.GetPetNodeList();
    }
    public override void OnStart()
    {

        InitPlayData();
        InitEnemyDataa();
        InitPetData();
        InitBtn();
        BattleManager.Instance.RoundEndCallBack += UpdataRound;
        UpdataRound();

        
    }
    public override void GoInUI()
    {
        isGoOthen = false;
    }
    public override void OutUI()
    {
        isGoOthen = true;
        isMousePressed = false;
        selectNode = null;
        for (int i = 0; i < btnImageList.Count; i++)
        {
            if (btnImageList[i].obj.activeInHierarchy == true)
            {
                btnImageList[i].obj.SetActive(false);
            }
        }
       
    }
 
    void Update()
    {
        if (!isGoOthen)
        {
            CheckMousePosition();
            if (Input.GetMouseButtonDown(0))
            {
                // 鼠标按下时执行的操作
                isMousePressed = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                // 鼠标抬起时执行的操作
                if (isMousePressed)
                {
                    if (selectNode != null)
                    {
                        Button button = selectNode.gameObject.transform.GetComponent<Button>();
                        selectNode.obj.SetActive(false);
                        button.onClick.Invoke();
                 

                    }
                }
                // 重置状态
                isMousePressed = false;
            }
        }
       

    }
    private void InitBtn()
    {
        battle_attack_btn.onClick.AddListener(OnAttackBtn);
        battle_skill_btn.onClick.AddListener(OnSkillBtn);
        battle_escape_btn.onClick.AddListener(CloseSelf);
        battle_auto_btn.onClick.AddListener(OnAutoBtn);
        battle_bag_btn.onClick.AddListener(() =>{ Common.Instance.ShowBag(); });
        battle_attack_btn.spriteState = new SpriteState { highlightedSprite = UiManager.LoadSprite("battle", "battle_attack_btn2") }; // 设置 highlightedSprite
        battle_bag_btn.spriteState = new SpriteState { highlightedSprite = UiManager.LoadSprite("battle", "battle_bag_btn2")}; // 设置 highlightedSprite
        battle_skill_btn.spriteState = new SpriteState { highlightedSprite = UiManager.LoadSprite("battle", "battle_skill_btn2") }; // 设置 highlightedSprite
        MapSceneNode obj = battle_attack_btn.transform.GetComponent<MapSceneNode>();
        btnImageList.Add(obj);
        obj = battle_bag_btn.transform.GetComponent<MapSceneNode>();
        btnImageList.Add(obj);
        obj = battle_skill_btn.transform.GetComponent<MapSceneNode>();
        btnImageList.Add(obj);
    }
     void UpdataRound()
    {
        round_num.text = BattleManager.Instance.roundCount.ToString();

    }
    void OnAutoBtn() {
        BattleManager.Instance.isAuto = true;
        BattleManager.Instance.AddAutoPlayAction();
        BattleManager.Instance.SwitchToPhase(GamePhase.Action);

    }
     void OnAttackBtn()
    {

        UIBattleAttack gui = UiManager.OpenUI<UIBattleAttack>("UIBattleAttack");
        gui.SetCallBack((float pro) =>
        {
            Enemy temEnemy = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].hp > 0)
                {
                    temEnemy = enemies[i];
                }
            }
            playert.SetActionPhaseCallback(() =>
            {
                if (temEnemy != null)
                {
                   playert.stateMachine.SetState(new AttackState(new List<BattleUnitBase>() { temEnemy },player_bones, playert, pro));
                }

            });
            BattleManager.Instance.SwitchToPhase(GamePhase.Action);

        });
        PetAttack();


    }
    void OnSkillBtn()
    {
        UILogon uILogon = UiManager.OpenUI<UILogon>("UILogon");
        uILogon.SetnewBieDotList(new List<int>() { 1, 4, 7 });
        uILogon.SetData(null, (pointArr) =>{
            ISkillEffect skill = SkillManager.Instance.GetSkillEffect();
            List<BattleUnitBase> battleUnitBase = new List<BattleUnitBase>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].hp > 0)
                {
                    battleUnitBase.Add(enemies[i]);
                }
            }
            playert.SetActionPhaseCallback(() =>
            {
                playert.stateMachine.SetState(new CastSkillState(skill, playert, battleUnitBase,player_bones));

            }) ;
            BattleManager.Instance.SwitchToPhase(GamePhase.Action);
        },3);
        PetAttack();


    }
    void InitPlayData()
    {
        m_name.text = GameManage.userData.userName;
        level.text = GameManage.userData.level.ToString();
        playert.UpdataAttributeChange = UpdataPlayHpData;
        player_bones = UiManager.LoadBonesByNmae("player_boy_bones");
        player_bones.transform.SetParent(player_bones_root);
        player_bones.transform.localScale = Vector3.one;
        player_bones.transform.localPosition = Vector3.zero;
        playert.obj_root = player_bones_base;
        playert.BindBones(player_bones);
        UpdataPlayHpData();
        if (GameManage.userData.userGender == Gender.Girl)
        {
            head.sprite = UiManager.LoadSprite("battle", "head_girl");  
        }

    }
    void UpdataPlayHpData()
    {
        PlayertCharacterNode playert = BattleManager.Instance.GetPlayerNode();

        player_hp.fillAmount = (float)playert.hp  / (float)playert.m_cfg.hp;
        for (int i = 0; i < energy_grid_list.Count; i++)
        {
            energy_grid_list[i].SetActive(i + 1 <= playert.mp);


        }

    }
    void InitEnemyDataa()
    {
        
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];
            if (enemyObjList.Count <= i)
            {
                if (i == 0)
                {
                    enemyObjList.Add(enemy_item);
                    enemy.BindObj(enemy_item);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(enemy_item);
                    go.transform.SetParent(enemy_root);
                    go.transform.localScale = Vector3.one;
                    enemy.BindObj(go);
                    enemyObjList.Add(go);
                }
            }
            enemy.UpdataAttributeChange += UpdateEnemyItem;
            enemy.obj_root = enemy_bones_list[i];
            UpdateEnemyItem(enemy, enemyObjList[i]);
            DragonBonesController dragon = UpdateEnemyBones(enemy, enemy_bones_list[i]);
            enemy.SetActionPhaseCallback(() =>
            {
                enemy.stateMachine.SetState(new AttackState(new List<BattleUnitBase>() { playert }, dragon, enemy));
            });  
        }
    }
    void PetAttack()
    {

        for (int i = 0; i < pets.Count; i++)
        {
            Pet pet = pets[i];
            Enemy temEnemy = null;
            for (int z = 0; z < enemies.Count; z++)
            {
                if (enemies[z].hp > 0)
                {
                    temEnemy = enemies[z];
                }
            }
            pet.SetActionPhaseCallback(() =>
            {

                pet.stateMachine.SetState(new AttackState(new List<BattleUnitBase>() { temEnemy }, pet.bones, pet));
            });
        }


    }
    void InitPetData()
    {

        for (int i = 0; i < pets.Count; i++)
        {
            Pet pet = pets[i];
            DragonBonesController dragon = UpdatePetBones(pet, pet_bones_list[i]);
            
        }


    }
    private void UpdateEnemyItem(Enemy enemie,GameObject go)
    {
        RectTransform rect = go.GetComponent<RectTransform>();
        Image enemy_hp = rect.Find("enemy_hp").GetComponent<Image>();
        TextMeshProUGUI enemy_name = rect.Find("enemy_name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI enemy_level = rect.Find("enemy_level").GetComponent<TextMeshProUGUI>();
        Image head_enemy = rect.Find("head_enemy").GetComponent<Image>();
        enemy_hp.fillAmount = (float)enemie.hp / (float)enemie.m_cfg.hp;
        enemy_name.text = enemie.m_cfg.name;
        enemy_level.text = enemie.m_cfg.level.ToString();
        head_enemy.sprite = UiManager.LoadSprite("head_icon", enemie.m_cfg.head);

    }
    private DragonBonesController UpdateEnemyBones(Enemy enemie, RectTransform rect)
    {
        RectTransform bones_root = rect.Find("bones_root").GetComponent<RectTransform>();
        DragonBonesController dragon = UiManager.LoadBonesByNmae(enemie.m_cfg.bones);
        enemie.BindBones(dragon);
        dragon.transform.SetParent(bones_root);
        dragon.transform.localScale = Vector3.one;
        dragon.transform.localPosition = Vector3.zero;
        return dragon;
    }
    private DragonBonesController UpdatePetBones(Pet pet, RectTransform rect)
    {
        RectTransform bones_root = rect.Find("bones_root").GetComponent<RectTransform>();
        DragonBonesController dragon = UiManager.LoadBonesByNmae(pet.m_cfg.bones);
        pet.BindBones(dragon);
        dragon.transform.SetParent(bones_root);
        dragon.transform.localScale = Vector3.one;
        dragon.transform.localPosition = Vector3.zero;
        return dragon;
    }
    public DragonBonesController LoadBeatenEffect(string beaten_effect)
    {
        if (beaten_effect_map.ContainsKey(beaten_effect))
        {
            return beaten_effect_map[beaten_effect];
        }
        DragonBonesController wordBones = UiManager.LoadBonesByNmae(beaten_effect);
        wordBones.transform.SetParent(beaten_effect_root);
        wordBones.transform.localScale = Vector3.one;
        wordBones.transform.localPosition = Vector3.zero;
        beaten_effect_map.Add(beaten_effect, wordBones);
        return wordBones;
    }
    void CheckMousePosition()
    {
        if (UiManager.uiCamera == null)
        {
            Debug.LogError("UI Camera is not assigned!");
            return;
        }

        // 检查是否点击在UI元素上
        if (EventSystem.current.IsPointerOverGameObject() || Application.platform != RuntimePlatform.WindowsPlayer)
        {
            // 获取鼠标在屏幕上的位置
            Vector3 mousePosition = Input.mousePosition;

            // 创建一条射线
            Ray ray = UiManager.uiCamera.ScreenPointToRay(mousePosition);



            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < btnImageList.Count; i++)
            {
                if (btnImageList[i].obj.activeInHierarchy == true)
                {
                    btnImageList[i].obj.SetActive(false);
                }
            }
            selectNode = null;
            foreach (RaycastHit hit in hits)
            {
                // 检查碰撞到的物体是否有Renderer组件
                Image renderer = hit.collider.GetComponent<Image>();
                MapSceneNode mapScene = hit.collider.gameObject.GetComponent<MapSceneNode>();
                if (renderer != null)
                {

                    // 获取鼠标指针位置的纹理坐标

                    // 获取纹理
                    Texture2D texture = renderer.sprite.texture;

                    if (texture != null)
                    {
                        Vector2 screenPos = Input.mousePosition;

                        // 将屏幕坐标转换为 Canvas 中的本地坐标
                        Vector2 localPos;

                        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapScene.rect, screenPos, UiManager.uiCamera, out localPos);
                        RectTransform imageRectTransform = renderer.rectTransform;
                        localPos.x = Mathf.Clamp(localPos.x, -imageRectTransform.rect.width / 2, imageRectTransform.rect.width / 2);
                        localPos.y = Mathf.Clamp(localPos.y, -imageRectTransform.rect.height / 2, imageRectTransform.rect.height / 2);
                        Vector2 pixelPos = new Vector2(
                                (localPos.x + imageRectTransform.rect.width / 2) / imageRectTransform.rect.width,
                                (localPos.y + imageRectTransform.rect.height / 2) / imageRectTransform.rect.height
                        );

                        Debug.Log(pixelPos);
                        // 转换为纹理坐标
                        int texX = Mathf.FloorToInt(pixelPos.x * texture.width);
                        int texY = Mathf.FloorToInt(pixelPos.y * texture.height);

                        // 获取像素颜色
                        Color pixelColor = texture.GetPixel(texX, texY);
                        Color[] pixelsColor = texture.GetPixels();
                        //for (int i = 0; i < pixelsColor.Length; i++)
                        //{
                        //    Debug.Log(texture.name + pixelsColor[i]);

                        //}
                        Debug.Log(renderer.gameObject.name + pixelColor + pixelsColor.Length);

                        // 检查透明度
                        if (pixelColor.a > 0)
                        {
                            selectNode = mapScene;
                            mapScene.obj.SetActive(true);
                            break;
                        }
                    }

                }
            }
  
        }
    }
    public override void OnDestroyImp()
    {
        BattleManager.Instance.ExitBattle();
    }
}
