using Godot;
using System;

public partial class CameraButtonInterpretor : CanvasLayer
{
	public void SwitchToRoom(int key) {
		GameManager.Instance.ActiveGameParent.SetActiveCameraToKey(key);
		GD.Print("Switching to camera " +  key);
	}	
}
