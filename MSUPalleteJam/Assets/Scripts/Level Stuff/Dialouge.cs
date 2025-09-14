using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Dialouge : MonoBehaviour
{
    [SerializeField] private TMP_Text _outputText; 

    [SerializeField] private float _typewriterTypeSpeed = 0.01f;

    [SerializeField] private List<DialougeLine_t> _lines;

    [SerializeField] private UnityEvent _onDialougeConclude; 

    

    public void BeginDialouge()
    {
        StartCoroutine(DialougeRoutine()); 
    }

    private IEnumerator DialougeRoutine()
    {
        foreach (DialougeLine_t line in _lines)
        {
            _outputText.text = "";
            string cString = "";
            for(int i = 0; i < line.Line.Length; i++)
            {
                cString += line.Line[i];

                _outputText.text = cString;

                yield return new WaitForSecondsRealtime(_typewriterTypeSpeed); 

            }

            yield return new WaitForSecondsRealtime(line.ReadTime);
        }

        _onDialougeConclude.Invoke(); 
    }

   
}
[System.Serializable]
struct DialougeLine_t
{
    public string Line;
    public float ReadTime; // how long to keep the line up before switching to the next line
}
