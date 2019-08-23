using UnityEngine;
using UnityEngine.UI;
using com.MAB.Web;

public class DebugConsole : MonoBehaviour
{
    [SerializeField]
    InputField _console = default;

    public void Submit()
    {
        WebsocketConnection.Instance.Send(_console.text);
    }	
}
