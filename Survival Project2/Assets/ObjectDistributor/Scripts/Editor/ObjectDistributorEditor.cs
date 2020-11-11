// Copyright 2018 Nick Alves - http://www.nickalves.com
// Object Distributor Editor v2018.3.18

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectDistributor))]
public class ObjectDistributorEditor : Editor
{

    private ObjectDistributor m_target;
    GUIStyle m_foldoutStyle;

    private bool m_bShowPlacement;
    private static string m_placementString = "Global Placement";
    private static string m_projectString = "Project Objects from ";
    private static string[] m_eAxisChoices = { "X", "Y", "Z" };
    private static string[] m_eDirectionChoices = { "+", "-" };
    private static string m_placementInSceneString = "Show Indicator in Scene ";
    private static string m_randomRotationString = "Randomly Rotate Axis ";
    private static string m_placementMethodString = "Placement Method ";
    private static string[] m_eMethodChoices = { "True Random", "Avoid Edges", "Brightness Texture Mask" };
    private static string m_PlacementTriesString = "Placement Tries ";
    private static string m_avoidEdgeString = "Avoidance Percentage ";
    private static string m_brightnessString = "Brightness Threshold ";
    private static string m_invertMaskString = "Invert Mask ";

    private SerializedProperty m_objectList;

    private void OnEnable()
    {
        m_target = (ObjectDistributor)target;
        m_bShowPlacement = true;
        m_objectList = serializedObject.FindProperty("m_objects");
    }
    private void Awake()
    {
        m_foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };
    }
    public override void OnInspectorGUI()
    {
        m_bShowPlacement = EditorGUILayout.Foldout(m_bShowPlacement, m_placementString, true, m_foldoutStyle);
        if (m_bShowPlacement)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_projectString, GUILayout.Width(EditorGUIUtility.labelWidth));
            switch (m_target.m_projectionAxis)
            {
                case 0:
                    GUI.backgroundColor = Handles.xAxisColor;
                    break;
                case 1:
                    GUI.backgroundColor = Handles.yAxisColor;
                    break;
                case 2:
                    GUI.backgroundColor = Handles.zAxisColor;
                    break;
                default:
                    break;
            }
            m_target.m_projectionAxis = EditorGUILayout.Popup(m_target.m_projectionAxis, m_eAxisChoices);
            GUI.backgroundColor = Color.white;
            m_target.m_bProjectionDirection = EditorGUILayout.Popup(m_target.m_bProjectionDirection ? 1 : 0, m_eDirectionChoices) == 1 ? true : false;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_placementInSceneString, GUILayout.Width(EditorGUIUtility.labelWidth));
            m_target.m_bPreviewInScene = EditorGUILayout.Toggle(m_target.m_bPreviewInScene);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_randomRotationString, GUILayout.Width(EditorGUIUtility.labelWidth));
            m_target.m_bUseRandomXRotation = GUILayout.Toggle(m_target.m_bUseRandomXRotation, "X", GUILayout.Width(25));
            m_target.m_bUseRandomYRotation = GUILayout.Toggle(m_target.m_bUseRandomYRotation, "Y", GUILayout.Width(25));
            m_target.m_bUseRandomZRotation = GUILayout.Toggle(m_target.m_bUseRandomZRotation, "Z", GUILayout.Width(25));
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            GUI.enabled = m_target.m_bUseRandomXRotation;
            m_target.m_randomXRotationRange = EditorGUILayout.Slider("X Range", m_target.m_randomXRotationRange, 0, 180);
            GUI.enabled = m_target.m_bUseRandomYRotation;
            m_target.m_randomYRotationRange = EditorGUILayout.Slider("Y Range", m_target.m_randomYRotationRange, 0, 180);
            GUI.enabled = m_target.m_bUseRandomZRotation;
            m_target.m_randomZRotationRange = EditorGUILayout.Slider("Z Range", m_target.m_randomZRotationRange, 0, 180);
            GUI.enabled = true;
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_placementMethodString, GUILayout.Width(EditorGUIUtility.labelWidth));
            m_target.m_projectionMethod = EditorGUILayout.Popup(m_target.m_projectionMethod, m_eMethodChoices);
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_PlacementTriesString, GUILayout.Width(EditorGUIUtility.labelWidth));
            m_target.m_placementTries = EditorGUILayout.IntSlider(m_target.m_placementTries, 1, 16);
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            switch (m_target.m_projectionMethod)
            {
                case 1:
                    EditorGUI.indentLevel++;
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(m_avoidEdgeString, GUILayout.Width(EditorGUIUtility.labelWidth));
                    m_target.m_edgeAvoidance = EditorGUILayout.Slider(m_target.m_edgeAvoidance, 0, 100);
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                    break;
                case 2:
                    EditorGUI.indentLevel++;
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(m_brightnessString, GUILayout.Width(EditorGUIUtility.labelWidth));
                    m_target.m_brightnessThreshold = EditorGUILayout.Slider(m_target.m_brightnessThreshold, 0, 1);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(m_invertMaskString, GUILayout.Width(EditorGUIUtility.labelWidth));
                    m_target.m_bInvertMask = EditorGUILayout.Toggle(m_target.m_bInvertMask);
                    GUILayout.EndHorizontal();
                    m_target.m_sampleImage = (Texture2D)EditorGUILayout.ObjectField("Image", m_target.m_sampleImage, typeof(Texture2D), false);
                    EditorGUI.indentLevel--;
                    break;
                default:
                    break;
            }
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(m_objectList, true);
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(20);

        if (GUILayout.Button("Distribute Objects"))
        {
            m_target.Execute();
        }
        if (m_target.m_bufferObject)
        {
            GUILayout.BeginHorizontal();
            GUI.color = Handles.yAxisColor;
            if (GUILayout.Button("  Save  "))
            {
                m_target.Save();
            }
            GUI.color = Handles.xAxisColor;
            if (GUILayout.Button("Remove"))
            {
                m_target.CleanUp();
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
        }
    }
    private void OnSceneGUI()
    {
        if (m_target.m_bPreviewInScene)
        {
            Vector3 cubeCenter = m_target.GetBounds().center;
            Vector3 cubeExtend = m_target.GetBounds().extents;
            Vector3 conePos = cubeCenter;
            Quaternion coneRot = Quaternion.identity;
            float coneSize = cubeExtend.magnitude / 2;

            switch (m_target.m_projectionAxis)
            {
                case 0:
                    Handles.color = Handles.xAxisColor;
                    coneRot = Quaternion.LookRotation(m_target.m_bProjectionDirection ? Vector3.right : Vector3.left);
                    conePos.x += m_target.m_bProjectionDirection ? -(coneSize * 2) : (coneSize * 2);
                    break;
                case 1:
                    Handles.color = Handles.yAxisColor;
                    coneRot = Quaternion.LookRotation(m_target.m_bProjectionDirection ? Vector3.up : Vector3.down);
                    conePos.y += m_target.m_bProjectionDirection ? -(coneSize * 2) : (coneSize * 2);
                    break;
                case 2:
                    Handles.color = Handles.zAxisColor;
                    coneRot = Quaternion.LookRotation(m_target.m_bProjectionDirection ? Vector3.forward : Vector3.back);
                    conePos.z += m_target.m_bProjectionDirection ? -(coneSize * 2) : (coneSize * 2);
                    break;
                default:
                    break;
            }

            Handles.ConeHandleCap(0, conePos, coneRot, coneSize, EventType.Repaint);
            Handles.DrawWireCube(cubeCenter, cubeExtend * 2);
        }
    }
}
