using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class HealthBarAttributeDrawer : OdinAttributeDrawer<HealthBarAttribute, float>
{

    protected override void DrawPropertyLayout(GUIContent label)
    {
        // Call the next drawer, which will draw the float field.
        this.CallNextDrawer(label);

        // Get a rect to draw the health-bar on.
        Rect rect = EditorGUILayout.GetControlRect();

        // Draw the health bar using the rect.
        float width = Mathf.Clamp01(this.ValueEntry.SmartValue / this.Attribute.MaxHealth);
        SirenixEditorGUI.DrawSolidRect(rect, new Color(0f, 0f, 0f, 0.3f), false);
        SirenixEditorGUI.DrawSolidRect(rect.SetWidth(rect.width * width), Color.red, false);
        SirenixEditorGUI.DrawBorders(rect, 1);
    }


}

