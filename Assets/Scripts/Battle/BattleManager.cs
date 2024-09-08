// 简单的战斗管理器
using System;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    Preparation,//准备
    Action, //行动
    // 可以根据需要添加其他阶段
}
public enum UnitType
{
    Enemy,
    Player,
    Default,
}
public class BattleManager
{
    public PlayertCharacterNode player;
    public List<Enemy> enemyList = new List<Enemy>();
    private static BattleManager m_Instance;
    public int attackCoeff = 40;
    private GamePhase currentPhase;
    private int currentTurnIndex = -1;
    public int roundCount = 1;
    private List<BattleUnitBase> sequenceAction;
    public Action RoundEndCallBack; // 状态改变时的事件
    private DelayedCoroutineData delayed;
    public UIBattle uIBattle;
    public int addExp = 0;
    public List<Pet> petList = new List<Pet>();
    public bool isAuto = false;

    public static BattleManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new BattleManager();
                return m_Instance;
            }
            else
            {
                return m_Instance;
            }
            
        }

    }
    public BattleManager()
    {

      //  StartBattle();
    }
    public void StartBattle(int barrierId)
    {
        Debug.Log("Battle Start!");
        sequenceAction = new List<BattleUnitBase>();
        player = new PlayertCharacterNode();
        barrieconfigData mCfg = GetCfgManage.Instance.GetCfgByNameAndId<barrieconfigData>("barrier", barrierId);
        string[] strs = mCfg.enemy_list.Split(",");
        for (int i = 0; i < strs.Length; i++)
        {
            enemyList.Add(new Enemy(int.Parse(strs[i])));
        }
        //string[] strs1 = mCfg.pet_list.Split(",");
        //for (int i = 0; i < strs1.Length; i++)
        //{
        //    petList.Add(new Pet(int.Parse(strs1[i])));
        //}
     
        addExp = 0;
        // 初始化战斗，将回合数置为1
        roundCount = 1;
        currentTurnIndex = 0;
        sequenceAction.Add(player);
        for (int i = 0; i < petList.Count; i++)
        {
            sequenceAction.Add(petList[i]);
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            sequenceAction.Add(enemyList[i]);
        }
        uIBattle = UiManager.OpenUI<UIBattle>("UIBattle");
        SwitchToPhase(GamePhase.Preparation);
  
    }
    private void StartTurn()
    {
        SwitchUnit();
    }
    public void AddAutoPlayAction()
    {
        List<Enemy>  enemies = GetEnemyNodeList();
        PlayertCharacterNode playert = GetPlayerNode();
        List<Pet> pets = GetPetNodeList();
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
            if (playert.mp == 2)
            {
                ISkillEffect skill = SkillManager.Instance.GetSkillEffect();
                List<BattleUnitBase> battleUnitBase = new List<BattleUnitBase>();
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].hp > 0)
                    {
                        battleUnitBase.Add(enemies[i]);
                    }
                   
                }
               
                playert.stateMachine.SetState(new CastSkillState(skill, playert, battleUnitBase, player.bones));

            }
            else
            {
                if (temEnemy != null)
                {
                    System.Random random = new System.Random();

                    // 生成随机整数，范围是 [0, 10)
                    int randomNumber = random.Next(0, 11);
                    float pro = (float)randomNumber / 10;
                    Console.WriteLine("随机数：" + randomNumber);
                    playert.stateMachine.SetState(new AttackState(new List<BattleUnitBase>() { temEnemy }, player.bones, playert, pro));
                }
            }
           

        });
        for (int i = 0; i < pets.Count; i++)
        {
            Pet pet = pets[i];
            pet.SetActionPhaseCallback(() =>
            {
                temEnemy = null;
                for (int z = 0; z < enemies.Count; z++)
                {
                    if (enemies[z].hp > 0)
                    {
                        temEnemy = enemies[z];
                    }
                }
                if (temEnemy != null)
                {
                    pet.stateMachine.SetState(new AttackState(new List<BattleUnitBase>() { temEnemy }, pet.bones, pet));
                }
            });
        }



    }
    public void SwitchUnit()
    {

        UnitType winUnit = UnitType.Default;
        if (IsBattleEnd(ref winUnit))
        {
            UIBattleSettle gui = UiManager.OpenUI<UIBattleSettle>("UIBattleSettle");
            if (winUnit == UnitType.Enemy)
            {
                gui.SetData(false, () =>
                {

                    uIBattle.CloseSelf();
                });
                Debug.Log("敌人胜利，战斗结束");
            }
            else if (winUnit == UnitType.Player)
            {
                gui.SetData(true, () =>
                {

                    uIBattle.CloseSelf();
                });
                Debug.Log("玩家胜利，战斗结束");
            }
            return;
        }

        if (currentTurnIndex == sequenceAction.Count-1  )
        {
            if (isAuto == false)
            {
                roundCount++;
                RoundEndCallBack?.Invoke();
                SwitchToPhase(GamePhase.Preparation);
             
            }
            else
            {
                AddAutoPlayAction();
                roundCount++;
                currentTurnIndex = -1;
                RoundEndCallBack?.Invoke();
                SwitchToPhase(GamePhase.Action);
            }
            return;
        }
        delayed = DelayedActionProvider.Instance.DelayedAction(() =>
        {
            currentTurnIndex++;
            sequenceAction[currentTurnIndex].OnActionPhaseComplete();
            delayed = null;
        }, 0.5f);
      
        
       
    }
    public void EndTurn()
    {
        // 结束当前回合，切换到下一个角色
        
        StartTurn();
    }
    public PlayertCharacterNode GetPlayerNode()
    {

        if (player!=null)
        {
            return player;
        }
        return player = new PlayertCharacterNode();

    }
    public List<Enemy> GetEnemyNodeList()
    {

        return enemyList;

    }
    public List<Pet> GetPetNodeList()
    {

        return petList;

    }

    private bool IsBattleEnd(ref UnitType winUnit)
    {
        if (player.hp <= 0)
        {
            winUnit = UnitType.Enemy;
            return true;
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].hp > 0)
            {
                return false;

            }
        }
        winUnit = UnitType.Player;
        return true;
    }
    public void SwitchToPhase(GamePhase phase )
    {
 
        currentPhase = phase;
        // 切换到下一个阶段
        if (phase == GamePhase.Action)
        {
            currentTurnIndex = -1;
            uIBattle.make.SetActive(true);
            uIBattle.operate_root.SetActive(false);
            StartTurn();
        }
        else
        {
            uIBattle.make.SetActive(false);
            uIBattle.operate_root.SetActive(true);
        }

        // 开始新的阶段
   
    }
    public void ExitBattle()
    {
        enemyList.Clear();
        petList.Clear();
        sequenceAction = null;
        player = null;

    }
}