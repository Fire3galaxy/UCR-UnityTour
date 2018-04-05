public class EnterRoomListener : Photon.MonoBehaviour {
    public void OnClickRoomItem() {
        MainMenuLogic.instance.OnClickRoomItem(gameObject.name);
    }
}
