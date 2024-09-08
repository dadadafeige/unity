using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionTest : MonoBehaviour
{
    private Renderer render;
    private Material BMX;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        BMX = render.material;

        BMX.EnableKeyword("_EMISSION");  //开启自发光
        BMX.SetColor("_EmissionColor", BMX.color);  // 将自发光颜色设为材质球颜色
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        BMX.DisableKeyword("_EMISSION");  //关闭自发光
    }
}

