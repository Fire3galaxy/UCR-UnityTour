using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuideLogic : MonoBehaviour {
    enum States { TourButtons, VideoButtons };

    static public GuideLogic instance;
    private const string PICK_TOUR = "Pick a Tour",
        PICK_VIDEO = "Pick a Video";
    private const string LEAVE_ROOM = "Leave Room",
        BACK_TO_TOUR = "Back to Tours";

    public GameObject TourGuideCanvas;
    public Text TitleText;
    public Text LeaveButtonText;

    private NetworkVideoControls MasterClientControls; // Passed in by master client when photon room created
    private States state; // To know which OnClickChoose listener to use
    private int currentTour;

    void Start() {
        instance = this;
        state = States.TourButtons;
        currentTour = -1;
    }

    public void SwitchToGuideLogic() {
        TourGuideCanvas.SetActive(true);

        // Add possible tours to list
        ListView listView = TourGuideCanvas.GetComponentInChildren<ListView>();
        List<TourJsonObject> tourList = TourLibrary.instance.tourList;
        for (int i = 0; i < tourList.Count; i++) {
            // Add button to the list
            GameObject tourPlaceButton = listView.AddView();

            // Name and index of tour
            tourPlaceButton.name = tourList[i].name;
            tourPlaceButton.GetComponentInChildren<Text>().text = tourList[i].name;
            tourPlaceButton.GetComponent<TourGuideButton>().setIndex(i);
        }
    }

    public void SwitchToUILogic() {
        TourGuideCanvas.SetActive(false);

        // Remove possible tours (since they're added in SwitchToGuideLogic)
        // I know it's redundant, but I justify it as saying that for room participants, the views
        // are never created.
        ListView listView = TourGuideCanvas.GetComponentInChildren<ListView>();
        listView.DestroyAllViews();
    }

    public void SetVideoControls(NetworkVideoControls controls) {
        MasterClientControls = controls;
    }

    public void OnClickChoose(int index) {
        switch (state) {
            case States.TourButtons:
                // Keep which tour is chosen
                currentTour = index;

                // Remove tours
                ListView listView = TourGuideCanvas.GetComponentInChildren<ListView>();
                listView.DestroyAllViews();

                // Add videos from selected tour
                VideoJsonObject[] videos = TourLibrary.instance.tourList[index].videos;
                for (int i = 0; i < videos.Length; i++) {
                    // Add button to the list
                    GameObject videoButton = listView.AddView();

                    // Name and index of tour
                    videoButton.name = videos[i].name;
                    videoButton.GetComponentInChildren<Text>().text = videos[i].name;
                    videoButton.GetComponent<TourGuideButton>().setIndex(i);
                }

                // Change UI text
                TitleText.text = PICK_VIDEO;
                LeaveButtonText.text = BACK_TO_TOUR;

                // Change state
                state = States.VideoButtons;

                break;
            case States.VideoButtons:
                PhotonView.Get(MasterClientControls)
                    .RPC("ChooseVideo", PhotonTargets.OthersBuffered, currentTour, index);
                break;
        }
    }

    public void OnClickLeave() {
        switch (state) {
            case States.TourButtons:
                // Leave Room
                PhotonNetwork.LeaveRoom();

                // Switch from Tour Guide UI to main menu UI
                GameObject logicObject = GameObject.Find("Game and PUN Logic");
                MainMenuLogic UIlogic = logicObject.GetComponent<MainMenuLogic>();
                GuideLogic TourGuideLogic = logicObject.GetComponent<GuideLogic>();

                UIlogic.BackToHomeScreen();         // Reset Main Menu back to home screen
                TourGuideLogic.SwitchToUILogic();   // Remove Tour Guide menu
                break;
            case States.VideoButtons:
                // Remove videos from list
                ListView listView = TourGuideCanvas.GetComponentInChildren<ListView>();
                listView.DestroyAllViews();

                // Add tours to list
                List<TourJsonObject> tourList = TourLibrary.instance.tourList;
                for (int i = 0; i < tourList.Count; i++) {
                    // Add button to the list
                    GameObject tourPlaceButton = listView.AddView();

                    // Name and index of tour
                    tourPlaceButton.name = tourList[i].name;
                    tourPlaceButton.GetComponentInChildren<Text>().text = tourList[i].name;
                    tourPlaceButton.GetComponent<TourGuideButton>().setIndex(i);
                }

                // Change UI text
                TitleText.text = PICK_TOUR;
                LeaveButtonText.text = LEAVE_ROOM;

                // Change state back to tours
                state = States.TourButtons;
                currentTour = -1;

                break;
        }
    }
}
