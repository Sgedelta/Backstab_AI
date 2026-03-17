using Godot;
using System;


/// <summary>
/// ButtonLinker is a helper class that can provide methods in a scene that will call methods in something like GameManager, for use for buttons
/// </summary>
public partial class ButtonLinker : Node
{
	private bool EnsureGMExists()
	{
		if(!IsInstanceValid(GameManager.Instance) || GameManager.Instance == null)
		{
			GD.Print("[Linker] Button Linker could not find GameManager! Not running command!");
			return false;
		}
		return true;
	}

	public void MakeGMRunCommands(NodePath path)
	{
		SabreRunner runner = GetNode<SabreRunner>(path);
		if (EnsureGMExists())
		{
			for(int i = 0; i < runner.SabreParsedOutput.Count; i++)
			{
				GD.Print($"Running Command: {runner.SabreParsedOutput[i]}");
				GameManager.Instance.ProcessSabreCommand(runner.SabreParsedOutput[i]);
			}
		}
	}
}
