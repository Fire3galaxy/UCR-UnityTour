using UnityEngine;

// Controls that are attached to Geodude player objects
// Used by the masterclient's geodude to control which video is playing
public class NetworkVideoControls : Photon.MonoBehaviour {
    ParticipantLogic videoLogic;

    void Start() {
        GameObject logicObject = GameObject.Find("Game and PUN Logic");
        videoLogic = logicObject.GetComponent<ParticipantLogic>();
    }

    [PunRPC]
    private void ChooseVideo(int tourNum, int vidNum) {
        videoLogic.LoadVideo(tourNum, vidNum);
    }
}
