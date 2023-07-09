using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultBlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, clueText;

    public void SetBlock(string name, string clue)
    {
        nameText.text = name;
        clueText.text = clue;
    }
}
