using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessingFistsTimer
{
    public bool isWin;
    public int cardId;
    public string m_name;
    public string diPaiName;
    public int needCard = -1;

    // 创建 Random 对象
    System.Random random = new System.Random();

    public GuessingFistsTimer()
    {
        // 生成随机数
        cardId = random.Next(1, 4); //1 布  2剪刀 3锤子
        isWin = random.Next(2) == 0;

        if (cardId == 1)
        {
            diPaiName = "02_DiPai_Bu";
        }
        else if (cardId == 2)
        {
            diPaiName = "01_DiPai_Jian";
        }
        else if (cardId == 3)
        {
            diPaiName = "03_DiPai_Quan";
        }


    }
    public bool CheckIsTrue(int cardId)
    {
        Debug.Log(this.cardId + "::" + cardId);
        if ( !isWin)
        {
            if (this.cardId == 1)
            {
                return cardId == 3;
            }
            else if (this.cardId == 2)
            {
                return cardId == 1;
            }
            else if (this.cardId == 3)
            {
                return cardId == 2;
            }
            
        }
        else
        {
            if (this.cardId == 1)
            {
                return cardId == 2;
            }
            else if (this.cardId == 2)
            {
                return cardId == 3;
            }
            else if (this.cardId == 3)
            {
                return cardId == 1;
            }
        }
        return false;
    }
}

public class UIGuessingFists : UIBase
{
    GuessingFistsTimer currOneTimer;
    public DragonBonesController guessing_fists_kapai;
    public DragonBonesController guessing_fists_dipai;
    public List<Button> buttons;
    public Image condition;
    public Image result;
    private Vector3 kapaiPos;
    private Vector3 dipaiPos;
    int right_value = 0;
    int error_value = 0;
    public Image guessing_fists1;
    public Image guessing_fists2;
    public Image pro;
    int maxPace = 5;
    int curr_value = 0;
    int currDiff = 0;
    bool isClick = true;
    public TextMeshProUGUI right_num;
    public TextMeshProUGUI error_num;
    public TextMeshProUGUI schedule;
    private float proTime = 5;
    Tween tween;
    public Button close_btn;
    public Button rule_btn;
    public GameObject lockui;
    // Start is called before the first frame update
    public override void OnStart()
    {
        kapaiPos = guessing_fists_kapai.transform.localPosition;
        dipaiPos = guessing_fists_dipai.transform.localPosition;
        InitBtn();
        isClick = false;
        MissionManage.ShowDescription(() =>
        {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
              UpdataDiff();
            });
        });

        rule_btn.onClick.AddListener(() =>
        {
            if (tween != null)
            {
                tween.Pause();

            }
            guessing_fists_dipai.PauseAnimation();
            MissionManage.ShowDescription(() =>
            {
                if (tween != null)
                {
                    tween.Play();

                }
                guessing_fists_dipai.ResumeAnimation();

            });

        });
        close_btn.onClick.AddListener(CloseSelf);
    }
    private void UpdataDiff(bool isAdd = true)
    {
        if (isAdd)
        {
            currDiff++;
        }
        if (currDiff == 1)
        {
            maxPace = 5;
            proTime = 5;
        }
        else if (currDiff == 2)
        {
            //if (isAdd)
            //{
            //    Common.Instance.ShowBones("nandutisheng_bones", () =>
            //    {
            //    });
            //}
      
            maxPace = 7;
            proTime = 3;
        }
        else if (currDiff == 3)
        {
            //if (isAdd)
            //{
            //    Common.Instance.ShowBones("nandutisheng_bones", () =>
            //    {
            //    });
            //}
            maxPace = 9;
            proTime = 1.5f;
        }
        else
        {
            
            return;
        }
        curr_value = 0;
        right_value = 0;
        error_value = 0;
        UpdataTopic();
        UpdataText();
    }
    private void UpdataTopic()
    {
        if (maxPace == curr_value)
        {
            if (CalculateWinningRounds(maxPace) <= right_value)
            {
                if (currDiff == 3)
                {
                    Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(currDiff), () =>
                    {
                        currDiff = 3;
                        UpdataDiff(false);

                    }, () => { CloseSelf(); }, () =>
                    {
                        UpdataDiff();

                    });
                }
                else
                {
                    Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(currDiff), () =>
                    {
                        UpdataDiff(false);

                    }, () => { CloseSelf(); }, () =>
                    {
                        UpdataDiff();

                    });
                }
                
                
            }
            else
            {

                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(currDiff), () =>
                {
                    UpdataDiff(false);

                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });

            }
            return;
        }
        curr_value++;
        schedule.text = curr_value + "/" + maxPace;
        currOneTimer = new GuessingFistsTimer();

        if (currOneTimer.isWin)
        {
            guessing_fists1.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists1");
            guessing_fists2.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists3");

            condition.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists_condition1");
        }
        else
        {
            guessing_fists1.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists2");
            guessing_fists2.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists1");
            condition.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists_condition2");

        }
        pro.fillAmount = 1;
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        guessing_fists_dipai.gameObject.SetActive(true);
        lockui.SetActive(true);
        guessing_fists_dipai.PlayAnimation(currOneTimer.diPaiName, false, () => {
            lockui.SetActive(false);
            tween = pro.DOFillAmount(0, proTime);
            tween.onComplete = () =>
            {
                tween = null;
                error_value++;
                UpdataTopic();
                UpdataText();

            };
            isClick = true;
        });
       
        
       
    }
    public int CalculateWinningRounds(int totalRounds)
    {
        // 奇数总局数，需要赢得总局数的一半加一
        // 偶数总局数，需要赢得总局数的一半加一
        return (totalRounds / 2) + 1;
    }
    private void UpdataText()
    {
        right_num.text = right_value.ToString();
        error_num.text = error_value.ToString();


    }
    private void InitBtn()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i + 1;
            buttons[i].onClick.AddListener(() =>
            {
                if (isClick == false)
                {
                    return;
                }
                if (tween != null)
                {
                    tween.Kill();
                }
                isClick = false;
                string animName = "02_Bu";
                if (index == 1)
                {
                    animName = "02_Bu";
                }
                else if (index == 2)
                {
                    animName = "01_Jian";
                }
                else if (index == 3)
                {
                    animName = "03_Quan";
                }
                guessing_fists_kapai.gameObject.SetActive(true);
                guessing_fists_kapai.PlayAnimation(animName, false, () =>
                {
                    result.gameObject.SetActive(true);
                   
                    if (currOneTimer.CheckIsTrue(index))
                    {
                        right_value++;
                        result.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists_right");
                    }
                    else
                    {
                        error_value++;
                        result.sprite = UiManager.LoadSprite("guessing_fists", "guessing_fists_error");
                    }
                    UpdataText();
                    DelayedActionProvider.Instance.DelayedAction(() =>
                    {
                        result.gameObject.SetActive(false);
                        Tween tween = guessing_fists_kapai.transform.DOLocalMoveY(kapaiPos.x - 800, 0.5f);
                        tween.onComplete = () =>
                        {
                            guessing_fists_kapai.gameObject.SetActive(false);
                            guessing_fists_kapai.transform.localPosition = kapaiPos;
                            UpdataTopic();

                        };
                    },1);

                });



            });
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
