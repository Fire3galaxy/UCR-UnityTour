using UnityEngine;
using System.Collections;

public class RoomNameHolder : Photon.MonoBehaviour {
    public string roomName;
    public MainMenuLogic gameLogic;

    public void OnClickRoomItem() {
        gameLogic.OnClickRoomItem(roomName);
    }
}
