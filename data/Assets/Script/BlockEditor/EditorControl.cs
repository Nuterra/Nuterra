using UnityEditor;
using UnityEngine;

public class EditorControl : EditorWindow
{
	private bool groupEnabled;
	private bool myBool;
	private float myFloat;
	private string myString;

	[MenuItem("Nuterra/Editor")]

	private static void ShowWindow()
	{
		EditorWindow.GetWindow<EditorControl>();
	}
	private void OnGUI()
	{
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField("Text Field", myString);

		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		if (groupEnabled)
		{
			myBool = EditorGUILayout.Toggle("Toggle", myBool);
			myFloat = EditorGUILayout.Slider("Slider", myFloat, -3f, 3f);
		}
		EditorGUILayout.EndToggleGroup();
	}

	private void OnSceneGUI()
	{
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
		{
			Debug.Log("Left-Mouse Down");
		}
		if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
		{
			Debug.Log("Left-Mouse Up");
		}
	}
}