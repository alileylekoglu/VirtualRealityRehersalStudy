using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
         DrawDefaultInspector();
         EditorGUILayout.HelpBox("This script is responsible for creating and joining rooms", MessageType.Info);
         RoomManager roomManager = (RoomManager)target;
         
         if (GUILayout.Button("Joim Random Room"))
         {
             roomManager.JoinRandomRoom();
         }

         if (GUILayout.Button("Join Practise One"))
         {
             roomManager.OnEnterButtonClicked_PractiseOne();
         }

         if (GUILayout.Button("Join Practise Two"))
         {
             roomManager.OnEnterButtonClicked_PractiseTwo();
         }
       
         if (GUILayout.Button("Join Practise Three"))
         {
             roomManager.OnEnterButtonClicked_PractiseThree();
         }

         if (GUILayout.Button("Join Practise Four"))
         {
             roomManager.OnEnterButtonClicked_PractiseFour();
         }

    }
}