using UnityEditor;
using UnityEngine;

public class EditorTest : ShaderGUI
{
    public enum _OutlineMode
    {
        NormalDirection,
        PositionScaling
    }

    public enum _CullingMode
    {
        CullingOff,
        FrontCulling,
        BackCulling
    }

    public enum _EmissiveMode
    {
        SimpleEmissive,
        EmissiveAnimation
    }

    public _OutlineMode outlineMode;
    public _CullingMode cullingMode;
    public _EmissiveMode emissiveMode;

    public GUILayoutOption[] shortButtonStyle = new GUILayoutOption[] {GUILayout.Width(130)};
    public GUILayoutOption[] middleButtonStyle = new GUILayoutOption[] {GUILayout.Width(130)};

    //查找属性
    MaterialProperty clippingMask = null;
    MaterialProperty mainTex = null;
    MaterialProperty clipping_Level = null;
    MaterialProperty tweak_transparency = null;
    
    
    static int _StencilNo_Setting;
    static bool _OriginalInspector = false;
    static bool _SimpleUI = true;

    static bool _BasicShaderSettings_Foldout = false;
    static bool _BasicThreeColors_Foldout = true;

    MaterialEditor m_MaterialEditor;

    public void FindProperties(MaterialProperty[] props)
    {
        clippingMask = FindProperty("_ClippingMask", props, false);
        clipping_Level = FindProperty("_Clipping_Level", props, false);
        mainTex = FindProperty("_MainTex", props);
        tweak_transparency = FindProperty("_Tweak_transparency", props, false);
    }

    static bool Foldout(bool display, string title)
    {
        var style = new GUIStyle("ModuleTitle");
        style.font = new GUIStyle(EditorStyles.boldLabel).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);

        var rect = GUILayoutUtility.GetRect(16f, 22f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }
    private static class Styles
    {
        public static GUIContent baseColorText = new GUIContent("测试1", "凑数1 卡精神科放大镜看法");
        public static GUIContent firstShadeColorText = new GUIContent("测试1", "凑数2 卡精神科放大镜看法");
        public static GUIContent clippingMaskText = new GUIContent("Clipping Mask","Clipping Mask : Texture(linear)");
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        EditorGUIUtility.fieldWidth = 0;
        FindProperties(props);
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;


        EditorGUILayout.BeginHorizontal();
        
        //Original Inspectorの選択チェック.
        if (material.HasProperty("_simpleUI"))
        {
            var selectedUI = material.GetInt("_simpleUI");
            Debug.Log(selectedUI);
            Debug.Log(_OriginalInspector);
            if (selectedUI == 2)
            {
                _OriginalInspector = true; //Original GUI
            }
            else if (selectedUI == 1)
            {
                _SimpleUI = true; //UTS2 Biginner GUI
            }

            //Original/Custom GUI 切り替えボタン.
            if (_OriginalInspector)
            {
                if (GUILayout.Button("Change CustomUI", middleButtonStyle))
                {
                    _OriginalInspector = false;
                    material.SetInt("_simpleUI", 0); //UTS2 Pro GUI
                }

                //OpenManualLink(); 打开手册
                //継承したレイアウトのクリア.
                EditorGUILayout.EndHorizontal();
                //オリジナルのGUI表示
                m_MaterialEditor.PropertiesDefaultGUI(props);
                return;
            }

            if (GUILayout.Button("Show All properties", middleButtonStyle))
            {
                _OriginalInspector = true;
                material.SetInt("_simpleUI", 2); //Original GUI
            }
        }

        //マニュアルを開く.
        //OpenManualLink(); 打开手册
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.Space();

        _BasicShaderSettings_Foldout = Foldout(_BasicShaderSettings_Foldout, "Basic Shader Settings");
        if (_BasicShaderSettings_Foldout)
        {
            EditorGUI.indentLevel++;
            
            GUI_SetCullingMode(material);

            if (material.HasProperty("_StencilNo"))
            {
                GUI_SetStencilNo(material);
            }

            if (material.HasProperty("_ClippingMask"))
            {
                GUI_SetClippingMask(material);
            }

            if (material.HasProperty("_Tweak_transparency"))
            {
                GUI_SetTransparencySetting(material);
            }

            GUI_OptionMenu(material);

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        _BasicThreeColors_Foldout = Foldout(_BasicThreeColors_Foldout, "Basic Three Colors and Control Maps Setups】");
        if (_BasicThreeColors_Foldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("xzczcccccccccccccccccccccccccccccccccccccccccccccc", EditorStyles.label);
            GUI_SetCullingMode(material);
            EditorGUI.indentLevel--;
        }
    }

    void GUI_OptionMenu(Material material)
        {
            GUILayout.Label("Option Menu", EditorStyles.boldLabel);
            
            if(material.HasProperty("_simpleUI")){
                if(material.GetInt("_simpleUI") == 1){
                    _SimpleUI = true; //UTS2 Custom GUI Biginner
                }
                else{
                    _SimpleUI = false; //UTS2 Custom GUI Pro
                }
            }

            EditorGUILayout.BeginHorizontal();
            
                EditorGUILayout.PrefixLabel("Current UI Type");
                if(_SimpleUI == false) {
                    if (GUILayout.Button("Pro / Full Control",middleButtonStyle))
                    {
                        material.SetInt("_simpleUI",1); //UTS2 Custom GUI Biginner
                    }
                }else{
                    if (GUILayout.Button("Biginner",middleButtonStyle))
                    {
                        material.SetInt("_simpleUI",0); //UTS2 Custom GUI Pro
                    }
                }
                
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("VRChat Recommendation");
                if (GUILayout.Button("Apply Settings",middleButtonStyle))
                {
                    GUILayout.Label("Apply Settings", EditorStyles.label);
                }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Remove Unused Keywords/Properties from Material");
                if (GUILayout.Button("Execute",middleButtonStyle))
                {
                    GUILayout.Label("Execute", EditorStyles.label);
                }
            EditorGUILayout.EndHorizontal();
        }
    
    void GUI_SetCullingMode(Material material)
    {
        int _CullMode_Setting = material.GetInt("_CullMode");
        //Enum形式に変換して、outlineMode変数に保持しておく.
        if ((int) _CullingMode.CullingOff == _CullMode_Setting)
        {
            cullingMode = _CullingMode.CullingOff;
        }
        else if ((int) _CullingMode.FrontCulling == _CullMode_Setting)
        {
            cullingMode = _CullingMode.FrontCulling;
        }
        else
        {
            cullingMode = _CullingMode.BackCulling;
        }

        //EnumPopupでGUI記述.
        cullingMode = (_CullingMode) EditorGUILayout.EnumPopup("Culling Mode", cullingMode);
        //値が変化したらマテリアルに書き込み.
        if (cullingMode == _CullingMode.CullingOff)
        {
            material.SetFloat("_CullMode", 0);
        }
        else if (cullingMode == _CullingMode.FrontCulling)
        {
            material.SetFloat("_CullMode", 1);
        }
        else
        {
            material.SetFloat("_CullMode", 2);
        }
    }

    void GUI_SetStencilNo(Material material)
    {
        GUILayout.Label("For _StencilMask or _StencilOut Shader", EditorStyles.boldLabel);
        _StencilNo_Setting = material.GetInt("_StencilNo");
        int _Current_StencilNo = _StencilNo_Setting;
        _Current_StencilNo = (int) EditorGUILayout.IntField("Stencil No.", _Current_StencilNo);
        if (_StencilNo_Setting != _Current_StencilNo)
        {
            material.SetInt("_StencilNo", _Current_StencilNo);
        }
    }

    void GUI_SetClippingMask(Material material)
    {
        GUILayout.Label("For _Clipping or _TransClipping Shader", EditorStyles.boldLabel);
        m_MaterialEditor.TexturePropertySingleLine(Styles.clippingMaskText, clippingMask);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Inverse Clipping Mask");
        //GUILayout.Space(60);
        if (material.GetFloat("_Inverse_Clipping") == 0)
        {
            if (GUILayout.Button("Off", shortButtonStyle))
            {
                material.SetFloat("_Inverse_Clipping", 1);
            }
        }
        else
        {
            if (GUILayout.Button("Active", shortButtonStyle))
            {
                material.SetFloat("_Inverse_Clipping", 0);
            }
        }

        EditorGUILayout.EndHorizontal();

        m_MaterialEditor.RangeProperty(clipping_Level, "Clipping Level");
    }

    void GUI_SetTransparencySetting(Material material)
    {
        GUILayout.Label("For _TransClipping Shader", EditorStyles.boldLabel);
        m_MaterialEditor.RangeProperty(tweak_transparency, "Transparency Level");

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Use BaseMap α as Clipping Mask");
            //GUILayout.Space(60);
            if (material.GetFloat("_IsBaseMapAlphaAsClippingMask") == 0)
            {
                if (GUILayout.Button("Off", shortButtonStyle))
                {
                    material.SetFloat("_IsBaseMapAlphaAsClippingMask", 1);
                }
            }
            else
            {
                if (GUILayout.Button("Active", shortButtonStyle))
                {
                    material.SetFloat("_IsBaseMapAlphaAsClippingMask", 0);
                }
            }

        EditorGUILayout.EndHorizontal();
    }
    
    
    
    
}