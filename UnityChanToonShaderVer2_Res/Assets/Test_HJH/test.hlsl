uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
uniform float4 _BaseColor;//调整基础颜色
uniform fixed _Is_LightColor_Base;//光照色*基础色

//法线贴图
uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
uniform float _BumpScale;//法线调整参数
uniform fixed _Is_NormalMapToBase;//用法线贴图改变显示的法线

uniform fixed _Use_BaseAs1st;
uniform fixed _Use_1stAs2nd;

//基础贴图
uniform sampler2D _1st_ShadeMap; uniform float4 _1st_ShadeMap_ST;
uniform float4 _1st_ShadeColor;
uniform fixed _Is_LightColor_1st_Shade;//光照色*基础色*纹理色
uniform sampler2D _2nd_ShadeMap; uniform float4 _2nd_ShadeMap_ST;
uniform float4 _2nd_ShadeColor;
uniform fixed _Is_LightColor_2nd_Shade;//光照色*基础色*纹理色

//自阴影相关
uniform fixed _Set_SystemShadowsToBase;
uniform float _Tweak_SystemShadowsLevel;//阴影等级
//基础色羽化
uniform float _BaseColor_Step;
uniform float _BaseShade_Feather;
//纹理色羽化
uniform float _ShadeColor_Step;
uniform float _1st2nd_Shades_Feather;

//阴影控制贴图
uniform sampler2D _Set_1st_ShadePosition; uniform float4 _Set_1st_ShadePosition_ST;
uniform sampler2D _Set_2nd_ShadePosition; uniform float4 _Set_2nd_ShadePosition_ST;


//高光
uniform float4 _HighColor;
uniform sampler2D _HighColor_Tex; uniform float4 _HighColor_Tex_ST;
uniform fixed _Is_LightColor_HighColor;
uniform fixed _Is_NormalMapToHighColor;
uniform float _HighColor_Power;
uniform fixed _Is_SpecularToHighColor;
uniform fixed _Is_BlendAddToHiColor;
uniform fixed _Is_UseTweakHighColorOnShadow;
uniform float _TweakHighColorOnShadow;
uniform sampler2D _Set_HighColorMask; uniform float4 _Set_HighColorMask_ST;
uniform float _Tweak_HighColorMaskLevel;



//边缘光
uniform fixed _RimLight;
uniform float4 _RimLightColor;
uniform fixed _Is_LightColor_RimLight;
uniform fixed _Is_NormalMapToRimLight;
uniform float _RimLight_Power;
uniform float _RimLight_InsideMask;
uniform fixed _RimLight_FeatherOff;

uniform fixed _LightDirection_MaskOn;
uniform float _Tweak_LightDirection_MaskLevel;
uniform fixed _Add_Antipodean_RimLight;
uniform float4 _Ap_RimLightColor;
uniform fixed _Is_LightColor_Ap_RimLight;
uniform float _Ap_RimLight_Power;
uniform fixed _Ap_RimLight_FeatherOff;

uniform sampler2D _Set_RimLightMask; uniform float4 _Set_RimLightMask_ST;
uniform float _Tweak_RimLightMaskLevel;




struct VertexInput {
	float4 vertex:POSITION;
	float3 normal: NORMAL;
	float4 tangent:TANGENT;
	float2 texcoord0:TEXCOORD0;	
};

struct VertexOutput {
	float4 pos:SV_POSITION;
	float2 uv0:TEXCOORD0;
	float4 posWorld:TEXCOORD1;
	float3 normalDir:TEXCOORD2;
	float3 tangentDir:TEXCOORD3;
	float3 bitangentDir:TEXCOORD4;
	
	float mirrorFlag : TEXCOORD5;
	LIGHTING_COORDS(6, 7)
	UNITY_FOG_COORDS(8)
};

VertexOutput vert(VertexInput v) {
	VertexOutput o = (VertexOutput)0;
	o.uv0 = v.texcoord0;
	o.normalDir= UnityObjectToWorldNormal(v.normal);	
	o.tangentDir= UnityObjectToWorldDir(v.tangent.xyz);
	o.bitangentDir= normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	o.pos = UnityObjectToClipPos(v.vertex);
	return o;
}

float4 frag(VertexOutput i) : SV_TARGET{
	//基于世界空间

	//基本工具
	i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);//切线空间
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);//世界-视角方向
	float3 lightDirection = _WorldSpaceLightPos0.xyz;//世界-光照方向
	float3 Set_LightColor = _LightColor0.rgb;//光照颜色
	//半角方向
	float3 halfDirection = normalize(viewDirection + lightDirection);

	//基本着色
	float2 Set_UV0 = i.uv0;//UV信息
	float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(Set_UV0, _MainTex));	
	float3 Set_BaseColor = lerp((_BaseColor.rgb*_MainTex_var.rgb), ((_BaseColor.rgb*_MainTex_var.rgb)*Set_LightColor), _Is_LightColor_Base);

	//法线贴图
	float3 _NormalMap_var = UnpackScaleNormal(tex2D(_NormalMap, TRANSFORM_TEX(Set_UV0, _NormalMap)), _BumpScale);
	float3 normalLocal = _NormalMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals 法线方向转换到 切线空间


	//光衰减??? attenuation好像是宏定义后 Unity自动会为其赋值
	UNITY_LIGHT_ATTENUATION(attenuation, i, i.posWorld.xyz);




//********************基本着色*********************
	//第一纹理 主纹理色*第一纹理色
	float4 _1st_ShadeMap_var = lerp(tex2D(_1st_ShadeMap, TRANSFORM_TEX(Set_UV0, _1st_ShadeMap)), _MainTex_var, _Use_BaseAs1st);
	//光照色*主纹理色*第一纹理色
	float3 Set_1st_ShadeColor = lerp((_1st_ShadeColor.rgb*_1st_ShadeMap_var.rgb), ((_1st_ShadeColor.rgb*_1st_ShadeMap_var.rgb)*Set_LightColor), _Is_LightColor_1st_Shade);
	//第二纹理 主纹理色*第二纹理色
	float4 _2nd_ShadeMap_var = lerp(tex2D(_2nd_ShadeMap, TRANSFORM_TEX(Set_UV0, _2nd_ShadeMap)), _1st_ShadeMap_var, _Use_1stAs2nd);
	float3 Set_2nd_ShadeColor = lerp((_2nd_ShadeColor.rgb*_2nd_ShadeMap_var.rgb), ((_2nd_ShadeColor.rgb*_2nd_ShadeMap_var.rgb)*Set_LightColor), _Is_LightColor_2nd_Shade);

	//半兰伯特 光照计算再切线空间下
	float _HalfLambert_var = 0.5*dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase), lightDirection) + 0.5;
	//自阴影位置
	float4 _Set_2nd_ShadePosition_var = tex2D(_Set_2nd_ShadePosition, TRANSFORM_TEX(Set_UV0, _Set_2nd_ShadePosition));
	float4 _Set_1st_ShadePosition_var = tex2D(_Set_1st_ShadePosition, TRANSFORM_TEX(Set_UV0, _Set_1st_ShadePosition));

	//计算自阴影
	//Minmimum value is same as the Minimum Feather's value with the Minimum Step's value as threshold.
	//最小值与Minimum Feather的值相同，以Minimum Step的值为阈值。
	//自阴影等级 
	float _SystemShadowsLevel_var = (attenuation*0.5) + 0.5 + _Tweak_SystemShadowsLevel > 0.001 ? (attenuation*0.5) + 0.5 + _Tweak_SystemShadowsLevel : 0.0001;
	//最后阴影
	float Set_FinalShadowMask =
		saturate((1.0 +
		(
			(
				lerp(_HalfLambert_var,
				_HalfLambert_var*saturate(_SystemShadowsLevel_var),
				_Set_SystemShadowsToBase
				)//设置_Set_SystemShadowsToBase为0 那么就是_HalfLambert_var 半兰伯特 光照模型
				-
				(_BaseColor_Step - _BaseShade_Feather)
			)
			*
			((1.0 - _Set_1st_ShadePosition_var.rgb).r - 1.0)//_Set_1st_ShadePosition_var 由于没有_Set_1st_ShadePosition 值为0
		)
			/
			(_BaseColor_Step - (_BaseColor_Step - _BaseShade_Feather))
			));

	//把三原色组合起来
	float3 Set_FinalBaseColor = 
		lerp(
			Set_BaseColor, 
			lerp(
				//把双贴图进行合并
				Set_1st_ShadeColor, 
				Set_2nd_ShadeColor, 
				saturate((1.0 + 
							((_HalfLambert_var - (_ShadeColor_Step - _1st2nd_Shades_Feather)) * ((1.0 - _Set_2nd_ShadePosition_var.rgb).r - 1.0)) 
							/ 
							(_ShadeColor_Step - (_ShadeColor_Step - _1st2nd_Shades_Feather))
						))
				), 
			Set_FinalShadowMask); // Final Color
//********************基本着色*********************



//********************高光*********************
	//高光遮罩
	float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask, TRANSFORM_TEX(Set_UV0, _Set_HighColorMask));
	//高光反射
	//高光 光照模型
	//光照计算再切线空间下
	float _Specular_var = 0.5*dot(halfDirection, lerp(i.normalDir, normalDirection, _Is_NormalMapToHighColor)) + 0.5; //  Specular                
	//遮罩强度
	float _TweakHighColorMask_var = (saturate((_Set_HighColorMask_var.g + _Tweak_HighColorMaskLevel))*lerp((1.0 - step(_Specular_var, (1.0 - pow(_HighColor_Power, 5)))), pow(_Specular_var, exp2(lerp(11, 1, _HighColor_Power))), _Is_SpecularToHighColor));
	//高光贴图
	//纹理色
	float4 _HighColor_Tex_var = tex2D(_HighColor_Tex, TRANSFORM_TEX(Set_UV0, _HighColor_Tex));
	//纹理色*光照色*基础色*遮罩色
	float3 _HighColor_var = (
		lerp(
			(_HighColor_Tex_var.rgb*_HighColor.rgb), 
			((_HighColor_Tex_var.rgb*_HighColor.rgb)*Set_LightColor), 
			_Is_LightColor_HighColor
			)
			*
			_TweakHighColorMask_var);

	//Composition: 3 Basic Colors and HighColor as Set_HighColor
	//组成:3个基本颜色和HighColor作为Set_HighColor
	//把高光组合进去
	float3 Set_HighColor =
		(
			lerp(
				saturate((Set_FinalBaseColor - _TweakHighColorMask_var)),
				Set_FinalBaseColor,
				lerp(_Is_BlendAddToHiColor, 1.0, _Is_SpecularToHighColor))
			+
			lerp(
				_HighColor_var,
				(_HighColor_var*((1.0 - Set_FinalShadowMask) + (Set_FinalShadowMask*_TweakHighColorOnShadow))),
				_Is_UseTweakHighColorOnShadow)
			);

//********************高光*********************
	




//********************边缘光*********************
	//采样边缘光Mask(如果有的话)
	float4 _Set_RimLightMask_var = tex2D(_Set_RimLightMask, TRANSFORM_TEX(Set_UV0, _Set_RimLightMask));

	//正面边缘光属性与光照合成 A1
	float3 _Is_LightColor_RimLight_var =
		lerp(
			_RimLightColor.rgb,
			(_RimLightColor.rgb*Set_LightColor),
			_Is_LightColor_RimLight
		);

	//边缘光范围
	float _RimArea_var = (1.0 - dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToRimLight), viewDirection));

	//边缘光强度
	float _RimLightPower_var = pow(_RimArea_var, exp2(lerp(3, 0, _RimLight_Power)));

	//根据_RimLight_FeatherOff设置来调整是否需要_RimLight_InsideMask起作用 得到Mask羽化作用结果 A2
	float _Rimlight_InsideMask_var =
		saturate(
			lerp(
			(0.0 + ((_RimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)),
				step(_RimLight_InsideMask, _RimLightPower_var),
				_RimLight_FeatherOff)
		);


	

	//半兰伯特
	float _VertHalfLambert_var = 0.5*dot(i.normalDir, lightDirection) + 0.5;

	//A1与A2合成前向光
	float3 _LightDirection_MaskOn_var =
		lerp(
		(_Is_LightColor_RimLight_var*_Rimlight_InsideMask_var),
			(_Is_LightColor_RimLight_var*saturate(
			(_Rimlight_InsideMask_var - ((1.0 - _VertHalfLambert_var) + _Tweak_LightDirection_MaskLevel))
			)),
			_LightDirection_MaskOn);



	//上面是前向光 后面是背向光 原理相同
	//背光向 边缘光
	float _ApRimLightPower_var = pow(_RimArea_var, exp2(lerp(3, 0, _Ap_RimLight_Power)));
	float3 Set_RimLight = (
		saturate((_Set_RimLightMask_var.g + _Tweak_RimLightMaskLevel))*//如果有Mask那么就乘上这个属性
		lerp(_LightDirection_MaskOn_var,
		(_LightDirection_MaskOn_var + (//后面是背光侧 前面是向光侧_LightDirection_MaskOn_var
			lerp(
				_Ap_RimLightColor.rgb,
				(_Ap_RimLightColor.rgb*Set_LightColor),
				_Is_LightColor_Ap_RimLight)
			*
			saturate(
			(lerp(
				(0.0 + ((_ApRimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)),
				step(_RimLight_InsideMask, _ApRimLightPower_var),
				_Ap_RimLight_FeatherOff)
				-
				(saturate(_VertHalfLambert_var) + _Tweak_LightDirection_MaskLevel)
				)
			)
			)
			),
			_Add_Antipodean_RimLight));

	//Composition: HighColor and RimLight as _RimLight_var
	//组成：HighColor and RimLight as _RimLight_var
	//把边缘光组合进去
	float3 _RimLight_var = lerp(Set_HighColor, (Set_HighColor + Set_RimLight), _RimLight);
//********************边缘光*********************




	fixed4 finalRGBA = fixed4(_RimLight_var, 0);
	return finalRGBA;
}