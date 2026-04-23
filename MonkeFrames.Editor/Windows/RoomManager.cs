using GorillaNetworking;
using MonkeFrames.Editor.Interfaces;
using UnityEngine;

namespace MonkeFrames.Editor.Windows;

public class RoomManager : IEditorWindow
{
    public string Name => "Room Manager";
    public Rect Rect => new Rect(60, 60, 300, 85);

    private string roomCode;

    public void JoinRoom(string room)
    {
        if (NetworkSystem.Instance.InRoom)
            NetworkSystem.Instance.ReturnToSinglePlayer();

        if (room == "") return;

        PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(room.ToUpper(), JoinType.Solo);
    }

    public void OnDraw() {
        roomCode = GUI.TextField(new Rect(10, 30, 280, 20), roomCode);

        if (GUI.Button(new Rect(10, 55, 135, 20), "Join"))
            JoinRoom(roomCode);

        if (GUI.Button(new Rect(155, 55, 135, 20), "Disconnect"))
            JoinRoom("");
    }
}