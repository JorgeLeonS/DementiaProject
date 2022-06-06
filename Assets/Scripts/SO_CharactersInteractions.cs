using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="CharacterData", menuName ="ScriptableObjects/CharacterInteractions")]
public class SO_CharactersInteractions : ScriptableObject
{
    public string Name;
    public List<string> dialogueText;
    public List<AudioClip> dialogueAudios;
    public List<string> animation;
    public List<bool> moveToNextLocation;
    // TODO missing actions
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharactersNames")]
public class SO_Characters : ScriptableObject
{
    public List<string> Names;
}