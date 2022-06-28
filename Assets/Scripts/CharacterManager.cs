using System;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour
{
	public List<CharacterActions> characterActions;
}
public struct CharacterActions
{
	public enum ActionType { Dialogue, Action };
	public ActionType actionType;
	public int quantity;
	[TextArea] public string Dialogue;
	public AudioClip DialogueAudio;
	// Only to be had by characters
	public enum Animation { Wave, ELSE };
	public bool MoveToNextLocation;
}