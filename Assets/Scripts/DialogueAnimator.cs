//  Adapted class from:
//  https://github.com/mixandjam/AC-Dialogue/blob/0f692d26517b081eabb981c49430774fa6d623ae/Assets/TMP_Animated/Runtime/TMP_Animated.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
    [System.Serializable] public class ActionEvent : UnityEvent<string> { }

    [System.Serializable] public class TextRevealEvent : UnityEvent<char> { }

    [System.Serializable] public class DialogueEvent : UnityEvent { }
    public class DialogueAnimator : TextMeshProUGUI
    {
        public ActionEvent onAction;
        public TextRevealEvent onTextReveal;
        public DialogueEvent onDialogueFinish;

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

            IEnumerator DisplayText()
            {
                int subCounter = 0;
                int visibleCounter = 0;
                while (subCounter < subTexts.Length)
                {
                    while (visibleCounter < subTexts[subCounter].Length)
                    {
                        onTextReveal.Invoke(subTexts[subCounter][visibleCounter]);
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
        
        public void ReadText(string textToRead, AudioClip audioToPlay)
        {
            float audioLength = audioToPlay.length*0.8f;
            ReadText(textToRead, audioLength);
        }
    }
}