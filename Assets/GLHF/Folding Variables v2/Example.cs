using UnityEngine;

namespace GLHF.FoldingVariables2
{
    public class Example : MonoBehaviour
    {
        // Visible fields
        public int VisibleInt;
        [SerializeField] private string VisibleString;

        // Invisible fields
        [HideInInspector] private int InVisibleInt;
        [HideInInspector] public string InVisibleString;

        // Visible fields in custom foldout
        [FoldingVariable("Foldout 1")] public int IntValue;
        [FoldingVariable("Foldout 1")] public string StringValue;
        [FoldingVariable("Foldout 2")] public GameObject GameObjectValue;
    }
}