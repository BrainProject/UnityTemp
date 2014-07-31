using UnityEngine;
using System.Collections;

public class GuiWindowScript : MonoBehaviour 
{
	public Rect guiWindowRect = new Rect(-280, 40, 260, 420);
	public GUISkin guiSkin;

	private string textGuiButton = "Press the button";
	
	private void ShowGuiWindow(int windowID) 
	{
		GUILayout.BeginVertical();
		// ...
		GUILayout.EndVertical();
		
		// Make the window draggable.
		GUI.DragWindow();
	}
	
	
	void OnGUI()
	{
		Rect windowRect = guiWindowRect;
		if(windowRect.x < 0)
			windowRect.x += Screen.width;
		if(windowRect.y < 0)
			windowRect.y += Screen.height;
		
		GUI.skin = guiSkin;

		if(GUI.Button(new Rect(100, 100, 200, 200), textGuiButton))
		{
			if(textGuiButton == "Press the button")
				textGuiButton = "Button got pressed";
			else
				textGuiButton = "Press the button";
		}

		guiWindowRect = GUI.Window(1, windowRect, ShowGuiWindow, "GUI Window");
	}
	
}
