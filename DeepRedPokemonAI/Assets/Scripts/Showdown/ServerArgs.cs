using com.MAB.Utils;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ServerArgs : ScriptableObject
{
	[SerializeField]
	private string _localhostUrl = default;
	[SerializeField]
	private string _showdownServerUrl = default;
	[SerializeField]
	private bool _connectToLocalhost = default;

	public string ServerUrl	{ get { return _connectToLocalhost ? _localhostUrl : _showdownServerUrl; } }

#if UNITY_EDITOR
	[MenuItem("Assets/Create/ServerArgs")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<ServerArgs>();
	}
#endif
}


