using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNode
{
    public storycnofigData storyCfg { get; }
    public int id {get;}
    public Dictionary<string,ChapterNode> chapterNodeMap = new Dictionary<string,ChapterNode>();
    public ChapterNode firstChapter = null;
    public StoryNode(int id)
    {
        this.id = id;
        storyCfg = GetCfgManage.Instance.GetCfgByNameAndId<storycnofigData>("storycnofig", id);
        initChapter();
    }
   public ChapterNode GetChapterById(string id)
    {
        if (chapterNodeMap.ContainsKey(id)){
            return chapterNodeMap[id];
        }
        else
        {
            Debug.Log("找不到对应章节");
            return null;
        }
    }
    private void initChapter()
    {   
        string[] chapterList = storyCfg.chapterList.Split(",");
        for (int i = 0; i < chapterList.Length; i++)
        {
            string chapterId = chapterList[i];
            if (!chapterNodeMap.ContainsKey(chapterId))
            {
               
                ChapterNode chapterNode = new ChapterNode(chapterId);
                chapterNodeMap.Add(chapterId, chapterNode);
                if (firstChapter == null)
                {
                    firstChapter = chapterNode;

                }
            }
        }
    }
}
