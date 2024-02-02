using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NovelPrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _novelTMP = default;

    public TMP_Text NovelTMP => _novelTMP;
    
    public bool IsPrinting => true;
    
    public void Skip() {}
    
    public void NextMessage() { }
}
