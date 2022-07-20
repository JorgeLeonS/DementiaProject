using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Script used to transition between scenes.
/// </summary>
public class MenuControl : MonoBehaviour
{
    public GameObject fadeCanvas;
    private void Awake()
    {
        try
        {
            fadeCanvas.SetActive(true);
            FadeCanvas.FadeOut();
        }
        catch (System.Exception e)
        {
            Debug.LogError("FadeCanvas could not be found! \n" +
                $"Make sure the object is on Scene and has the correct name! \n {e}");
        }
    }
    public static void LoadLevel(string Level)
    {
        Time.timeScale = 1;
        FadeCanvas.FadeIn().OnComplete(() => SceneManager.LoadScene(Level));
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
