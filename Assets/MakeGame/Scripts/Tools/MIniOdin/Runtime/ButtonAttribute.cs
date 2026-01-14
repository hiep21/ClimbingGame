using System;
using UnityEngine;

namespace NTHiep.MiniOdin
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Label;

        public ButtonAttribute(string label = null)
        {
            Label = label;
        }
    }
}
