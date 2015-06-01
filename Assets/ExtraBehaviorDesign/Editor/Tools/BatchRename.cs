using UnityEngine;
using System.Collections;
using UnityEditor;

public class BatchRename : ScriptableWizard {

	// Base Name
    public string m_baseName = "MyObject_";

    // Start Count
    public int m_startNumber = 0;

    // Increment
    public int m_increment = 1;

    [MenuItem("Edit/Batch Rename...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Batch Rename", typeof(BatchRename), "Rename");
    }

    // Called when the window first appears
    void OnEnable()
    {
        UpdateSelectionHelper();
    }

    // Function called when selection changes in scene
    void OnSelectionChange()
    {
        UpdateSelectionHelper();
    }

    // Update selection counter
    void UpdateSelectionHelper()
    {
        helpString = "";

        // 这些objects都是scene视图中存在的
        if (null != Selection.objects)
	    {
            helpString = "Number of objects selected: " + Selection.objects.Length;
	    }
    }

    // Rename，点击生成wizard按钮的时候被调用
    void OnWizardCreate()
    {
        // If selection empty, then exit
        if (null == Selection.objects)
	    {
                return;
	    }

        // Current Increment
        int PostFix = m_startNumber;

        // Cycle and  rename
        for (int i = 0; i < Selection.objects.Length; i++)
		{
            Object obj = Selection.objects[i];
            obj.name = m_baseName + PostFix;
            PostFix += m_increment;
		}
    }
}
