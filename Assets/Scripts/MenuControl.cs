using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    #region TODO Locked Scenes   
    /*
    [SerializeField]
    Button Level2Button;

    private static bool isLevel2Playable;
    public bool IsLevel2Playable
    {
        get
        {
            return isLevel2Playable;
        }
        set
        {
            isLevel2Playable = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //IsLevel2Playable = true;
        Debug.Log("static Level 2 is playable:     " + isLevel2Playable);
        Debug.Log("Level 2 is playable:     " + IsLevel2Playable);
        try
        {
            if (isLevel2Playable || IsLevel2Playable)
            {
                Level2Button.interactable = true;
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Level 2 button does not exist in this scene.");
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    */
    #endregion
    public void LoadLevel(string Level)
    {
        SceneManager.LoadScene(Level);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RestartGame();
        //}
    }

    
}
