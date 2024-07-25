using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrequencyReciever : Puzzle
{
    [HideInInspector] public int buttonPressed = 20;
    int reset = 20;

    FrequencyHub stream;

    int time = 20;

    [Header("LineRenderer")]
    public LineRendererBehaviour ToMatch;
    public LineRendererBehaviour PlayerControlled;

    [Header("UI stuff")]
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Lights")]
    [SerializeField] GameObject lightOff;
    [SerializeField] GameObject lightOn;
    [SerializeField] GameObject wrongX;

    Coroutine resetTimerCo;
    private void Start()
    {
        stream = transform.parent.GetComponent<FrequencyHub>();
    }
    private void Update()
    {
        timerText.text = time.ToString();

        if (buttonPressed != reset)
        {
            switch (buttonPressed)
            {
                //Handle Frequency
                case 1:
                    ChangeFrequency(1);
                    break;
                case 2:
                    ChangeFrequency(-1);
                    break;

                //Handle Amp
                case 3:
                    ChangeAmp(0.5f);
                    break;
                case 4:
                    ChangeAmp(-0.5f);
                    break;

                //Handle Speed
                case 5:
                    ChangeSpeed(2);
                    break;
                case 6:
                    ChangeSpeed(-2);
                    break;

                //Hanlde Set
                case 7:
                    HandleSet();
                    break;
                default:
                    break;
            }
            buttonPressed = reset;
        }
    }

    public override void OnFocused()
    {
        if (!stream.won)
        {
            Randomize();
            resetTimerCo = StartCoroutine(ResetTimer());
        }
    }
    public override void OnUnFocus()
    {
        if (!stream.won)
        {
            StopCoroutine(resetTimerCo);
            time = 20;
        }
    }


    void ChangeFrequency(int value)
    {
        if(value > 0 && PlayerControlled.frequency == 6)
        {
            
        }
        else if(value < 0 && PlayerControlled.frequency == 1)
        {

        }
        else
        {
            PlayerControlled.frequency += value;
        }
    }
    void ChangeAmp(float value)
    {
        if (value > 0 && PlayerControlled.amplitude == 3.5f)
        {

        }
        else if (value < 0 && PlayerControlled.frequency == 1)
        {

        }
        else
        {
            PlayerControlled.amplitude += value;
        }
    }
    void ChangeSpeed(int value)
    {
        if (value > 0 && PlayerControlled.speed == 12)
        {

        }
        else if (value < 0 && PlayerControlled.speed == 2)
        {

        }
        else
        {
            PlayerControlled.speed += value;

        }
    }

    void Randomize()
    {
        ToMatch.frequency = Random.Range(1, 7);
        ToMatch.amplitude = Random.Range(2, 8) / 2;
        ToMatch.speed = Random.Range(1, 7) * 2;
    }
    IEnumerator ResetTimer()
    {
        time = 20;
        for (int i = 0; i < 20; i++)
        {
            time--;
            yield return new WaitForSeconds(1);
            if(time == 0)
            {
                StopCoroutine(resetTimerCo);
                Randomize();
                resetTimerCo = StartCoroutine(ResetTimer());
            }
        }
    }

    void HandleSet()
    {
        if(ToMatch.frequency == PlayerControlled.frequency &&
            ToMatch.amplitude == PlayerControlled.amplitude &&
            ToMatch.speed == PlayerControlled.speed)
        {
            stream.SendIfWon(true);
            StopCoroutine(resetTimerCo);
            lightOff.SetActive(false);
            lightOn.SetActive(true);
        }
        else
        {
            StopCoroutine(resetTimerCo);
            Randomize();
            StartCoroutine(wrong());
            resetTimerCo = StartCoroutine(ResetTimer());
        }
    }

    IEnumerator wrong()
    {
        wrongX.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        wrongX.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        wrongX.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        wrongX.SetActive(false);
    }
}
