using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    // To add more options, add the property, option module (setup and add in dict), default value, and getter.

    public static int AnimationSpeed { get; private set; } = 20;
    public static float AnimationTime { get; private set; } = 2.00f;
    public static bool PanicMode { get; private set; } = false;
    public static bool Mute { get; private set; } = false;

    [SerializeField] private GameObject sliderBox;
    [SerializeField] private GameObject toggleBox;

    [SerializeField] private Transform optionField;

    private Dictionary<string, OptionModule> allOption;

    void Start()
    {
        SliderModule animSpeed = Instantiate(sliderBox, optionField, false).GetComponent<SliderModule>();
        SliderModule animTime = Instantiate(sliderBox, optionField, false).GetComponent<SliderModule>();
        ToggleModule panicMode = Instantiate(toggleBox, optionField, false).GetComponent<ToggleModule>();
        ToggleModule mute = Instantiate(toggleBox, optionField, false).GetComponent<ToggleModule>();

        animSpeed.SetUpModule("Animation Speed", AnimationSpeed, 1, 60, "%value% tps", SetAnimationSpeed);
        animTime.SetUpModule("Animation Time", AnimationTime, 0f, 5f, "%value% sec", SetAnimationTime);
        panicMode.SetUpModule("Panic Mode", PanicMode, SetPanicMode);
        mute.SetUpModule("Mute Sounds", Mute, SetMute);

        allOption = new Dictionary<string, OptionModule>
        {
            { "AnimationSpeed", animSpeed },
            { "AnimationTime", animTime },
            { "PanicMode", panicMode },
            { "Mute", mute }
        };
    }

    public void ResetToDefault()
    {
        AnimationSpeed = 20;
        AnimationTime = 2.00f;
        PanicMode = false;
        Mute = false;

        (allOption["AnimationSpeed"] as SliderModule).SyncValue(AnimationSpeed);
        (allOption["AnimationTime"] as SliderModule).SyncValue(AnimationTime);
        (allOption["PanicMode"] as ToggleModule).SyncValue(PanicMode);
        (allOption["Mute"] as ToggleModule).SyncValue(Mute);
    }

    public void SetAnimationSpeed(int i) { AnimationSpeed = i; }
    public void SetAnimationTime(float f) { AnimationTime = f; }
    public void SetPanicMode(bool b) { PanicMode = b; }
    public void SetMute(bool b) { Mute = b; }

}
