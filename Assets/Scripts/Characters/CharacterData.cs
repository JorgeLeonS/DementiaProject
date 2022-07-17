using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A character can have the listed components.
/// They are filled for each scene, and are referenced from <see cref="CharacterController"/>
/// </summary>

public class CharacterData : MonoBehaviour
{
    public string Name;
    public List<string> DialogueText;
    public List<AudioClip> DialogueAudios;
    public List<float> DialogueDurations;
    public List<string> AnimationName;
    public List<bool> MoveToNextLocation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}