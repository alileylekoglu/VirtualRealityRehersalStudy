using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]
public class LoginManagerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    EditorGUILayout.HelpBox("This script is used to connect to Photon Server", MessageType.Info);
    LoginManager loginManager = (LoginManager)target;
    
    if (GUILayout.Button("Connect to Photon Server"))
    {
      loginManager.ConnectToPhotonServer();
    }
  }
}
