using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SubclassSelectorAttribute : PropertyAttribute
{
    private bool _includeMono;

    public SubclassSelectorAttribute(bool includeMono = false)
    {
        _includeMono = includeMono;
    }

    public bool IsIncludeMono() => _includeMono;
}
