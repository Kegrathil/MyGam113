// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33345,y:32911,varname:node_9361,prsc:2|normal-2918-RGB,emission-6324-OUT,custl-1876-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:33342,y:32425,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:33342,y:32560,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:31137,y:32637,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:9684,x:30963,y:33086,prsc:2,pt:True;n:type:ShaderForge.SFN_HalfVector,id:9471,x:31267,y:32985,varname:node_9471,prsc:2;n:type:ShaderForge.SFN_Dot,id:7782,x:31392,y:32771,cmnt:Lambert,varname:node_7782,prsc:2,dt:0|A-6869-OUT,B-9684-OUT;n:type:ShaderForge.SFN_Dot,id:3269,x:31461,y:33084,cmnt:Blinn-Phong,varname:node_3269,prsc:2,dt:0|A-9471-OUT,B-9684-OUT;n:type:ShaderForge.SFN_Multiply,id:5085,x:32627,y:32781,cmnt:Attenuate and Color,varname:node_5085,prsc:2|A-202-RGB,B-7079-OUT,C-6667-OUT,D-1648-OUT;n:type:ShaderForge.SFN_AmbientLight,id:7528,x:32874,y:32936,varname:node_7528,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:202,x:32197,y:32775,ptovrint:False,ptlb:Diffuse Ramp,ptin:_DiffuseRamp,varname:_DiffuseRamp,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-3291-OUT;n:type:ShaderForge.SFN_Tex2d,id:3837,x:32221,y:33160,ptovrint:False,ptlb:Spec Ramp,ptin:_SpecRamp,varname:_SpecRamp,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-1024-OUT;n:type:ShaderForge.SFN_Multiply,id:6386,x:32614,y:33046,varname:node_6386,prsc:2|A-9387-OUT,B-4920-OUT,C-7079-OUT,D-6667-OUT;n:type:ShaderForge.SFN_Add,id:1876,x:32937,y:33214,varname:node_1876,prsc:2|A-5085-OUT,B-6386-OUT;n:type:ShaderForge.SFN_Tex2d,id:2918,x:33072,y:32833,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:_Normal,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Append,id:3291,x:31998,y:32775,varname:node_3291,prsc:2|A-1100-OUT,B-2053-OUT;n:type:ShaderForge.SFN_Clamp01,id:9493,x:31586,y:32771,varname:node_9493,prsc:2|IN-7782-OUT;n:type:ShaderForge.SFN_Vector1,id:2053,x:31742,y:32952,varname:node_2053,prsc:2,v1:0.5;n:type:ShaderForge.SFN_OneMinus,id:1100,x:31742,y:32771,varname:node_1100,prsc:2|IN-9493-OUT;n:type:ShaderForge.SFN_Append,id:1024,x:32004,y:33084,varname:node_1024,prsc:2|A-7063-OUT,B-2053-OUT;n:type:ShaderForge.SFN_Clamp01,id:7396,x:31641,y:33084,varname:node_7396,prsc:2|IN-3269-OUT;n:type:ShaderForge.SFN_OneMinus,id:7063,x:31801,y:33084,varname:node_7063,prsc:2|IN-7396-OUT;n:type:ShaderForge.SFN_ViewVector,id:6772,x:31221,y:33372,varname:node_6772,prsc:2;n:type:ShaderForge.SFN_Dot,id:7777,x:31390,y:33337,varname:node_7777,prsc:2,dt:0|A-9684-OUT,B-6772-OUT;n:type:ShaderForge.SFN_Clamp01,id:479,x:31551,y:33337,varname:node_479,prsc:2|IN-7777-OUT;n:type:ShaderForge.SFN_Append,id:8766,x:32013,y:33334,varname:node_8766,prsc:2|A-5032-OUT,B-2053-OUT;n:type:ShaderForge.SFN_Tex2d,id:192,x:32221,y:33334,ptovrint:False,ptlb:Rim Ramp,ptin:_RimRamp,varname:_RimRamp,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cfe141e225cb93c4f8a483bf8e50e81b,ntxv:2,isnm:False|UVIN-8766-OUT;n:type:ShaderForge.SFN_VertexColor,id:8635,x:33109,y:32082,varname:node_8635,prsc:2;n:type:ShaderForge.SFN_Color,id:587,x:33109,y:32244,ptovrint:False,ptlb:Tint,ptin:_Tint,varname:_Tint,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Set,id:2525,x:33513,y:32270,varname:BaseColor,prsc:2|IN-9253-OUT;n:type:ShaderForge.SFN_Multiply,id:9253,x:33337,y:32270,varname:node_9253,prsc:2|A-8635-RGB,B-587-RGB,C-9166-RGB;n:type:ShaderForge.SFN_Get,id:3449,x:32894,y:33067,varname:node_3449,prsc:2|IN-2525-OUT;n:type:ShaderForge.SFN_Multiply,id:6324,x:33095,y:33031,varname:node_6324,prsc:2|A-7528-RGB,B-3449-OUT,C-5108-OUT;n:type:ShaderForge.SFN_Sign,id:7702,x:31612,y:32603,varname:node_7702,prsc:2|IN-7782-OUT;n:type:ShaderForge.SFN_Clamp01,id:584,x:31804,y:32603,varname:node_584,prsc:2|IN-7702-OUT;n:type:ShaderForge.SFN_Set,id:6528,x:31982,y:32603,varname:CorrectSide,prsc:2|IN-584-OUT;n:type:ShaderForge.SFN_Get,id:6667,x:32351,y:32884,varname:node_6667,prsc:2|IN-6528-OUT;n:type:ShaderForge.SFN_Set,id:7899,x:33759,y:32425,varname:LightXBaseCol,prsc:2|IN-1849-OUT;n:type:ShaderForge.SFN_Multiply,id:1849,x:33579,y:32425,varname:node_1849,prsc:2|A-9253-OUT,B-8068-OUT,C-3406-RGB;n:type:ShaderForge.SFN_Get,id:7079,x:32351,y:32935,varname:node_7079,prsc:2|IN-7899-OUT;n:type:ShaderForge.SFN_OneMinus,id:5032,x:31712,y:33337,varname:node_5032,prsc:2|IN-479-OUT;n:type:ShaderForge.SFN_Slider,id:1648,x:32161,y:32672,ptovrint:False,ptlb:DiffuseContrib,ptin:_DiffuseContrib,varname:node_1648,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4,max:1;n:type:ShaderForge.SFN_Slider,id:4920,x:32182,y:32998,ptovrint:False,ptlb:SpecularContrib,ptin:_SpecularContrib,varname:_DiffuseContrib_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Tex2d,id:9166,x:33109,y:32425,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_9166,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:9264,x:32221,y:33084,ptovrint:False,ptlb:SpecularPower,ptin:_SpecularPower,varname:node_9264,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:9387,x:32406,y:33129,varname:node_9387,prsc:2|VAL-3837-RGB,EXP-9264-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4960,x:32240,y:33533,ptovrint:False,ptlb:RimScale,ptin:_RimScale,varname:node_4960,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:5108,x:32429,y:33391,varname:node_5108,prsc:2|A-192-RGB,B-4960-OUT;proporder:587-9166-2918-1648-202-4920-9264-3837-192-4960;pass:END;sub:END;*/

Shader "AID/Custom/Ramp" {
    Properties {
        _Tint ("Tint", Color) = (1,1,1,1)
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _DiffuseContrib ("DiffuseContrib", Range(0, 1)) = 0.4
        _DiffuseRamp ("Diffuse Ramp", 2D) = "black" {}
        _SpecularContrib ("SpecularContrib", Range(0, 1)) = 0.2
        _SpecularPower ("SpecularPower", Float ) = 1
        _SpecRamp ("Spec Ramp", 2D) = "black" {}
        _RimRamp ("Rim Ramp", 2D) = "black" {}
        _RimScale ("RimScale", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _DiffuseRamp; uniform float4 _DiffuseRamp_ST;
            uniform sampler2D _SpecRamp; uniform float4 _SpecRamp_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _RimRamp; uniform float4 _RimRamp_ST;
            uniform float4 _Tint;
            uniform float _DiffuseContrib;
            uniform float _SpecularContrib;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _SpecularPower;
            uniform float _RimScale;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_9253 = (i.vertexColor.rgb*_Tint.rgb*_Diffuse_var.rgb);
                float3 BaseColor = node_9253;
                float node_2053 = 0.5;
                float2 node_8766 = float2((1.0 - saturate(dot(normalDirection,viewDirection))),node_2053);
                float4 _RimRamp_var = tex2D(_RimRamp,TRANSFORM_TEX(node_8766, _RimRamp));
                float3 emissive = (UNITY_LIGHTMODEL_AMBIENT.rgb*BaseColor*(_RimRamp_var.rgb*_RimScale));
                float node_7782 = dot(lightDirection,normalDirection); // Lambert
                float2 node_3291 = float2((1.0 - saturate(node_7782)),node_2053);
                float4 _DiffuseRamp_var = tex2D(_DiffuseRamp,TRANSFORM_TEX(node_3291, _DiffuseRamp));
                float3 LightXBaseCol = (node_9253*attenuation*_LightColor0.rgb);
                float3 node_7079 = LightXBaseCol;
                float CorrectSide = saturate(sign(node_7782));
                float node_6667 = CorrectSide;
                float2 node_1024 = float2((1.0 - saturate(dot(halfDirection,normalDirection))),node_2053);
                float4 _SpecRamp_var = tex2D(_SpecRamp,TRANSFORM_TEX(node_1024, _SpecRamp));
                float3 finalColor = emissive + ((_DiffuseRamp_var.rgb*node_7079*node_6667*_DiffuseContrib)+(pow(_SpecRamp_var.rgb,_SpecularPower)*_SpecularContrib*node_7079*node_6667));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _DiffuseRamp; uniform float4 _DiffuseRamp_ST;
            uniform sampler2D _SpecRamp; uniform float4 _SpecRamp_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _RimRamp; uniform float4 _RimRamp_ST;
            uniform float4 _Tint;
            uniform float _DiffuseContrib;
            uniform float _SpecularContrib;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _SpecularPower;
            uniform float _RimScale;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_7782 = dot(lightDirection,normalDirection); // Lambert
                float node_2053 = 0.5;
                float2 node_3291 = float2((1.0 - saturate(node_7782)),node_2053);
                float4 _DiffuseRamp_var = tex2D(_DiffuseRamp,TRANSFORM_TEX(node_3291, _DiffuseRamp));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_9253 = (i.vertexColor.rgb*_Tint.rgb*_Diffuse_var.rgb);
                float3 LightXBaseCol = (node_9253*attenuation*_LightColor0.rgb);
                float3 node_7079 = LightXBaseCol;
                float CorrectSide = saturate(sign(node_7782));
                float node_6667 = CorrectSide;
                float2 node_1024 = float2((1.0 - saturate(dot(halfDirection,normalDirection))),node_2053);
                float4 _SpecRamp_var = tex2D(_SpecRamp,TRANSFORM_TEX(node_1024, _SpecRamp));
                float3 finalColor = ((_DiffuseRamp_var.rgb*node_7079*node_6667*_DiffuseContrib)+(pow(_SpecRamp_var.rgb,_SpecularPower)*_SpecularContrib*node_7079*node_6667));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
