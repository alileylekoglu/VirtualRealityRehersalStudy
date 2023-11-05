using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    public Button leaveRoomButton; // Assign this button via the Inspector

    private void Start()
    {
        // Add a listener to the button that calls the LeaveRoom function when clicked
        if (leaveRoomButton != null)
            leaveRoomButton.onClick.AddListener(LeaveRoomAndGoToRoomSelection);
    }

    // This function is called when the button is clicked
    public void LeaveRoomAndGoToRoomSelection()
    {
        // Tell Photon to leave the room
        PhotonNetwork.LeaveRoom();
    }

    // Photon callback called after successfully leaving a room
    public override void OnLeftRoom()
    {
        // After leaving the room, load the RoomSelection scene
        SceneManager.LoadScene("Login");
    }

    // If OnLeftRoomFailed is not a part of MonoBehaviourPunCallbacks, it should be removed
}