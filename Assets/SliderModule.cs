using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderModule : OptionModule
{
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private Slider slider;

    private string displayFormat = "%value%";

    public delegate void IntFunc(int i);
    public delegate void FloatFunc(float f);

    /// <summary>
    /// Initialize the slider module. Whole number.
    /// </summary>
    /// <param name="displayFormat">Use %value% to represent value.</param>
    public void SetUpModule(string label, int initValue, int lowLimit, int highLimit, string displayFormat, IntFunc updateFunction)
    {
        this.label.text = label;

        slider.wholeNumbers = true;

        slider.minValue = lowLimit;
        slider.maxValue = highLimit;
        slider.value = initValue;

        slider.onValueChanged.AddListener(delegate
        {
            UpdateValue((int)slider.value);
            updateFunction((int)slider.value);
        });

        this.displayFormat = displayFormat;

        UpdateValue((int)slider.value);
    }

    /// <summary>
    /// Initialize the slider module.
    /// </summary>
    /// <param name="displayFormat">Use %value% to represent value.</param>
    public void SetUpModule(string label, float initValue, float lowLimit, float highLimit, string displayFormat, FloatFunc updateFunction)
    {
        this.label.text = label;

        slider.wholeNumbers = false;

        slider.minValue = lowLimit;
        slider.maxValue = highLimit;
        slider.value = initValue;

        slider.onValueChanged.AddListener(delegate
        {
            UpdateValue(slider.value, 2);
            updateFunction(slider.value);
        });

        this.displayFormat = displayFormat;

        UpdateValue(slider.value, 2);
    }

    /// <param name="displayFormat">Use %value% to represent value.</param>
    public void SetUpModule(string label, int initValue, int lowLimit, int highLimit, string displayFormat) { SetUpModule(label, initValue, lowLimit, highLimit, displayFormat, delegate { }); }
    /// <param name="displayFormat">Use %value% to represent value.</param>
    public void SetUpModule(string label, float initValue, float lowLimit, float highLimit, string displayFormat) { SetUpModule(label, initValue, lowLimit, highLimit, displayFormat, delegate { }); }
    public void SetUpModule(string label, int initValue, int lowLimit, int highLimit) { SetUpModule(label, initValue, lowLimit, highLimit, "%value%"); }
    public void SetUpModule(string label, float initValue, float lowLimit, float highLimit) { SetUpModule(label, initValue, lowLimit, highLimit, "%value%"); }
    public void SetUpModule(string label) { SetUpModule(label, 0f, 0f, 1f); }
    public override void SetUpModule() { SetUpModule(""); }  

    public void UpdateValue(int i)
    {
        value.text = displayFormat.Replace("%value%", i.ToString());
    }

    public void UpdateValue(float f)
    {
        value.text = displayFormat.Replace("%value%", f.ToString());
    }

    public void UpdateValue(float f, int precision)
    {
        value.text = displayFormat.Replace("%value%", f.ToString($"F{precision}"));
    }

    public void SyncValue(int v) { slider.value = v; }
    public void SyncValue(float v) { slider.value = v; }
}
