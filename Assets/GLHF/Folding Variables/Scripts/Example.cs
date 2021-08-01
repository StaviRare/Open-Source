using UnityEngine;
using GLHF.FoldingVariables;

public class Example : MonoBehaviour
{
    [SerializeField] private int _serializedVariable = 1;
    
    [FoldingVariable] [HideInInspector] public int _hiddenFoldedVariable = 2;
    
    [FoldingVariable] [SerializeField] private string _foldedVariable01 = "This is a folded variable";
    
    [FoldingVariable] [SerializeField] private GameObject _foldedVariable02 = null;
}