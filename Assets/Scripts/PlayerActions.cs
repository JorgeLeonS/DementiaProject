using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//[System.Serializable] public class PlayerCompletedAction : UnityEvent<bool> { }
//[System.Serializable] public class PlayerInteraction : UnityEvent { }

public class PlayerActions : MonoBehaviour
{
    public XROrigin MyXROrigin;
    private XRRayInteractor LeftXRRayInteractor;
    private XRRayInteractor RightXRRayInteractor;

    public UnityEvent PlayerCompletedAction = new UnityEvent();
    public UnityEvent PlayerInteraction = new UnityEvent();
    public event Action InteractWithSomething;

    public List<GameObject> itemsToInteract;

    private void Awake()
    {
        LeftXRRayInteractor = MyXROrigin.transform.GetChild(0).Find("LeftHandController").GetComponent<XRRayInteractor>();
        RightXRRayInteractor = MyXROrigin.transform.GetChild(0).Find("RightHandController").GetComponent<XRRayInteractor>();
    }

    //void PlayerAction(bool hasCompletedAction)
    //{
    //    playerCompletion.Invoke(hasCompletedAction);
    //}

    // Start is called before the first frame update
    void Start()
    {
        PlayerInteraction.AddListener(DoPlayerAction);
    }

    //public void CheckSelectedInteractable(XRRayInteractor obj)
    //{
    //    Debug.Log(obj.)
    //}

    void DoPlayerAction()
    {
        Debug.Log("Player is doing an action!");

        bool hasCompletedAction = true;
        PlayerCompletedAction.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        // Go to laying down position
        if (Input.GetKeyDown(KeyCode.M))
        {
            MyXROrigin.transform.position = new Vector3(0.6f, 0f, -1.07f);
            MyXROrigin.transform.Rotate(new Vector3(0, 0, 70));
        }
        // Go to standing position
        if (Input.GetKeyDown(KeyCode.N))
        {
            MyXROrigin.transform.position = new Vector3(0.65f, 0f, 0.39f);
            MyXROrigin.transform.Rotate(new Vector3(0, 0, -70));
        }
    }
}
