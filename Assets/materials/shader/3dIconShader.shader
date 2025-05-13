Shader "Custom/BillboardAlpha" {
Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
        }

SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

        Pass{
                Cull Off
                ZTest Always
                Blend SrcAlpha OneMinusSrcAlpha

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _MainTex;
float _Scale;

struct v2f {
        float4 pos:SV_POSITION;
        float2 texc:TEXCOORD0;
};

v2f vert(appdata_base v)
{
        v2f o;
        float4 ori=mul(UNITY_MATRIX_MV,float4(0,0,0,1));
        float4 vt=v.vertex*_Scale;
        vt.y=-vt.z;
        vt.z=0;
        vt.x=-vt.x;


        vt.xyz+=ori.xyz;//result is vt.z==ori.z ,so the distance to camera keeped ,and screen size keeped
        o.pos=mul(UNITY_MATRIX_P,vt);

        o.texc=v.texcoord;
        return o;
}

float4 frag(v2f i):COLOR
{
        return tex2D(_MainTex,i.texc);
}
ENDCG
}//endpass
}
}