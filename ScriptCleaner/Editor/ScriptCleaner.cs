using UnityEngine;
using UnityEditor;

public class ScriptCleaner : MonoBehaviour
{
    [MenuItem("GameObject/Automation/Remove Scripts", false, 49)]
    public static void CleanScripts()
    {
        var obj = Selection.activeGameObject;
        if (EditorUtility.DisplayDialog("Remove Scripts",$"Are you sure you want to remove all scripts from {obj.name} and it's children?", "Yes", "No"))
        {
            foreach (var t in obj.GetComponentsInChildren<Transform>())
            {

                //Checks if the current component is the transform component
                

                //Removes all components from the object
                foreach (var c in t.GetComponents<Component>())
                {
                    if (c.GetType() == typeof(Transform))
                        continue;
                    DestroyImmediate(c);
                }
            }
        }

        EditorUtility.DisplayDialog("Scripts Removal Complete",$"All scripts form {obj.name} and it's children were removed successfully", "Continue", "");
    }

}