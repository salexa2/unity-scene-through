//Open Source Clock Timer code provided from https://github.com/herbou/Unity_ReusableTimers.git
//Thank you Hamza Herbou

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using TMPro; // Added TextMeshPro namespace

public class ClockTimer : MonoBehaviour
{
    [Header("Timer UI references :")]
    [SerializeField] private Image uiFillImage;
    [SerializeField] private TMP_Text uiText; // Replaced Text with TMP_Text

    public int Duration { get; private set; }

    public bool IsPaused { get; private set; }

    private int remainingDuration;

    public PlayerMovement pmt;

    // Events --
    private UnityAction onTimerBeginAction;
    private UnityAction<int> onTimerChangeAction;
    private UnityAction onTimerEndAction;
    private UnityAction<bool> onTimerPauseAction;

    private void Awake()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        uiText.text = "00:00";
        uiFillImage.fillAmount = 0f;

        Duration = remainingDuration = 0;

        onTimerBeginAction = null;
        onTimerChangeAction = null;
        onTimerEndAction = null;
        onTimerPauseAction = null;

        IsPaused = false;
    }

    public void SetPaused(bool paused)
    {
        IsPaused = paused;

        if (onTimerPauseAction != null)
            onTimerPauseAction.Invoke(IsPaused);
    }


    public ClockTimer SetDuration(int seconds)
    {
        Duration = remainingDuration = seconds;
        return this;
    }

    //-- Events ----------------------------------
    public ClockTimer OnBegin(UnityAction action)
    {
        onTimerBeginAction = action;
        return this;
    }

    public ClockTimer OnChange(UnityAction<int> action)
    {
        onTimerChangeAction = action;
        return this;
    }

    public ClockTimer OnEnd(UnityAction action)
    {
        onTimerEndAction = action;
        return this;
    }

    public ClockTimer OnPause(UnityAction<bool> action)
    {
        onTimerPauseAction = action;
        return this;
    }





    public void Begin()
    {
        if (onTimerBeginAction != null)
            onTimerBeginAction.Invoke();

        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration > 0)
        {
            if (!IsPaused)
            {
                if (onTimerChangeAction != null)
                    onTimerChangeAction.Invoke(remainingDuration);

                UpdateUI(remainingDuration);
                remainingDuration--;
            }
            yield return new WaitForSeconds(1f);
        }
        End();
    }

    private void UpdateUI(int seconds)
    {
        uiText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
        uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
    }

    public void End()
    {
        if (onTimerEndAction != null)
            onTimerEndAction.Invoke();

        pmt.UniveralDie(0);
        ResetTimer();
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

