using UnityEngine;

namespace NTHiep.MiniOdin
{
    public class ColorHeaderAttribute : PropertyAttribute
    {
        public string text;

        public ColorHeaderAttribute(string text)
        {
            this.text = text;
        }
    }
}