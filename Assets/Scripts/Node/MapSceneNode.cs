using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSceneNode : MonoBehaviour
{
    public GameObject obj;
    public Texture2D image; // 这里指定要查看通道的图像
    public Canvas canvas;
    public RectTransform rect;
    UIPlayerStory uIPlayer;
    public int sceneId;
    scenecnofigData scenecfg;
    private void Awake()
    {
        canvas = gameObject.GetComponent<Canvas>();
        rect = gameObject.GetComponent<RectTransform>();
    }
    void Start()
    {
        image = gameObject.GetComponent<UnityEngine.UI.Image>().sprite.texture;
        int channelCount = image.format == TextureFormat.ARGB32 ? 3 : (image.format == TextureFormat.RGBA32 ? 4 : -1);
        if (sceneId > 0)
        {

            scenecfg = GetCfgManage.Instance.GetCfgByNameAndId<scenecnofigData>("scene", sceneId);

        }
        if (channelCount != -1)
            Debug.Log("该图像包含 " + channelCount + " 个通道");
        else
            Debug.LogError(gameObject.name+ "无法获取图像的通道信息");
    }
    public void SetData(UIPlayerStory uIPlayer)
    {
        this.uIPlayer = uIPlayer;

    }
    public bool ClickNode()
    {

        if (sceneId <= 0)
        {
            Common.Instance.ShowTips("这里没什么可玩的了~");
            return false;
        }
            MissionNode mission = MissionManage.GetMissionNodeById(scenecfg.missionid);
        if (mission.missionState == MISSIONSTATE.DAILY)
        {
            uIPlayer.missioNode = mission;
            uIPlayer.curPlot = mission.GetPlotList()[0];
            uIPlayer.ShowUI();
            GameManage.curMissionId = scenecfg.missionid;
            GameManage.curGameMissionId = GameManage.curMissionId;
            GameManage.isMap = true;
            return true;
        }
        else
        {
            Common.Instance.ShowTips("当前场景未解锁");
            return false;
        }


    }
}
