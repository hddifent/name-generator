using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleModule : OptionModule
{
    [SerializeField] private Toggle toggle;

    public delegate void BoolFunc(bool b);

    public void SetUpModule(string label, bool initValue, BoolFunc updateFunc)
    {
        this.label.text = label;

        toggle.isOn = initValue;

        toggle.onValueChanged.AddListener(delegate
        {
            updateFunc(toggle.isOn);
        });
    }

    public void SetUpModule(string label, bool initValue) { SetUpModule(label, initValue, delegate { }); }
    public void SetUpModule(string label) { SetUpModule(label, false); }
    public override void SetUpModule() { SetUpModule(""); }

    public void SyncValue(bool v) { toggle.isOn = v; }
}
