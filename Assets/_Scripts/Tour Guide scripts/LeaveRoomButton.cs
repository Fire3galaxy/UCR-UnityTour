public class LeaveRoomButton : Photon.MonoBehaviour {
    public void OnClickLeaveRoom() {
        GuideLogic.instance.OnClickLeave();
    }
}
