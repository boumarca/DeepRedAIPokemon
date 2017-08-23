using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class LobbyBattleModule : MonoBehaviour
{
    const string LeftBracket = "[";
    const string RightBracket = "]";
    const string Space = " ";

    [SerializeField]
    Dropdown _formats;

    public void PopulateFormats(List<string> formatsList)
    {
        _formats.AddOptions(formatsList);
    }

    public string CurrentFormatId()
    {
        string formatText = _formats.options[_formats.value].text;
        StringBuilder s = new StringBuilder(formatText);
        s.Replace(LeftBracket, string.Empty);
        s.Replace(RightBracket, string.Empty);
        s.Replace(Space, string.Empty);
        return s.ToString().ToLower();
    }
}
