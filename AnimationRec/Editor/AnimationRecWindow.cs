#if UNITY_EDITOR

//---------------------------------------------------
// <copyright file="AnimationRecWindow.cs" company="Rafael Vital Lacerda Alves"
//       Author: Rafael Vital Lacerda Alves
//       Copyright (c) 2021 Rafael Vital Lacerda Alves
// </copyright>
//---------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class AnimationRecWindow : EditorWindow
{
    public string savePath;
    public string clipName;
    public float recordFrequency = 0.25f;
    public TransformRecordObject transformRecorder;
    public KeyCode startRecordKey = KeyCode.I;
    public KeyCode stopRecordKey = KeyCode.O;
    public KeyCode deleteRecordKey = KeyCode.P;

    public SerializedObject so;
    public SerializedProperty propSavePath;
    public SerializedProperty propClipName;
    public SerializedProperty propRecordFrequency;

    private bool isRecording = false;
    private bool deletedRecording = false;
    private double lastFrameTime = 0.0f;
    private double startedRecordingTime = 0.0f;
    private Transform selectedTransform;

    [MenuItem("Tools/Animation Rec")]
    public static void OpenWindow()
    {
        var window = EditorWindow.GetWindow<AnimationRecWindow>();
        window.Show();
    }

    private void OnEnable()
    {
        //if (string.IsNullOrEmpty(savePath))
        GetDefaultSavePath();

        so = new SerializedObject(this);

        propSavePath = so.FindProperty("savePath");
        propClipName = so.FindProperty("clipName");
        propRecordFrequency = so.FindProperty("recordFrequency");

        isRecording = false;
    }

    private void OnGUI()
    {
        so.Update();

        selectedTransform = (Transform)EditorGUILayout.ObjectField("Object to Animate", selectedTransform, typeof(Transform), true);

        EditorGUILayout.Space(15);

        #region Recording Keys
        using (new EditorGUILayout.HorizontalScope())
        {
            //EditorGUILayout.LabelField("Start Recording Key");
            startRecordKey = (KeyCode)EditorGUILayout.EnumPopup("Start Recording Key:", startRecordKey);
            startRecordKey = InputControls.KeyCodeField(EditorGUILayout.GetControlRect(), startRecordKey);
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            //EditorGUILayout.LabelField("Stop Recording Key");
            stopRecordKey = (KeyCode)EditorGUILayout.EnumPopup("Stop Recording Key:", stopRecordKey);
            stopRecordKey = InputControls.KeyCodeField(EditorGUILayout.GetControlRect(), stopRecordKey);
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            //EditorGUILayout.LabelField("Stop Recording Key");
            deleteRecordKey = (KeyCode)EditorGUILayout.EnumPopup("Delete Recording Key:", deleteRecordKey);
            deleteRecordKey = InputControls.KeyCodeField(EditorGUILayout.GetControlRect(), deleteRecordKey);
        }
        #endregion

        EditorGUILayout.PropertyField(propRecordFrequency);
        recordFrequency = Mathf.Max(0.01f, recordFrequency);

        EditorGUILayout.PropertyField(propClipName);
        CheckClipName();

        using (new EditorGUILayout.HorizontalScope())
        {
            //EditorGUILayout.PropertyField(propSavePath);
            EditorGUILayout.LabelField("Save Path", savePath);
            if (GUILayout.Button("Select"))
            {
                savePath = EditorUtility.OpenFolderPanel("Destination Folder", savePath, "");
            }
        }

        if (string.IsNullOrEmpty(savePath))
            GetDefaultSavePath();

        EditorGUILayout.Space(20);
        EditorGUILayout.HelpBox("Current State: " + (isRecording ? "Recording" : "Not Recording"), MessageType.None, true);
        EditorGUILayout.Space(20);

        if (GUILayout.Button(isRecording ? "Stop Recording" : "Start Recording"))
        {
            if (isRecording)
                StopRecording();
            else
                StartRecording();
        }

        so.ApplyModifiedProperties();
    }

    private void Update()
    {
        var currentEvent = Event.current;

        if (currentEvent == null)
            return;

        if (currentEvent.isKey)
        {
            // returns if the user is typing in at a text field
            if (EditorGUIUtility.editingTextField)
                return;

            if (currentEvent.keyCode == startRecordKey)
            {
                StartRecording();
            }
            else if (currentEvent.keyCode == stopRecordKey)
            {
                StopRecording();
            }
            else if (currentEvent.keyCode == deleteRecordKey)
            {
                deletedRecording = true;
                StopRecording();
            }

            // consume the event so it doesn't activate other functionalities
            currentEvent.Use();
        }

        if (isRecording)
        {
            if (EditorApplication.timeSinceStartup > lastFrameTime + recordFrequency)
                RecordFrame();
        }

    }

    public void StartRecording()
    {
        if (isRecording || string.IsNullOrEmpty(clipName) || string.IsNullOrEmpty(savePath) || selectedTransform == null)
            return;

        //selectedTransform = Selection.activeTransform;
        if (selectedTransform == null)
            return;

        Debug.Log("começou");

        startedRecordingTime = EditorApplication.timeSinceStartup;
        transformRecorder = new TransformRecordObject(selectedTransform.name, selectedTransform);

        isRecording = true;

    }

    public void StopRecording()
    {
        if (!isRecording)
            return;

        if (deletedRecording)
        {
            deletedRecording = false;
            EditorUtility.DisplayDialog("Cancel Recording", "Recording deleted", "Continue", "");
            return;
        }
        Debug.Log("parou");
        isRecording = false;
        ExportAnimation();
    }

    public void RecordFrame()
    {
        //Debug.Log(EditorApplication.timeSinceStartup - lastFrameTime);
        transformRecorder.AddFrame(System.Convert.ToSingle(EditorApplication.timeSinceStartup - startedRecordingTime));

        lastFrameTime = EditorApplication.timeSinceStartup;
    }

    public void ExportAnimation()
    {
        string exportFilePath = savePath + clipName + ".anim";

        exportFilePath = AssetDatabase.GenerateUniqueAssetPath(exportFilePath);

        AnimationClip clip = new AnimationClip();
        clip.name = clipName;

        var allCurves = transformRecorder.curves;

        for (int i = 0; i < allCurves.Length; i++)
        {
            clip.SetCurve("", typeof(Transform), allCurves[i].property, allCurves[i].curve);
        }

        clip.EnsureQuaternionContinuity();
        AssetDatabase.CreateAsset(clip, exportFilePath);
    }

    private void GetDefaultSavePath() => savePath = Path.GetDirectoryName(SceneManager.GetActiveScene().path) + "/";

    private void CheckClipName()
    {
        if (selectedTransform == null)
            return;

        if (string.IsNullOrEmpty(clipName))
        {
            clipName = selectedTransform.name + "_Animation";
        }

    }

}
#endif