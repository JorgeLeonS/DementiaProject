//  Adapted class from:
//  https://github.com/mixandjam/AC-Dialogue/blob/0f692d26517b081eabb981c49430774fa6d623ae/Assets/TMP_Animated/Runtime/TMP_Animated.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
    /// <summary>
    /// Class used to spawn each character of a text depending on a duration.
    /// Both the player and character have it assigned on their respective Canvas.
    /// </summary>
    public class DialogueAnimator : TextMeshProUGUI
    {
        /// <summary>
        /// Only method that is used for this implementation.
        /// ReadText will calculate the spawning speed of the characters with the readSpeed variable.
        /// It divides the textToRead into an array of Chars and then calls the DisplayText coroutine.
        /// </summary>
        public void ReadText(string textToRead, float textDuration)
        {
            float readSpeed = textDuration / textToRead.Length;
            string[] subTexts = textToRead.Split("");

            string displayText = "";

            for (int i = 0; i < subTexts.Length; i++)
            {
                if (i % 2 == 0)
                    displayText += subTexts[i];
            }

            text = displayText;
            maxVisibleCharacters = 0;

            StartCoroutine(DisplayText());

            /// <summary>
            /// Coroutine used to display the text during the duration.
            /// What allows the text to have this behviour is the maxVisibleCharacters variable, from TMPro.
            /// </summary>
            IEnumerator DisplayText()
            {
                int subCounter = 0;
                int visibleCounter = 0;
                while (subCounter < subTexts.Length)
                {
                    while (visibleCounter < subTexts[subCounter].Length)
                    {
                        visibleCounter++;
                        maxVisibleCharacters++;
                        yield return new WaitForSeconds(readSpeed);
                    }
                    visibleCounter = 0;
                    subCounter++;
                    yield return null;
                }
            }
        }
        
        /// <summary>
        /// The method can also be called, sending an AudioClip parameter, 
        /// which length will replace the duration of the original method.
        /// </summary>
        public void ReadText(string textToRead, AudioClip audioToPlay)
        {
            float audioLength = audioToPlay.length*0.8f;
            ReadText(textToRead, audioLength);
        }
    }
}