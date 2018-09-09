using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatHUD : MonoBehaviour {
    private static CombatHUD instance;

    public static CombatHUD Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CombatHUD>();
            }
            return instance;
        }
    }
    public Image blackBackground;
    public PlayerSelectionUI playerSelectionUI;

    public void FadeBlackBackground()
    {
        StartCoroutine(FadeOutBlackBackground());
    }

    private IEnumerator FadeOutBlackBackground()
    {
        float timeToFadeOutBlackBackground = 2.5f;

        float timer = 0;
        while (timer < timeToFadeOutBlackBackground)
        {
            timer += Time.deltaTime;
            blackBackground.color = new Color(0, 0, 0, 1 - (timer / timeToFadeOutBlackBackground));
            yield return null;
        }
        blackBackground.color = new Color(0, 0, 0, 0);
    }

    public void OpenPlayerSelectionMenu()
    {
        playerSelectionUI.gameObject.SetActive(true);
    }

    public void ClosePlayerSelectionMenu()
    {
        playerSelectionUI.gameObject.SetActive(false);
    }
}
