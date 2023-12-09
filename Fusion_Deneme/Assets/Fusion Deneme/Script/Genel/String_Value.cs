using UnityEngine;

[CreateAssetMenu(menuName = "Value/String Value")]
public class String_Value : ScriptableObject
{
    public string myString;

    public void SetStringValue(string s)
    {
        myString = s;
    }
    public string GetStringValue()
    {
        return myString;
    }
}