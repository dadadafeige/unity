Shader "Custom/AlphaMaskUIShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _MaskTex ("Mask (A)", 2D) = "white" { }
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Overlay"
            "RenderType" = "Transparent"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            ENDCG

            SetTexture[_MainTex]
            {
                combine primary
            }
        }
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Overlay"
            "RenderType" = "Transparent"
        }

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // 使用遮罩图的 alpha 通道作为遮罩
            clip(tex2D(_MaskTex, IN.uv_MaskTex).a - 0.1);

            // 设置颜色，保持原有的颜色和透明度
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
        }
        ENDCG
    }

    Fallback "UI/Default"
}
