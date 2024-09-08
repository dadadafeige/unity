Shader "Unlit/005"
{
    Properties
    {
        // 漫反射颜色
        _Diffuse ("Diffuse",Color) = (1,1,1,1)
        // 高光反射颜色值
        _Specular ("Specular",Color) = (1,1,1,1)
        // 高光反射值
        _Gloss ("_Gloss",range(1,100)) = 5
        // 主纹理
        _MaxTex ("_MaxTex",2d) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            sampler2D _MaxTex;
            float4 _MaxTex_ST;
            float4 _Specular;
            float4 _Diffuse;
            float _Gloss;
            
            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD1;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD2;
            };
 
            v2f vert (appdata_base v)
            {
                v2f o;
                // 将对象空间中的点变换到齐次坐标中的摄像机裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 计算uv坐标
                o.uv = TRANSFORM_TEX(v.texcoord,_MaxTex);
                // o.uv = v.texcoord.xy * _MaxTex_ST.xy + _MaxTex_ST.zw;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(o.vertex));
                return o;   
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                // 纹理采样
                float3 texColor = tex2D(_MaxTex,i.uv);
 
                // 计算漫反射
                float3 diffuse = texColor * _LightColor0.rgb * _Diffuse.rgb * (dot(_WorldSpaceLightPos0.xyz,i.worldNormal) * 0.5 + 0.5);
                // 计算高光
                float3 halfVector = normalize(normalize(_WorldSpaceLightPos0.xyz) + i.viewDir);
                // 计算高光
                float3 specular = texColor * _LightColor0.rgb * _Specular.rgb * pow(max(0,dot(halfVector,i.worldNormal)),_Gloss);
                
                return float4(diffuse + specular,1);
            }
            ENDCG
        }
    }
}