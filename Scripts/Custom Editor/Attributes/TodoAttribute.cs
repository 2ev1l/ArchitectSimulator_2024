using UnityEngine;
using System;

namespace EditorCustom.Attributes
{
    /// <summary>
    /// Do nothing but indicates what else need to be edited
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class TodoAttribute : PropertyAttribute
    {
        private string description;

        public TodoAttribute(string description = "")
        {
            this.description = description;
        }
    }
}