using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetUp : UIBase
{
    public Button quit_btn;
    public Button notice_btn;
    public Button service_btn;
    public TextMeshProUGUI user_name;
    public Slider slider1;
    public Slider slider2;
    public Button privacy_btn;
    public Button protocol_btn;
    public Button close_btn;

    public Image head;
    public override void OnStart()
    {
        slider1.value = AudioManager.Instance.GetSoundVolume();
        slider2.value = AudioManager.Instance.GetBGMVolume();
        slider1.onValueChanged.AddListener(SetVolume1);
        slider2.onValueChanged.AddListener(SetVolume2);
        close_btn.onClick.AddListener(CloseSelf);
        if (GameManage.userData.userGender == Gender.Boy)
        {
            head.sprite = UiManager.LoadSprite("setup", "setup5");
        }
        else
        {
            head.sprite = UiManager.LoadSprite("setup", "setup6");
        }
        quit_btn.onClick.AddListener(()=>
        {
            Application.Quit();
        });
        user_name.text = GameManage.userData.userName;
        privacy_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("       隐私政策\r\n1. 引言 我们尊重并重视您的隐私。本隐私政策详细说明了我们如何收集、使用、保护和分享您的个人信息。我们致力于保护您的隐私，并遵守所有适用的隐私法律和规定。\r\n2. 信息收集 我们可能通过以下方式收集您的个人信息：\r\n● 当您注册账户或使用我们的服务时，我们可能会收集您的姓名、电子邮件地址、电话号码等联系信息。\r\n● 当您使用我们的服务时，我们可能会收集您的产品体验数据，包括但不限于进度、得分、偏好设置等。\r\n3. 信息使用 我们使用您的个人信息来：\r\n● 提供和改进我们的产品和服务。\r\n● 与您沟通，包括发送通知、促销信息和更新。\r\n● 进行数据分析，以更好地了解用户需求和优化服务。\r\n4. 信息分享 我们不会与第三方分享您的个人信息，除非：\r\n● 您明确同意我们这样做。\r\n● 我们与第三方服务提供商合作，以提供服务或执行交易，且这些服务提供商同意遵守本隐私政策。\r\n● 我们认为有必要遵守法律或保护我们或他人的权益。\r\n5. 信息安全 我们采取适当的技术和管理措施来保护您的个人信息免受未经授权的访问、披露、更改或破坏。\r\n6. 用户权利 您有权访问、更正、删除或限制我们处理您的个人信息。如需行使这些权利，请通过我们的联系方式与我们联系。\r\n7. 未成年人保护 我们非常重视未成年人的隐私保护。我们不会故意收集儿童的个人信息。\r\n8. 政策更新 我们保留随时更新本隐私政策的权利。任何重大变更将通过我们的网站或应用通知您。\r\n\r\n");
        });
        protocol_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("       用户协议\r\n1. 引言 欢迎来到 《神符师之路》！本用户协议（以下简称“协议”）规定了您使用 《神符师之路》 服务的条款和条件。通过注册账户或使用我们的服务，您同意遵守本协议的所有条款。\r\n2. 账户注册与使用\r\n● 您应提供真实、准确、最新的个人信息，并负责更新这些信息。\r\n● 您应负责保护您的账户安全，不得将账户信息透露给他人。\r\n3. 服务条款\r\n● 我们保留随时修改或终止服务的权利，无需事先通知。\r\n● 我们可能不时更新产品内容或功能，您应下载最新版本以享受最佳体验。\r\n4. 知识产权\r\n● 产品及其所有内容，包括但不限于文本、图形、界面设计、代码和数据，均受版权、商标和其他知识产权法律的保护。\r\n● 未经我们明确书面同意，您不得复制、修改、分发、出售或公开展示产品或其任何部分。\r\n5. 用户行为规范\r\n● 您应遵守所有适用的法律和本协议，不得从事任何非法活动。\r\n● 您不得干扰或破坏游戏的正常运行，包括但不限于使用作弊软件、自动化脚本或任何未经授权的第三方工具。\r\n6. 责任限制\r\n● 我们不对因使用服务而可能产生的任何间接、特殊、偶然或后果性损害承担责任。\r\n● 我们不对服务的中断、延迟、错误或不准确信息承担责任。\r\n7. 免责声明\r\n● 产品及其服务按“现状”提供，我们不提供任何形式的保证或条件。\r\n● 我们不对第三方服务或内容的准确性、完整性或合法性负责。\r\n8. 争议解决\r\n● 本协议的解释、适用和争议解决均适用中国法律。\r\n● 任何因本协议引起的或与本协议有关的争议应首先通过友好协商解决。\r\n9. 协议修改\r\n● 我们保留随时修改本协议的权利。任何重大变更将通过我们的网站或应用通知您，并在变更生效前至少30天公布。\r\n10. 完整协议\r\n● 本协议，连同我们的隐私政策和任何其他相关政策，构成了您与我们之间的完整协议。\r\n");
        });
        notice_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("          公告\r\n亲爱的玩家们，\r\n我们激动地宣布，经过无数个日夜的努力和精心打磨，<<神符师之路>>终于在今天正式与大家见面了！\r\n<<神符师之路>>是一款结合了浓浓的心理学元素、面向青少年抑郁患者的辅助训练产品。在这里，将和您创建的角色经历一场前所未有的英雄之旅，角色在师傅的成长性陪伴下，外出冒险、遇见挫折、回到师父这里灌注能量、再次不断冒险，在这个过程中逐渐克服错误经验组织原则，形成内聚性核心自体，进而达到疗愈自身的效果。\r\n我们诚挚地邀请每一位用户一起开启这段奇妙的旅程。您的支持是我们前进的动力，我们期待您的宝贵意见和建议，让我们共同打造一个更有促进性的环境。\r\n神符师之路研发团队\r\n2024.06.25");

        });
        service_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("客服\r\n737197@qq.com");
        });

    }
    private void SetVolume1(float volume)
    {
        AudioManager.Instance.SetSoundVolume(volume);
    }
    private void SetVolume2(float volume)
    {
        AudioManager.Instance.SetBGMVolume(volume);
    }
}
