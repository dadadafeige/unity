Shader "Custom/Rim"

{

    Properties

    {

        _MainColor("MainColor", Color) = (1,1,1,1)

        _InsideRimColor("InsideRimColor", Color) = (1,1,1,1)

        _InsideRimValue("InsideRimValue", Range(0, 10)) = 0

        _InsideRimInsity("InsideRimInsity", Range(0, 10)) = 1

        _OutsideRim("OutsideRim", Float) = 1

        _OutsideRimColor("OutsideRimColor", Color) = (1,1,1,1)

        _OutsideRimValue("OutsideRimValue", Float) = 1

        _OutsideRimInsity("OutsideRimInsity", Range(0, 10)) = 1

    }

    SubShader

    {

        pass

        {

            Tags{"Queue"="Transparent"}

            CGPROGRAM

            #pragma vertex vert

            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f

            {

                float4 pos : SV_POSITION;

                float4 worldVertex : TEXCOORD0;

                half3 worldNormal : TEXCOORD1;

            };

           

            fixed4 _MainColor;

            fixed4 _InsideRimColor;

            float _InsideRimValue;

            float _InsideRimInsity;

            v2f vert(appdata_base v)

            {

                v2f f;

                f.pos = UnityObjectToClipPos(v.vertex);

                f.worldVertex = mul(unity_ObjectToWorld, v.vertex);

                f.worldNormal = UnityObjectToWorldNormal(v.normal);

                return f;

            }

            fixed4 frag(v2f i) : SV_TARGET

            {

                half3 worldNormal = normalize(i.worldNormal);

                fixed3 worldView = normalize(UnityWorldSpaceViewDir(i.worldVertex).xyz);

                half insideRim =max(0, dot(worldNormal, worldView));

                insideRim = 1 - insideRim;

                fixed3 insideRimColor = (pow(insideRim, _InsideRimValue) * _InsideRimInsity) * _InsideRimColor.rgb;

                fixed3 col = _MainColor.rgb + insideRimColor;

                return fixed4(col, 1);

            }

            ENDCG

        }

        pass

        {

            Tags{"Queue"="Transparent"}

            Cull Front

            Blend srcAlpha One

            ZWrite Off

            CGPROGRAM

            #pragma vertex vert

            #pragma fragment frag

            #include "UnityCG.cginc"

            #include "Lighting.cginc"

            struct v2f

            {

                float4 pos : SV_POSITION;

                float4 worldVertex : TEXCOORD0;

                half3 worldNormal : TEXCOORD1;

            };

           

            fixed4 _MainColor;

            float _OutsideRim;

            fixed4 _OutsideRimColor;

            float _OutsideRimValue;

            float _OutsideRimInsity;

            v2f vert(appdata_base v)

            {

                v2f f;

                // v.normal.z = -0.5;

                // half3 normal = saturate(v.normal);

                v.vertex.xyz += v.normal * _OutsideRim;

                f.pos = UnityObjectToClipPos(v.vertex);

                f.worldVertex = mul(unity_ObjectToWorld, v.vertex);

                f.worldNormal = UnityObjectToWorldNormal(v.normal);

                return f;

            }

            fixed4 frag(v2f i) : SV_TARGET

            {

                half3 worldNormal = normalize(i.worldNormal);

                fixed3 worldView = normalize(i.worldVertex.xyz - _WorldSpaceCameraPos.xyz);

                fixed outsideRimPow = saturate(dot(worldNormal, worldView));

                fixed outsideRim = pow(outsideRimPow, _OutsideRimValue) * _OutsideRimInsity;

                return fixed4(_OutsideRimColor.rgb, outsideRim);

            }

            ENDCG

        }

    }

}
