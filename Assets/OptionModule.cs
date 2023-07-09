using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class OptionModule : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI label;

    public abstract void SetUpModule();
}
