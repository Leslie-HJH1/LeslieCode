


/*

BaseMap
BaseColor
Light_Base
NormalMap

RimLight
Light_Rim
RimMask

LightDirMask

APRim
Light_APRim
ApRimMask


*/


Shader "hjh/shader"{

    Properties{
        //[HideInInspector] _simpleUI ("SimpleUI", Int ) = 0
        _MainTex ("BaseMap", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_LightColor_Base("Is_LightColor_BaseColor", Float) = 1
		
		_NormalMap("NormalMap", 2D) = "bump" {}
		_BumpScale("Normal Scale", Range(0, 1)) = 1
		[Toggle(_)] _Is_NormalMapToBase("Is_NormalMapToBase", Float) = 0

		[Toggle(_)] _Is_NormalMapToRimLight("Is_NormalMapToRimLight", Float) = 0


		//基础贴图
		[Toggle(_)] _Set_SystemShadowsToBase("Set_SystemShadowsToBase", Float) = 1
		_Tweak_SystemShadowsLevel("Tweak_SystemShadowsLevel", Range(-0.5, 0.5)) = 0
		_BaseColor_Step("BaseColor_Step", Range(0, 1)) = 0.5
		_BaseShade_Feather("Base/Shade_Feather", Range(0.0001, 1)) = 0.0001
		_ShadeColor_Step("ShadeColor_Step", Range(0, 1)) = 0
		_1st2nd_Shades_Feather("1st/2nd_Shades_Feather", Range(0.0001, 1)) = 0.0001

		[HideInInspector] _1st_ShadeColor_Step("1st_ShadeColor_Step", Range(0, 1)) = 0.5
		[HideInInspector] _1st_ShadeColor_Feather("1st_ShadeColor_Feather", Range(0.0001, 1)) = 0.0001
		[HideInInspector] _2nd_ShadeColor_Step("2nd_ShadeColor_Step", Range(0, 1)) = 0
		[HideInInspector] _2nd_ShadeColor_Feather("2nd_ShadeColor_Feather", Range(0.0001, 1)) = 0.0001

		_1st_ShadeMap("1st_ShadeMap", 2D) = "white" {}
		_1st_ShadeColor("1st_ShadeColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_LightColor_1st_Shade("Is_LightColor_1st_Shade", Float) = 1
		[Toggle(_)] _Use_BaseAs1st("Use BaseMap as 1st_ShadeMap", Float) = 0

		_2nd_ShadeMap("2nd_ShadeMap", 2D) = "white" {}
		_2nd_ShadeColor("2nd_ShadeColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_LightColor_2nd_Shade("Is_LightColor_2nd_Shade", Float) = 1
		[Toggle(_)] _Use_1stAs2nd("Use 1st_ShadeMap as 2nd_ShadeMap", Float) = 0


		_Set_1st_ShadePosition("Set_1st_ShadePosition", 2D) = "white" {}
		_Set_2nd_ShadePosition("Set_2nd_ShadePosition", 2D) = "white" {}
		

		//高光贴图
		_HighColor("HighColor", Color) = (0,0,0,1)
		_HighColor_Tex("HighColor_Tex", 2D) = "white" {}
		[Toggle(_)] _Is_LightColor_HighColor("Is_LightColor_HighColor", Float) = 1
		[Toggle(_)] _Is_NormalMapToHighColor("Is_NormalMapToHighColor", Float) = 0
		_HighColor_Power("HighColor_Power", Range(0, 1)) = 0
		[Toggle(_)] _Is_SpecularToHighColor("Is_SpecularToHighColor", Float) = 0
		[Toggle(_)] _Is_BlendAddToHiColor("Is_BlendAddToHiColor", Float) = 0
		[Toggle(_)] _Is_UseTweakHighColorOnShadow("Is_UseTweakHighColorOnShadow", Float) = 0
		_TweakHighColorOnShadow("TweakHighColorOnShadow", Range(0, 1)) = 0
		_Set_HighColorMask("Set_HighColorMask", 2D) = "white" {}
		_Tweak_HighColorMaskLevel("Tweak_HighColorMaskLevel", Range(-1, 1)) = 0



		//边缘光
		[Toggle(_)] _RimLight("RimLight", Float) = 0
		_RimLightColor("RimLightColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_LightColor_RimLight("Is_LightColor_RimLight", Float) = 1
		
		_RimLight_Power("RimLight_Power", Range(0, 1)) = 0.1
		_RimLight_InsideMask("RimLight_InsideMask", Range(0.0001, 1)) = 0.0001
		[Toggle(_)] _RimLight_FeatherOff("RimLight_FeatherOff", Float) = 0


		//追加 光照方向Mask  反向边缘光
		[Toggle(_)] _LightDirection_MaskOn("LightDirection_MaskOn", Float) = 0
		_Tweak_LightDirection_MaskLevel("Tweak_LightDirection_MaskLevel", Range(0, 0.5)) = 0
		[Toggle(_)] _Add_Antipodean_RimLight("Add_Antipodean_RimLight", Float) = 0
		_Ap_RimLightColor("Ap_RimLightColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_LightColor_Ap_RimLight("Is_LightColor_Ap_RimLight", Float) = 1
		_Ap_RimLight_Power("Ap_RimLight_Power", Range(0, 1)) = 0.1
		[Toggle(_)] _Ap_RimLight_FeatherOff("Ap_RimLight_FeatherOff", Float) = 0

		//边缘光Mask
		_Set_RimLightMask("Set_RimLightMask", 2D) = "white" {}
		_Tweak_RimLightMaskLevel("Tweak_RimLightMaskLevel", Range(-1, 1)) = 0       
    }
    
	SubShader{
		Tags {"RenderType" = "Opaque"}

		/*Pass {
			Name "Outline"
			Tags {
				"LightMode" = "ForwardBase"
			}
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma only_renderers d3d9 d3d11 glcore gles gles3 metal vulkan xboxone ps4 switch
			#pragma target 3.0
			#pragma multi_compile _IS_OUTLINE_CLIPPING_NO 
			#pragma multi_compile _OUTLINE_NML _OUTLINE_POS
			#include "outline.hlsl"
			ENDCG
		}*/


		Pass {
			Name "FORWARD"
			Tags {
				"LightMode" = "ForwardBase"
			}

			Cull[_CullMode]


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#pragma multi_compile_fwdbase_fullshadows
			#pragma multi_compile_fog
			#pragma only_renderers d3d9 d3d11 glcore gles gles3 metal vulkan xboxone ps4 switch
			#pragma target 3.0

			#pragma multi_compile _IS_CLIPPING_OFF
			#pragma multi_compile _IS_PASS_FWDBASE
			#pragma multi_compile _EMISSIVE_SIMPLE _EMISSIVE_ANIMATION
			//
			#include "test.hlsl"

			ENDCG
		}


	}



    FallBack "Legacy Shaders/VertexLit"
    //CustomEditor "EditorTest"
}