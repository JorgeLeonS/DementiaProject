using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable] public class PlayerCompletedAction : UnityEvent<bool> { }
//[System.Serializable] public class PlayerInteraction : UnityEvent { }

public class PlayerActions : MonoBehaviour
{
    public UnityEvent PlayerCompletedAction = new UnityEvent();
    public UnityEvent PlayerInteraction = new UnityEvent();

    //void PlayerAction(bool hasCompletedAction)
    //{
    //    playerCompletion.Invoke(hasCompletedAction);
    //}

    // Start is called before the first frame update
    void Start()
    {
        PlayerInteraction.AddListener(DoPlayerAction);
    }

    void DoPlayerAction()
    {
        Debug.Log("Player is doing an action!");

        bool hasCompletedAction = true;
        PlayerCompletedAction.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
