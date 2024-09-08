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

        BMX.EnableKeyword("_EMISSION");  //�����Է���
        BMX.SetColor("_EmissionColor", BMX.color);  // ���Է�����ɫ��Ϊ��������ɫ
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        BMX.DisableKeyword("_EMISSION");  //�ر��Է���
    }
}

