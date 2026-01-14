using UnityEngine;
using System;

namespace NTHiep.MiniOdin
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string fieldName;
        public int enumValue;

        public ShowIfAttribute(string fieldName, int enumValue)
        {
            this.fieldName = fieldName;
            this.enumValue = enumValue;
        }
    }
}