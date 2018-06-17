using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {

    public static PopupManager instance;

    public TextMeshProUGUI infoText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowNotification()
    {

    }

    public void ShowInfo(string text, float showtime)
    {
        if(infoText != null)
        {
            StartCoroutine(InfoPopup(text, showtime));
        }
    }

    IEnumerator InfoPopup(string text, float time)
    {
        infoText.SetText(text);
        Animation anim = GetComponent<Animation>();
        anim["InfoTextFade"].speed = 1f;
        anim["InfoTextFade"].time = 0f;
        anim.Play("InfoTextFade");
        yield return new WaitForSeconds(time/1000f);
        anim["InfoTextFade"].speed = -1f;
        anim["InfoTextFade"].time = anim["InfoTextFade"].length;
        anim.Play("InfoTextFade");
    }
}
