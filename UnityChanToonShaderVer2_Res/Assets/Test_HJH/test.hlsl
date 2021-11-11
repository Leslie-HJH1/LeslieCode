uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
uniform float4 _BaseColor;//����������ɫ
uniform fixed _Is_LightColor_Base;//����ɫ*����ɫ

//������ͼ
uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
uniform float _BumpScale;//���ߵ�������
uniform fixed _Is_NormalMapToBase;//�÷�����ͼ�ı���ʾ�ķ���

uniform fixed _Use_BaseAs1st;
uniform fixed _Use_1stAs2nd;

//������ͼ
uniform sampler2D _1st_ShadeMap; uniform float4 _1st_ShadeMap_ST;
uniform float4 _1st_ShadeColor;
uniform fixed _Is_LightColor_1st_Shade;//����ɫ*����ɫ*����ɫ
uniform sampler2D _2nd_ShadeMap; uniform float4 _2nd_ShadeMap_ST;
uniform float4 _2nd_ShadeColor;
uniform fixed _Is_LightColor_2nd_Shade;//����ɫ*����ɫ*����ɫ

//����Ӱ���
uniform fixed _Set_SystemShadowsToBase;
uniform float _Tweak_SystemShadowsLevel;//��Ӱ�ȼ�
//����ɫ��
uniform float _BaseColor_Step;
uniform float _BaseShade_Feather;
//����ɫ��
uniform float _ShadeColor_Step;
uniform float _1st2nd_Shades_Feather;

//��Ӱ������ͼ
uniform sampler2D _Set_1st_ShadePosition; uniform float4 _Set_1st_ShadePosition_ST;
uniform sampler2D _Set_2nd_ShadePosition; uniform float4 _Set_2nd_ShadePosition_ST;


//�߹�
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



//��Ե��
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
	//��������ռ�

	//��������
	i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);//���߿ռ�
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);//����-�ӽǷ���
	float3 lightDirection = _WorldSpaceLightPos0.xyz;//����-���շ���
	float3 Set_LightColor = _LightColor0.rgb;//������ɫ
	//��Ƿ���
	float3 halfDirection = normalize(viewDirection + lightDirection);

	//������ɫ
	float2 Set_UV0 = i.uv0;//UV��Ϣ
	float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(Set_UV0, _MainTex));	
	float3 Set_BaseColor = lerp((_BaseColor.rgb*_MainTex_var.rgb), ((_BaseColor.rgb*_MainTex_var.rgb)*Set_LightColor), _Is_LightColor_Base);

	//������ͼ
	float3 _NormalMap_var = UnpackScaleNormal(tex2D(_NormalMap, TRANSFORM_TEX(Set_UV0, _NormalMap)), _BumpScale);
	float3 normalLocal = _NormalMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals ���߷���ת���� ���߿ռ�


	//��˥��??? attenuation�����Ǻ궨��� Unity�Զ���Ϊ�丳ֵ
	UNITY_LIGHT_ATTENUATION(attenuation, i, i.posWorld.xyz);




//********************������ɫ*********************
	//��һ���� ������ɫ*��һ����ɫ
	float4 _1st_ShadeMap_var = lerp(tex2D(_1st_ShadeMap, TRANSFORM_TEX(Set_UV0, _1st_ShadeMap)), _MainTex_var, _Use_BaseAs1st);
	//����ɫ*������ɫ*��һ����ɫ
	float3 Set_1st_ShadeColor = lerp((_1st_ShadeColor.rgb*_1st_ShadeMap_var.rgb), ((_1st_ShadeColor.rgb*_1st_ShadeMap_var.rgb)*Set_LightColor), _Is_LightColor_1st_Shade);
	//�ڶ����� ������ɫ*�ڶ�����ɫ
	float4 _2nd_ShadeMap_var = lerp(tex2D(_2nd_ShadeMap, TRANSFORM_TEX(Set_UV0, _2nd_ShadeMap)), _1st_ShadeMap_var, _Use_1stAs2nd);
	float3 Set_2nd_ShadeColor = lerp((_2nd_ShadeColor.rgb*_2nd_ShadeMap_var.rgb), ((_2nd_ShadeColor.rgb*_2nd_ShadeMap_var.rgb)*Set_LightColor), _Is_LightColor_2nd_Shade);

	//�������� ���ռ��������߿ռ���
	float _HalfLambert_var = 0.5*dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase), lightDirection) + 0.5;
	//����Ӱλ��
	float4 _Set_2nd_ShadePosition_var = tex2D(_Set_2nd_ShadePosition, TRANSFORM_TEX(Set_UV0, _Set_2nd_ShadePosition));
	float4 _Set_1st_ShadePosition_var = tex2D(_Set_1st_ShadePosition, TRANSFORM_TEX(Set_UV0, _Set_1st_ShadePosition));

	//��������Ӱ
	//Minmimum value is same as the Minimum Feather's value with the Minimum Step's value as threshold.
	//��Сֵ��Minimum Feather��ֵ��ͬ����Minimum Step��ֵΪ��ֵ��
	//����Ӱ�ȼ� 
	float _SystemShadowsLevel_var = (attenuation*0.5) + 0.5 + _Tweak_SystemShadowsLevel > 0.001 ? (attenuation*0.5) + 0.5 + _Tweak_SystemShadowsLevel : 0.0001;
	//�����Ӱ
	float Set_FinalShadowMask =
		saturate((1.0 +
		(
			(
				lerp(_HalfLambert_var,
				_HalfLambert_var*saturate(_SystemShadowsLevel_var),
				_Set_SystemShadowsToBase
				)//����_Set_SystemShadowsToBaseΪ0 ��ô����_HalfLambert_var �������� ����ģ��
				-
				(_BaseColor_Step - _BaseShade_Feather)
			)
			*
			((1.0 - _Set_1st_ShadePosition_var.rgb).r - 1.0)//_Set_1st_ShadePosition_var ����û��_Set_1st_ShadePosition ֵΪ0
		)
			/
			(_BaseColor_Step - (_BaseColor_Step - _BaseShade_Feather))
			));

	//����ԭɫ�������
	float3 Set_FinalBaseColor = 
		lerp(
			Set_BaseColor, 
			lerp(
				//��˫��ͼ���кϲ�
				Set_1st_ShadeColor, 
				Set_2nd_ShadeColor, 
				saturate((1.0 + 
							((_HalfLambert_var - (_ShadeColor_Step - _1st2nd_Shades_Feather)) * ((1.0 - _Set_2nd_ShadePosition_var.rgb).r - 1.0)) 
							/ 
							(_ShadeColor_Step - (_ShadeColor_Step - _1st2nd_Shades_Feather))
						))
				), 
			Set_FinalShadowMask); // Final Color
//********************������ɫ*********************



//********************�߹�*********************
	//�߹�����
	float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask, TRANSFORM_TEX(Set_UV0, _Set_HighColorMask));
	//�߹ⷴ��
	//�߹� ����ģ��
	//���ռ��������߿ռ���
	float _Specular_var = 0.5*dot(halfDirection, lerp(i.normalDir, normalDirection, _Is_NormalMapToHighColor)) + 0.5; //  Specular                
	//����ǿ��
	float _TweakHighColorMask_var = (saturate((_Set_HighColorMask_var.g + _Tweak_HighColorMaskLevel))*lerp((1.0 - step(_Specular_var, (1.0 - pow(_HighColor_Power, 5)))), pow(_Specular_var, exp2(lerp(11, 1, _HighColor_Power))), _Is_SpecularToHighColor));
	//�߹���ͼ
	//����ɫ
	float4 _HighColor_Tex_var = tex2D(_HighColor_Tex, TRANSFORM_TEX(Set_UV0, _HighColor_Tex));
	//����ɫ*����ɫ*����ɫ*����ɫ
	float3 _HighColor_var = (
		lerp(
			(_HighColor_Tex_var.rgb*_HighColor.rgb), 
			((_HighColor_Tex_var.rgb*_HighColor.rgb)*Set_LightColor), 
			_Is_LightColor_HighColor
			)
			*
			_TweakHighColorMask_var);

	//Composition: 3 Basic Colors and HighColor as Set_HighColor
	//���:3��������ɫ��HighColor��ΪSet_HighColor
	//�Ѹ߹���Ͻ�ȥ
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

//********************�߹�*********************
	




//********************��Ե��*********************
	//������Ե��Mask(����еĻ�)
	float4 _Set_RimLightMask_var = tex2D(_Set_RimLightMask, TRANSFORM_TEX(Set_UV0, _Set_RimLightMask));

	//�����Ե����������պϳ� A1
	float3 _Is_LightColor_RimLight_var =
		lerp(
			_RimLightColor.rgb,
			(_RimLightColor.rgb*Set_LightColor),
			_Is_LightColor_RimLight
		);

	//��Ե�ⷶΧ
	float _RimArea_var = (1.0 - dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToRimLight), viewDirection));

	//��Ե��ǿ��
	float _RimLightPower_var = pow(_RimArea_var, exp2(lerp(3, 0, _RimLight_Power)));

	//����_RimLight_FeatherOff�����������Ƿ���Ҫ_RimLight_InsideMask������ �õ�Mask�����ý�� A2
	float _Rimlight_InsideMask_var =
		saturate(
			lerp(
			(0.0 + ((_RimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)),
				step(_RimLight_InsideMask, _RimLightPower_var),
				_RimLight_FeatherOff)
		);


	

	//��������
	float _VertHalfLambert_var = 0.5*dot(i.normalDir, lightDirection) + 0.5;

	//A1��A2�ϳ�ǰ���
	float3 _LightDirection_MaskOn_var =
		lerp(
		(_Is_LightColor_RimLight_var*_Rimlight_InsideMask_var),
			(_Is_LightColor_RimLight_var*saturate(
			(_Rimlight_InsideMask_var - ((1.0 - _VertHalfLambert_var) + _Tweak_LightDirection_MaskLevel))
			)),
			_LightDirection_MaskOn);



	//������ǰ��� �����Ǳ���� ԭ����ͬ
	//������ ��Ե��
	float _ApRimLightPower_var = pow(_RimArea_var, exp2(lerp(3, 0, _Ap_RimLight_Power)));
	float3 Set_RimLight = (
		saturate((_Set_RimLightMask_var.g + _Tweak_RimLightMaskLevel))*//�����Mask��ô�ͳ����������
		lerp(_LightDirection_MaskOn_var,
		(_LightDirection_MaskOn_var + (//�����Ǳ���� ǰ��������_LightDirection_MaskOn_var
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
	//��ɣ�HighColor and RimLight as _RimLight_var
	//�ѱ�Ե����Ͻ�ȥ
	float3 _RimLight_var = lerp(Set_HighColor, (Set_HighColor + Set_RimLight), _RimLight);
//********************��Ե��*********************




	fixed4 finalRGBA = fixed4(_RimLight_var, 0);
	return finalRGBA;
}