using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class AudioManager
{
    //定义两个音源组件
    private AudioSource _bgmAudioSource;
    private AudioSource _soundAudioSource;
    //定义背景音乐和音效的字典
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
    //初始化方法
    public void OnInit()
    {
        //创建音频管理器、背景音乐、音效三个游戏对象，并设置它们的父节点
        GameObject audioManagerGo = new GameObject("AudioManager");
        GameObject bgm = new GameObject("BGM");
        GameObject sound = new GameObject("Sound");
        bgm.transform.SetParent(audioManagerGo.transform);
        sound.transform.SetParent(audioManagerGo.transform);
        //给背景音乐和音效游戏对象添加音源组件
        _bgmAudioSource = bgm.AddComponent<AudioSource>();
        _soundAudioSource = sound.AddComponent<AudioSource>();
        //设置音源组件的初始值，背景音乐循环播放，音效不循环，都不立即播放
        _bgmAudioSource.loop = true;
        _bgmAudioSource.playOnAwake = true;
        _soundAudioSource.loop = false;
        _soundAudioSource.playOnAwake = false;
        //设置背景音乐的默认音量
        _bgmAudioSource.volume = 0.1f;
        //加载声音资源
      //  LoadAudioResources();
    }

    //加载声音资源
    //private void LoadAudioResources()
    //{
    //    //背景音乐资源的加载
    //    AudioClip aboutUsClip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm1Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm2Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm3Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm4Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip gameBgm5Clip = Resources.Load<AudioClip>("ma1");
    //    AudioClip mainPanelClip = Resources.Load<AudioClip>("ma1");
    //    //将背景音乐资源添加到字典中
    //    _bgmDict = new Dictionary<BGMType, AudioClip>();
    //    _bgmDict.Add(BGMType.AboutUS, aboutUsClip);
    //    _bgmDict.Add(BGMType.BGM_1, gameBgm1Clip);
    //    _bgmDict.Add(BGMType.BGM_2, gameBgm2Clip);
    //    _bgmDict.Add(BGMType.BGM_3, gameBgm3Clip);
    //    _bgmDict.Add(BGMType.BGM_4, gameBgm4Clip);
    //    _bgmDict.Add(BGMType.BGM_5, gameBgm5Clip);
    //    _bgmDict.Add(BGMType.Main_Panel, mainPanelClip);

    //    //音效音乐资源的加载
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
    //    //将音效资源添加到字典中
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

    //播放背景音乐
    public void PlayBGM(string BGMName)
    {

        //停止当前背景音乐的播放
        _bgmAudioSource.Stop();
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "bgm/" + BGMName;
        string assetName = BGMName;
        AudioClip myBGM = AssetBundleManager.Instance.LoadAsset<AudioClip>(assetBundleName, assetName);
        //设置新的背景音乐，并播放
        _bgmAudioSource.clip = myBGM;
        _bgmAudioSource.Play();
    }

    //播放音效
    public void PlaySound(string soundName)
    {
        //停止当前音效的播放
        _soundAudioSource.Stop();
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "sound/" + soundName;
        string assetName = soundName;
        AudioClip mySound = AssetBundleManager.Instance.LoadAsset<AudioClip>(assetBundleName, assetName);
        //设置新的音效，并播放
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
    //关闭BGM播放
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }
    public void StopSound()
    {
        _soundAudioSource.Stop();
    }

    //音量控制
    public void ControlVolume(float bgmVolume, float soundVolume)
    {
        _bgmAudioSource.volume = bgmVolume;
        _soundAudioSource.volume = soundVolume;
    }

    //以下三个方法都是空实现，不做任何操作

}

//定义背景音乐类型枚举
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

//定义音效类型枚举
public enum SoundType
{
    Award, Brick, Brick_1, Brick_Destroy, Button, Failed, Fall_Down, Jump, Punishment, Win, WinPerfect
}