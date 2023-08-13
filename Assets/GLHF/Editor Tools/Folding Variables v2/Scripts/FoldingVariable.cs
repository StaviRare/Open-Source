using UnityEngine;
using System;

namespace GLHF.FoldingVariables2
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class FoldingVariable : PropertyAttribute
    {
        public string foldoutName;

        public FoldingVariable(string foldoutName)
        {
            this.foldoutName = foldoutName;
        }
    }
}