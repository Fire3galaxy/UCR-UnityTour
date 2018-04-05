using UnityEngine;

public class TourGuideButton : Photon.MonoBehaviour {
    private int index;
    
    public void setIndex(int i) {
        index = i;
    }

    // Change video based on which button was clicked
    public void OnClick() {
        GuideLogic.instance.OnClickChoose(index);
    }
}
