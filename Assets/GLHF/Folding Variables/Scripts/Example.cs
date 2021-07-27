using UnityEngine;
using GLHF.FoldingVariables;

public class Example : MonoBehaviour
{
    [SerializeField] private string _testNumber1;
    [SerializeField] private int _testNumber2;
    [FoldingVariable] [SerializeField] private int _testNumber3;
    [FoldingVariable] [SerializeField] private GameObject _testNumber4;
}