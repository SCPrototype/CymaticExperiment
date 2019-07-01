using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResetGame : MonoBehaviour
{
    public float pressDelay = 3.0f;
    private float pressStartTime;
    public KeyCode DutchReset;
    public KeyCode GermanReset;
    public KeyCode DutchQuestions;
    public KeyCode GermanQuestions;

    public Image FadeImage;
    public float FadeTime;
    public bool FadeInOnStart = true;
    private bool isFadingIn;
    private bool shouldFade = false;

    public UnityEvent OnFadedIn = new UnityEvent();
    private UnityEvent OnFadedOut = new UnityEvent();

    void Awake()
    {
        FMODUnity.RuntimeManager.LoadBank("Master Bank");
        FMODUnity.RuntimeManager.LoadBank("Master Bank.strings");

        if (FadeInOnStart)
        {
            //FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1);
            //DoFadeIn();
        }
        else
        {
            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!shouldFade)
        {
            if (Input.GetKeyDown(DutchReset) || Input.GetKeyDown(GermanReset))
            {
                pressStartTime = Time.time;
            }
            if (Input.GetKey(DutchReset) || Input.GetKey(GermanReset))
            {
                if (Time.time - pressStartTime >= pressDelay)
                {
                    pressStartTime = Time.time;
                    if (Input.GetKey(GermanReset))
                    {
                        GLOB.LanguageSelected = GLOB.Language.German;
                    }
                    if (Input.GetKey(DutchReset))
                    {
                        GLOB.LanguageSelected = GLOB.Language.Dutch;
                    }
                    OnFadedOut.RemoveAllListeners();
                    OnFadedOut.AddListener(DoResetGame);
                    DoFadeOut();
                    //DoResetGame();
                }
            }

            if (Input.GetKeyDown(DutchQuestions) || Input.GetKeyDown(GermanQuestions))
            {
                pressStartTime = Time.time;
            }
            if (Input.GetKey(DutchQuestions) || Input.GetKey(GermanQuestions))
            {
                if (Time.time - pressStartTime >= pressDelay)
                {
                    pressStartTime = Time.time;
                    OnFadedOut.RemoveAllListeners();
                    OnFadedOut.AddListener(GoToQuestions);
                    DoFadeOut();
                    //GoToQuestions();
                }
            }
        }

        if (shouldFade)
        {
            if (isFadingIn && FadeImage.color.a > 0)
            {
                FadeImage.color -= new Color(0, 0, 0, Time.deltaTime / FadeTime);
            }
            else if (isFadingIn && FadeImage.color.a <= 0)
            {
                OnFadedIn.Invoke();
                shouldFade = false;
            }
            else if (!isFadingIn && FadeImage.color.a < 1)
            {
                FadeImage.color += new Color(0, 0, 0, Time.deltaTime / FadeTime);
            }
            else if (!isFadingIn && FadeImage.color.a >= 1)
            {
                OnFadedOut.Invoke();
                shouldFade = false;
            }
        }
    }

    private void DoFadeIn()
    {
        shouldFade = true;
        isFadingIn = true;
    }
    private void DoFadeOut()
    {
        shouldFade = true;
        isFadingIn = false;
    }

    public void DoResetGame()
    {
        Application.LoadLevel(0);
    }
    public void GoToQuestions()
    {
        Application.LoadLevel(1);
    }
}
