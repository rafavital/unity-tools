#if UNITY_EDITOR

//---------------------------------------------------
// <copyright file="PhysicalSimulatorWindow.cs" company="Rafael Vital Lacerda Alves"
//       Author: Rafael Vital Lacerda Alves
//       Copyright (c) 2021 Rafael Vital Lacerda Alves
// </copyright>
//---------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class PhysicalSimulatorWindow : EditorWindow
{
    public List<GameObject> objectsToSimulate;

    private SerializedObject so;
    private SerializedProperty propObjectsToSimulate;

    private bool isRunning;
    private bool runnedAtLeastOnce;
    private float lastSimulationTime;
    private Queue<GameObject> objectsWithoutRb;
    private Queue<GameObject> objectsWithoutMeshCol;
    private List<Vector3> prevPositions;
    private List<Quaternion> prevRotations;
    private List<Vector3> prevScales;


    [MenuItem("Automation/Physical Simulator")]
    public static void OpenWindow() => EditorWindow.GetWindow<PhysicalSimulatorWindow>();

    private void OnEnable()
    {
        runnedAtLeastOnce = false;
        so = new SerializedObject(this);
        propObjectsToSimulate = so.FindProperty("objectsToSimulate");

        if (!Application.isPlaying)
            Physics.autoSimulation = false;

        objectsToSimulate = new List<GameObject>();

        objectsWithoutMeshCol = new Queue<GameObject>();
        objectsWithoutRb = new Queue<GameObject>();
    }

    private void OnGUI()
    {
        EditorGUILayout.PropertyField(propObjectsToSimulate);
        // using (new EditorGUI.DisabledScope(Selection.count == 0))
        // {
        //     if (GUILayout.Button("Add Selection"))
        //     {
        //         foreach (var obj in Selection.gameObjects)
        //         {
        //             objectsToSimulate.Add(obj);
        //         }
        //     }
        // }

        using (new EditorGUILayout.HorizontalScope())
        {
            using (new EditorGUI.DisabledScope(isRunning))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_PlayButton@2x")))
                    StartSimulation();
            }

            using (new EditorGUI.DisabledScope(!isRunning))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_PreMatQuad@2x")))
                    StopSimulation();
            }
        }

        EditorGUILayout.LabelField("Simulation State: ", isRunning ? "Active" : "Inactive");

        EditorGUILayout.Space(25);

        using (new EditorGUI.DisabledScope(!runnedAtLeastOnce || prevPositions.Count == 0))
        {
            if (GUILayout.Button("Undo"))
            {
                Undo();
            }
        }
    }

    private void Update()
    {
        if (!isRunning || Application.isPlaying)
            return;

        if (EditorApplication.timeSinceStartup > lastSimulationTime + Time.fixedDeltaTime)
        {
            Simulate();
        }
    }

    private void StartSimulation()
    {
        // checks if objects need rigidbody and mesh colliders to be added
        foreach (var obj in objectsToSimulate)
        {
            if (obj.GetComponent<Rigidbody>() == null)
            {
                objectsWithoutRb.Enqueue(obj);
                //Debug.Log("Queued");
                obj.AddComponent<Rigidbody>();
            }


            foreach (var child in obj.GetComponentsInChildren<Transform>())
            {
                var childGO = child.gameObject;
                if (childGO.GetComponent<MeshFilter>())
                    if (!childGO.GetComponent<MeshCollider>())
                    {
                        objectsWithoutMeshCol.Enqueue(childGO);
                        var mc = childGO.AddComponent<MeshCollider>();
                        mc.convex = true;
                    }
            }
        }

        // saves the previous positions in case the user wants to undo

        prevPositions = new List<Vector3>();
        prevRotations = new List<Quaternion>();
        prevScales = new List<Vector3>();

        for (int i = 0; i < objectsToSimulate.Count; i++)
            prevPositions.Add(objectsToSimulate[i].transform.position);
        
        for (int i = 0; i < objectsToSimulate.Count; i++)
            prevRotations.Add(objectsToSimulate[i].transform.rotation);

        for (int i = 0; i < objectsToSimulate.Count; i++)
            prevScales.Add(objectsToSimulate[i].transform.localScale);
        
        // prevPositions = (List<Vector3>)objectsToSimulate.Select(t => t.transform.position);
        // prevRotations = (List<Quaternion>)objectsToSimulate.Select(t => t.transform.rotation);
        // prevScales = (List<Vector3>)objectsToSimulate.Select(t => t.transform.localScale);

        isRunning = true;
    }

    private void Simulate()
    {
        Debug.Log("Simulating");
        Physics.Simulate(Time.fixedDeltaTime);
        lastSimulationTime = (float)EditorApplication.timeSinceStartup;
    }

    private void StopSimulation()
    {
        isRunning = false;

        //Debug.Log(objectsWithoutRb.Count);
        // cleanup rigidbodies and mesh colliders that were added
        if (objectsWithoutRb.Count > 0)
        {
            while (objectsWithoutRb.Count > 0)
            {
                DestroyImmediate(objectsWithoutRb.Dequeue().GetComponent<Rigidbody>());
            }
        }

        if (objectsWithoutMeshCol.Count > 0)
        {
            while (objectsWithoutMeshCol.Count > 0)
            {
                DestroyImmediate(objectsWithoutMeshCol.Dequeue().GetComponent<MeshCollider>());
            }
        }

        runnedAtLeastOnce = true;
    }

    private void Undo()
    {
        for (int i = 0; i < objectsToSimulate.Count; i++)
        {
            var trans = objectsToSimulate[i].transform;

            trans.position = prevPositions[i];
            trans.rotation = prevRotations[i];
            trans.localScale = prevScales[i];
        }
    }
}

#endif