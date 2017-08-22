using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBattleModule : MonoBehaviour
{
    [SerializeField]
    Dropdown _formats;

    public void PopulateFormats(List<string> formatsList)
    {
        _formats.AddOptions(formatsList);
    }
}
