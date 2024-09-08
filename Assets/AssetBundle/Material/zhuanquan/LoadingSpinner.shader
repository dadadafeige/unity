Shader "UI/LoadingSpinner"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1("Color1",Color) = (1,1,1,1)
        _Color2("Color2",Color) = (1,1,1,1)
        _Radiu("_Radiu",Range(0,0.5)) = 0.3
        _Width("_Width",Range(0,0.5)) = 0.1
        _Speed("_Speed",float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Tags{"Queue"="TransParent"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define PI 3.14159
            #define edge 4.0 / min(_ScreenParams.x,_ScreenParams.y)
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 _Color1;
            fixed4 _Color2;
            fixed _Radiu;
            fixed _Width;
            fixed _Speed;
            
            fixed4 frag (v2f i) : SV_Target
            {
                //uv偏移 原点移动到中心
                fixed2 uv = i.uv - 0.5;    
                   
                //圆环
                fixed ring =  (1 - smoothstep(_Radiu,_Radiu + edge,length(uv))) * smoothstep(_Radiu - _Width - edge,_Radiu - _Width,length(uv));            
 
                //渐变
                fixed angle =  _Time.y * _Speed;
                uv = mul(uv,float2x2(cos(angle),sin(angle),-sin(angle),cos(angle)));
                fixed a = atan2(uv.x,uv.y) * 0.2 + 0.5;
                a = clamp(a,0,1);
                
                //小圆
                float circle = 1 - smoothstep(_Width * 0.5, _Width* 0.5 + edge,length(uv - fixed2(0, -_Radiu + _Width * 0.5)));
                a = max(a,circle);      
                a *= ring;
                
                return fixed4(lerp(_Color2.rgb,_Color1.rgb,a),a);
            }
            ENDCG
        }
    }
}