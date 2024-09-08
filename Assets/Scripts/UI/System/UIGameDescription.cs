using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameDescription : UIBase
{
    public Image dec;
    public Button close_btn;
    // Start is called before the first frame update
    public override void OnStart()
    {
        close_btn.onClick.AddListener(CloseSelf);

    }
    public void SetDecById(int id)
    {
        dec.sprite = UiManager.getTextureSpriteByNmae("game_description_texture","game_description_" + id);


    }
}
