using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class AudioManager
{
    //����������Դ���
    private AudioSource _bgmAudioSource;
    private AudioSource _soundAudioSource;
    //���屳�����ֺ���Ч���ֵ�
    private Dictionary<BGMType, AudioClip> _bgmDict;
    private Dictionary<SoundType, AudioClip> _soundDict;
    private static AudioManager instance;
    public string currBGMName;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {

                instance = new AudioManager();
            }
            return instance;
        }
    }

    public AudioManager()
    {
        OnInit();

    }
    //��ʼ������
    public void OnInit()
    {
        //������Ƶ���������������֡���Ч������Ϸ���󣬲��������ǵĸ��ڵ�
        GameObject audioManagerGo = new GameObject("AudioManager");
        GameObject bgm = new GameObject("BGM");
        GameObject sound = new GameObject("Sound");
        bgm.transform.SetParent(audioManagerGo.transform);
        sound.transform.SetParent(audioManagerGo.transform);
        //���������ֺ���Ч��Ϸ���������Դ���
        _bgmAudioSource = bgm.AddComponent<AudioSource>();
        _soundAudioSource = sound.AddComponent<AudioSource>();
        //������Դ����ĳ�ʼֵ����������ѭ�����ţ���Ч��ѭ����������������
        _bgmAudioSource.loop = true;
        _bgmAudioSource.playOnAwake = true;
        _soundAudioSource.loop = false;
        _soundAudioSource.playOnAwake = false;
        //���ñ������ֵ�Ĭ������
        _bgmAudioSource.volume = 0.1f;
        //����������Դ
      //  LoadAudioResources();
    }

    //����������Դ
    //private void LoadAudioResources()
    //{
    //    //����������Դ�ļ���
    //    AudioClip aboutUsClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm1Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm2Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm3Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm4Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm5Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip mainPanelClip = Resources.Load<AudioClip>("ma1");
    //    //������������Դ��ӵ��ֵ���
    //    _bgmDict = new Dictionary<BGMType, AudioClip>();
    //    _bgmDict.Add(BGMType.AboutUS, aboutUsClip);
    //    _bgmDict.Add(BGMType.BGM_1, gameBgm1Clip);
    //    _bgmDict.Add(BGMType.BGM_2, gameBgm2Clip);
    //    _bgmDict.Add(BGMType.BGM_3, gameBgm3Clip);
    //    _bgmDict.Add(BGMType.BGM_4, gameBgm4Clip);
    //    _bgmDict.Add(BGMType.BGM_5, gameBgm5Clip);
    //    _bgmDict.Add(BGMType.Main_Panel, mainPanelClip);

    //    //��Ч������Դ�ļ���
    //    AudioClip awardClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip brickClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip brick1Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip brickDestroyClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip buttonClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip failedClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip fallDownClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip jumpClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip punishmentClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip winClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip winPerfectClip = Resources.Load<AudioClip>("ma1");
    //    //����Ч��Դ��ӵ��ֵ���
    //    _soundDict = new Dictionary<SoundType, AudioClip>();
    //    _soundDict.Add(SoundType.Award, awardClip);
    //    _soundDict.Add(SoundType.Brick, brickClip);
    //    _soundDict.Add(SoundType.Brick_1, brick1Clip);
    //    _soundDict.Add(SoundType.Brick_Destroy, brickDestroyClip);
    //    _soundDict.Add(SoundType.Button, buttonClip);
    //    _soundDict.Add(SoundType.Failed, failedClip);
    //    _soundDict.Add(SoundType.Fall_Down, fallDownClip);
    //    _soundDict.Add(SoundType.Jump, jumpClip);
    //    _soundDict.Add(SoundType.Punishment, punishmentClip);
    //    _soundDict.Add(SoundType.Win, winClip);
    //    _soundDict.Add(SoundType.WinPerfect, winPerfectClip);
    //}

    //���ű�������
    public void PlayBGM(string BGMName)
    {

        //ֹͣ��ǰ�������ֵĲ���
        _bgmAudioSource.Stop();
        AssetBundleManager.Instance.Initialize();
        // ���� AssetBundle �е���Դ
        string assetBundleName = "bgm/" + BGMName;
        string assetName = BGMName;
        AudioClip myBGM = AssetBundleManager.Instance.LoadAsset<AudioClip>(assetBundleName, assetName);
        //�����µı������֣�������
        _bgmAudioSource.clip = myBGM;
        _bgmAudioSource.Play();
    }

    //������Ч
    public void PlaySound(string soundName)
    {
        //ֹͣ��ǰ��Ч�Ĳ���
        _soundAudioSource.Stop();
        AssetBundleManager.Instance.Initialize();
        // ���� AssetBundle �е���Դ
        string assetBundleName = "sound/" + soundName;
        string assetName = soundName;
        AudioClip mySound = AssetBundleManager.Instance.LoadAsset<AudioClip>(assetBundleName, assetName);
        //�����µ���Ч��������
        _soundAudioSource.clip = mySound;
        _soundAudioSource.Play();
    }
    public void SetSoundVolume(float volume)
    {
        if (_soundAudioSource != null)
        {
            _soundAudioSource.volume = volume;
        }
    }
    public float GetSoundVolume()
    {
        if (_soundAudioSource != null)
        {
           return _soundAudioSource.volume ;
        }
        return 0;
    }
    public void SetBGMVolume(float volume)
    {
        if (_bgmAudioSource != null)
        {
            _bgmAudioSource.volume = volume;
        }
    }
    public float GetBGMVolume()
    {
        if (_bgmAudioSource != null)
        {
            return _bgmAudioSource.volume;
        }
        return 0;
    }
    //�ر�BGM����
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }
    public void StopSound()
    {
        _soundAudioSource.Stop();
    }

    //��������
    public void ControlVolume(float bgmVolume, float soundVolume)
    {
        _bgmAudioSource.volume = bgmVolume;
        _soundAudioSource.volume = soundVolume;
    }

    //���������������ǿ�ʵ�֣������κβ���

}

//���屳����������ö��
public enum BGMType
{
    AboutUS,
    BGM_1,
    BGM_2,
    BGM_3,
    BGM_4,
    BGM_5,
    Main_Panel
}

//������Ч����ö��
public enum SoundType
{
    Award, Brick, Brick_1, Brick_Destroy, Button, Failed, Fall_Down, Jump, Punishment, Win, WinPerfect
}