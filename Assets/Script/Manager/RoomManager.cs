using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public  class RoomManager : MonoBehaviourPunCallbacks
    {
     private string mapType;
     
     public TextMeshProUGUI numberOfPlayers_PractiseOne;
     public TextMeshProUGUI numberOfPlayers_PractiseTwo;
     public TextMeshProUGUI numberOfPlayers_PractiseThree;
     public TextMeshProUGUI numberOfPlayers_PractiseFour;
     
        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinLobby();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        #endregion

#region UI Callback Methods

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        
        public void OnEnterButtonClicked_PractiseOne()
        {
           mapType = MultiplayerVRConstants.MAP_TYPE_KEY_PractiseOne;
           ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
           PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 10);
        }
        
        public void OnEnterButtonClicked_PractiseTwo()
        {
            mapType = MultiplayerVRConstants.MAP_TYPE_KEY_PractiseTwo;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 10);

        }
        
        public void OnEnterButtonClicked_PractiseThree()
        {
            mapType = MultiplayerVRConstants.MAP_TYPE_KEY_PractiseThree;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 10);

        }
        
        public void OnEnterButtonClicked_PractiseFour()
        {
            mapType = MultiplayerVRConstants.MAP_TYPE_KEY_PractiseFour;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 10);

        }
        

#endregion

#region Photon Callbacks Methods

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            CreateAndJoinRoom();
        }
        
        public override void OnCreatedRoom()
        {
            Debug.Log("Room is created successfully"+ PhotonNetwork.CurrentRoom.Name);   
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("The player has joined the room"+ PhotonNetwork.NickName+ "joined to"+ PhotonNetwork.CurrentRoom.Name+ "Player Count"+ PhotonNetwork.CurrentRoom.PlayerCount);

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out object mapType))
                {
                    Debug.Log("Map Name: "+ (string)mapType);

                    if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_KEY_PractiseOne)
                    {
                        PhotonNetwork.LoadLevel("PractiseOne");
                    }
                    else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_KEY_PractiseTwo)
                    {
                        PhotonNetwork.LoadLevel("PractiseTwo");
                    }
                    else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_KEY_PractiseThree)
                    {
                        PhotonNetwork.LoadLevel("PractiseThree");
                    }
                    else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_KEY_PractiseFour)
                    {
                        PhotonNetwork.LoadLevel("PractiseFour");

                    }
                }
            }
            
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log(newPlayer.NickName+ "Joined to: "+ "Player Count: "+ PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (roomList.Count == 0)
            {
                numberOfPlayers_PractiseOne.text = 0 + " / "+ 10;
                numberOfPlayers_PractiseTwo.text = 0 + " / "+ 10;
                numberOfPlayers_PractiseThree.text = 0 + " / "+ 10;
                numberOfPlayers_PractiseFour.text = 0 + " / "+ 10;
            }

            foreach (RoomInfo room in roomList) 
            {
                Debug.Log(room.Name);
                if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_KEY_PractiseOne))
                {
                    Debug.Log("Room is a practise one. Player Count: "+ room.PlayerCount);
                    numberOfPlayers_PractiseOne.text = room.PlayerCount + " / "+ 10;
                }
                else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_KEY_PractiseTwo))
                {
                    Debug.Log("Room is a practise two. Player Count: "+ room.PlayerCount);
                    numberOfPlayers_PractiseTwo.text = room.PlayerCount + " / "+ 10;
                }
                else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_KEY_PractiseThree))
                {
                    Debug.Log("Room is a practise three. Player Count: "+ room.PlayerCount);
                    numberOfPlayers_PractiseThree.text = room.PlayerCount + " / "+ 10;
                }
                else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_KEY_PractiseFour))
                {
                    Debug.Log("Room is a practise four. Player Count: "+ room.PlayerCount);
                    numberOfPlayers_PractiseFour.text = room.PlayerCount + " / "+ 10;
                }
                
                
            }
        }
        
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined to Lobby");
        }
         
#endregion

#region Private Methods

        private void CreateAndJoinRoom()
        {
            string randomRoomName = "Room" + mapType + Random.Range(0, 10000);
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;
            
            string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY};
            
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() {{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
            
            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;
            
            PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
        }


#endregion

    }