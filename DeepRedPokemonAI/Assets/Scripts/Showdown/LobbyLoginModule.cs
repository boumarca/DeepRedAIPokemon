using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyLoginModule : MonoBehaviour
{
    [SerializeField]
    Text _userLabel;

    public void SetUserLabel(string user)
    {
        _userLabel.text = user;
    }
}
