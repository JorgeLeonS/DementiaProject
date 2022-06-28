using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] //needed to make ScriptableObject out of this class
public class DialogueElement
{
	// You would only store an index to the according character
	// Since I don't have your Characters type for now lets reference them via the EnemyStats.CharactersList
	public int CharacterID;

	public enum ActionType { Dialogue, Action };
	public ActionType actionToDo;

	//public Characters Character; 

	// By using the attribute [TextArea] this creates a nice multi-line text are field
	// You could further configure it with a min and max line size if you want: [TextArea(minLines, maxLines)]
	[TextArea] public string EnemyStatsText;
}

public class ActionToHave : MonoBehaviour
{
	//public string[] CharactersList; 
	public List<DialogueElement> DialogueItems;

	[Header("Basic")]
	public int health;
	public int defense;
	public float movementSpeed;

	[Header("Combat")]
	public int attack;
	public float attackRange;
	public float attackSpeed;

	[Header("Magic")]
	public int magicResistance;
	public bool hasMagic;
	public int mana;
	public enum MagicElementType { Fire, Water, Earth, Air };
	public MagicElementType magicType;
	public int magicDamage;
}