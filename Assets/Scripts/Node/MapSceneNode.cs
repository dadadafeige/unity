using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSceneNode : MonoBehaviour
{
    public GameObject obj;
    public Texture2D image; // ����ָ��Ҫ�鿴ͨ����ͼ��
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
            Debug.Log("��ͼ����� " + channelCount + " ��ͨ��");
        else
            Debug.LogError(gameObject.name+ "�޷���ȡͼ���ͨ����Ϣ");
    }
    public void SetData(UIPlayerStory uIPlayer)
    {
        this.uIPlayer = uIPlayer;

    }
    public bool ClickNode()
    {

        if (sceneId <= 0)
        {
            Common.Instance.ShowTips("����ûʲô�������~");
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
            Common.Instance.ShowTips("��ǰ����δ����");
            return false;
        }


    }
}
