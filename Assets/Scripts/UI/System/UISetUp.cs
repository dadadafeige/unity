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
            uISet.SetLabel("       ��˽����\r\n1. ���� �������ز�����������˽������˽������ϸ˵������������ռ���ʹ�á������ͷ������ĸ�����Ϣ�����������ڱ���������˽���������������õ���˽���ɺ͹涨��\r\n2. ��Ϣ�ռ� ���ǿ���ͨ�����·�ʽ�ռ����ĸ�����Ϣ��\r\n�� ����ע���˻���ʹ�����ǵķ���ʱ�����ǿ��ܻ��ռ����������������ʼ���ַ���绰�������ϵ��Ϣ��\r\n�� ����ʹ�����ǵķ���ʱ�����ǿ��ܻ��ռ����Ĳ�Ʒ�������ݣ������������ڽ��ȡ��÷֡�ƫ�����õȡ�\r\n3. ��Ϣʹ�� ����ʹ�����ĸ�����Ϣ����\r\n�� �ṩ�͸Ľ����ǵĲ�Ʒ�ͷ���\r\n�� ������ͨ����������֪ͨ��������Ϣ�͸��¡�\r\n�� �������ݷ������Ը��õ��˽��û�������Ż�����\r\n4. ��Ϣ���� ���ǲ�����������������ĸ�����Ϣ�����ǣ�\r\n�� ����ȷͬ��������������\r\n�� ����������������ṩ�̺��������ṩ�����ִ�н��ף�����Щ�����ṩ��ͬ�����ر���˽���ߡ�\r\n�� ������Ϊ�б�Ҫ���ط��ɻ򱣻����ǻ����˵�Ȩ�档\r\n5. ��Ϣ��ȫ ���ǲ�ȡ�ʵ��ļ����͹����ʩ���������ĸ�����Ϣ����δ����Ȩ�ķ��ʡ���¶�����Ļ��ƻ���\r\n6. �û�Ȩ�� ����Ȩ���ʡ�������ɾ�����������Ǵ������ĸ�����Ϣ��������ʹ��ЩȨ������ͨ�����ǵ���ϵ��ʽ��������ϵ��\r\n7. δ�����˱��� ���Ƿǳ�����δ�����˵���˽���������ǲ�������ռ���ͯ�ĸ�����Ϣ��\r\n8. ���߸��� ���Ǳ�����ʱ���±���˽���ߵ�Ȩ�����κ��ش�����ͨ�����ǵ���վ��Ӧ��֪ͨ����\r\n\r\n");
        });
        protocol_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("       �û�Э��\r\n1. ���� ��ӭ���� �����ʦ֮·�������û�Э�飨���¼�ơ�Э�顱���涨����ʹ�� �����ʦ֮·�� ����������������ͨ��ע���˻���ʹ�����ǵķ�����ͬ�����ر�Э����������\r\n2. �˻�ע����ʹ��\r\n�� ��Ӧ�ṩ��ʵ��׼ȷ�����µĸ�����Ϣ�������������Щ��Ϣ��\r\n�� ��Ӧ���𱣻������˻���ȫ�����ý��˻���Ϣ͸¶�����ˡ�\r\n3. ��������\r\n�� ���Ǳ�����ʱ�޸Ļ���ֹ�����Ȩ������������֪ͨ��\r\n�� ���ǿ��ܲ�ʱ���²�Ʒ���ݻ��ܣ���Ӧ�������°汾������������顣\r\n4. ֪ʶ��Ȩ\r\n�� ��Ʒ�����������ݣ��������������ı���ͼ�Ρ�������ơ���������ݣ����ܰ�Ȩ���̱������֪ʶ��Ȩ���ɵı�����\r\n�� δ��������ȷ����ͬ�⣬�����ø��ơ��޸ġ��ַ������ۻ򹫿�չʾ��Ʒ�����κβ��֡�\r\n5. �û���Ϊ�淶\r\n�� ��Ӧ�����������õķ��ɺͱ�Э�飬���ô����κηǷ����\r\n�� �����ø��Ż��ƻ���Ϸ���������У�������������ʹ������������Զ����ű����κ�δ����Ȩ�ĵ��������ߡ�\r\n6. ��������\r\n�� ���ǲ�����ʹ�÷�������ܲ������κμ�ӡ����⡢żȻ�������𺦳е����Ρ�\r\n�� ���ǲ��Է�����жϡ��ӳ١������׼ȷ��Ϣ�е����Ρ�\r\n7. ��������\r\n�� ��Ʒ������񰴡���״���ṩ�����ǲ��ṩ�κ���ʽ�ı�֤��������\r\n�� ���ǲ��Ե�������������ݵ�׼ȷ�ԡ������Ի�Ϸ��Ը���\r\n8. ������\r\n�� ��Э��Ľ��͡����ú��������������й����ɡ�\r\n�� �κ���Э������Ļ��뱾Э���йص�����Ӧ����ͨ���Ѻ�Э�̽����\r\n9. Э���޸�\r\n�� ���Ǳ�����ʱ�޸ı�Э���Ȩ�����κ��ش�����ͨ�����ǵ���վ��Ӧ��֪ͨ�������ڱ����Чǰ����30�칫����\r\n10. ����Э��\r\n�� ��Э�飬��ͬ���ǵ���˽���ߺ��κ�����������ߣ���������������֮�������Э�顣\r\n");
        });
        notice_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("          ����\r\n�װ�������ǣ�\r\n���Ǽ�����������������������ҹ��Ŭ���;��Ĵ�ĥ��<<���ʦ֮·>>�����ڽ�����ʽ���Ҽ����ˣ�\r\n<<���ʦ֮·>>��һ������ŨŨ������ѧԪ�ء������������������ߵĸ���ѵ����Ʒ������������������Ľ�ɫ����һ��ǰ��δ�е�Ӣ��֮�ã���ɫ��ʦ���ĳɳ�������£����ð�ա��������ۡ��ص�ʦ�������ע�������ٴβ���ð�գ�������������𽥿˷���������֯ԭ���γ��ھ��Ժ������壬�����ﵽ���������Ч����\r\n���ǳ�ֿ������ÿһλ�û�һ�������������ó̡�����֧��������ǰ���Ķ����������ڴ����ı�������ͽ��飬�����ǹ�ͬ����һ�����дٽ��ԵĻ�����\r\n���ʦ֮·�з��Ŷ�\r\n2024.06.25");

        });
        service_btn.onClick.AddListener(() =>
        {
            UISetUpNotice uISet = UiManager.OpenUI<UISetUpNotice>("UISetUpNotice");
            uISet.SetLabel("�ͷ�\r\n737197@qq.com");
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
