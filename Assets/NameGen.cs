using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class NameGen : MonoBehaviour
{
    public TextMeshProUGUI nameText, clueText, remainingText;
    public TMP_InputField nameFileInputField, clueFileInputField;

    public GameObject resultGroup, resultBlock;

    public AudioControl audioControl;

    private string nameFilePath, clueFilePath;
    private List<string> nameList, clueList, permNameList, permClueList;
    private Dictionary<string, string> allPair = new();

    [SerializeField] private Button[] allRandomizerButton;
    [SerializeField] private Button fileSubmitButton;
    private bool keyLock = false;

    private Coroutine rollTemp;

    void Start()
    {
        nameFileInputField.onEndEdit.AddListener(SetNameFilePath);
        clueFileInputField.onEndEdit.AddListener(SetClueFilePath);
    }

    void Update()
    {
        nameText.transform.localScale = Vector3.Lerp(nameText.transform.localScale, Vector3.one, 0.05f);
        clueText.transform.localScale = Vector3.Lerp(clueText.transform.localScale, Vector3.one, 0.05f);

        fileSubmitButton.interactable = File.Exists(nameFilePath) && File.Exists(clueFilePath);
    }

    public void InitializeRandomizer()
    {
        if (!(File.Exists(nameFilePath) && File.Exists(clueFilePath))) { return; }

        nameList = File.ReadAllLines(nameFilePath).ToList();
        clueList = File.ReadAllLines(clueFilePath).ToList();

        permNameList = new(nameList);
        permClueList = new(clueList);

        allPair = new();
        foreach (Transform t in resultGroup.transform.GetComponentsInChildren<Transform>())
        {
            if (t == resultGroup.transform) { continue; }
            Destroy(t.gameObject);
        }

        for (int i = 0; i < clueList.Count; i++) { clueList[i] = clueList[i].Replace("\\n", "\n"); }
        for (int i = 0; i < permClueList.Count; i++) { permClueList[i] = permClueList[i].Replace("\\n", "\n"); }
        if (nameList.Count != clueList.Count) { Debug.LogWarning($"Count is not the same.\nName: {nameList.Count}\nClue: {clueList.Count}"); }

        nameText.text = "Lucky Star";
        clueText.text = "Who will it be!?";

        remainingText.text = $"Remaining: {nameList.Count}";
    }

    public void ClickGeneratePair()
    {
        if (nameList.Count == 0 || clueList.Count == 0 || keyLock) { return; }

        if (SettingControl.PanicMode)
        {
            audioControl.PlayDesperateSFX();
            _ = StartCoroutine(PanicGenerate());
        }
        else
        {
            GeneratePair();
        }
    }

    public void ExportToCSV()
    {
        string pathToFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\NameData-" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv";
        string content = "";

        foreach (KeyValuePair<string, string> p in allPair) { content += $"\"{ p.Key.Replace("\"", "\"\"") }\",\"{ p.Value.Replace("\"", "\"\"") }\"\n"; }

        File.WriteAllText(pathToFile, content, System.Text.Encoding.Unicode);
    }

    private void GeneratePair()
    {
        if (nameList.Count == 0 || clueList.Count == 0) { return; }

        if (rollTemp != null) { StopCoroutine(rollTemp); }

        int rn = Random.Range(0, nameList.Count), rc = Random.Range(0, clueList.Count);
        string pName = nameList[rn], pClue = clueList[rc];

        nameList.RemoveAt(rn);
        clueList.RemoveAt(rc);

        allPair.Add(pName, pClue);
        CreateLogObject(pName, pClue);

        if (SettingControl.PanicMode || SettingControl.AnimationTime == 0)
        {
            rollTemp = StartCoroutine(DelayDisplay(0, 0, pName, pClue));
        }
        else
        {
            rollTemp = StartCoroutine(DelayDisplay(1f / SettingControl.AnimationSpeed,
                                        (int)(SettingControl.AnimationSpeed * SettingControl.AnimationTime),
                                        pName, pClue));
            audioControl.PlayRollSFX(Mathf.Pow((float)(permClueList.Count - nameList.Count) / permClueList.Count, 3));
        }

        // Debug.Log($"{nameList.Count} {permNameList.Count}");
    }

    private void CreateLogObject(string name, string clue)
    {
        GameObject g = Instantiate(resultBlock);
        g.transform.SetParent(resultGroup.transform, false);

        ResultBlock rst = g.GetComponent<ResultBlock>();
        rst.SetBlock(name, clue);
    }

    private void KeyLock(bool toggle)
    {
        keyLock = toggle;
        foreach (Button b in allRandomizerButton) { b.interactable = !toggle; }
    }

    private void SetNameFilePath(string path) { nameFilePath = path; }
    private void SetClueFilePath(string path) { clueFilePath = path; }

    public IEnumerator DelayDisplay(float period, int amount, string trueName, string trueClue)
    {
        KeyLock(true);

        int i = 0;
        while (i < amount)
        {
            nameText.text = permNameList[Random.Range(0, permNameList.Count)];
            clueText.text = permClueList[Random.Range(0, permClueList.Count)];
            yield return new WaitForSecondsRealtime(period);
            i++;
        }

        nameText.text = trueName;
        clueText.text = trueClue;

        nameText.transform.localScale = Vector3.one * 1.5f;
        clueText.transform.localScale = Vector3.one * 1.5f;

        remainingText.text = $"Remaining: {nameList.Count}";

        audioControl.PlayShowSFX();

        KeyLock(SettingControl.PanicMode && nameList.Count > 0 && clueList.Count > 0);
    }

    public IEnumerator PanicGenerate()
    {
        audioControl.SetVolume(AudioControl.Player.player, 0.5f);

        while (nameList.Count > 0 && clueList.Count > 0)
        {
            GeneratePair();
            yield return new WaitForSecondsRealtime(.5f);
        }

        audioControl.StopPlayer(AudioControl.Player.desperatePlayer);
        audioControl.SetVolume(AudioControl.Player.player, 1f);
    }
}
