using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class MainMenuLogic : Photon.MonoBehaviour {
    enum States { HomeScreen, CreateRoomScreen, JoinRoomScreen, EnterRoomScreen, TourParticipantScreen, TourGuideScreen };

    static public MainMenuLogic instance;
    public float Version = 1.0f;

    private const string QUIT = "Quit"; // Quit button strings
    private const string BACK = "Back";

    // Objects that are used for all user screens
    public GameObject UICanvas;
    public Text QuitButtonText;

    // Objects that indicate Loading
    public Text LoadingText;
    public Image LoadingImage;
    public Sprite LoadedSprite;
    public Sprite NotLoadedSprite;

    // Different User Screens
    public GameObject HomeScreen;
    public GameObject CreateRoomScreen;
    public GameObject JoinRoomScreen;
    public GameObject EnterRoomScreen;
    
    private States GameState = States.HomeScreen;

    void Start() {
        instance = this;    // For easy access by other classes. Only one instance ever exists, right?

        VRSettings.enabled = false;
        Screen.orientation = ScreenOrientation.Portrait;
    }
    
	public virtual void Update ()
    {
        if (!PhotonNetwork.connecting && !PhotonNetwork.connected) {
            Debug.Log("In update: connecting");
            PhotonNetwork.ConnectUsingSettings(Version.ToString("N1")); // 1 place after decimal
        }
    }

    // Occurs at HomeScreen, after connected to Photon in Update()
    public virtual void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        LoadingText.text = "In Lobby!";
        LoadingImage.sprite = LoadedSprite;
        HomeScreen.SetActive(true);
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause) {
        Debug.LogError("OnFailedToConnectToPhoton(): " + cause.ToString());
    }

    public void OnConnectionFail(DisconnectCause cause) {
        Debug.LogError("OnConnectionFail(): " + cause.ToString());
    }

    /* Changes text and sprite after room is joined. You join a room either by 
     * creating one (from CreateRoomScreen) or joining one (from JoinRoomScreen). 
     */
    public virtual void OnJoinedRoom() {
        switch (GameState) {
            // Go to Tour Participant UI
            case States.CreateRoomScreen:
                UICanvas.SetActive(false);
                GameState = States.TourGuideScreen;
                GetComponent<GuideLogic>().SwitchToGuideLogic();
                break;
            // Go to EnterRoomScreen
            case States.JoinRoomScreen:
                EnterRoomScreen.GetComponentInChildren<Text>().text = "Joined room!\n<size=40>"
                    + PhotonNetwork.room.name + "</size>";
                LoadingImage.sprite = LoadedSprite;
                EnterRoomScreen.GetComponentInChildren<Button>(true).gameObject.SetActive(true);
                GameState = States.EnterRoomScreen;
                break;
        }
    }

    // For JoinRoomScreen. Keeps the list of open rooms updated.
    public virtual void OnReceivedRoomListUpdate() {
        if (GameState == States.JoinRoomScreen) {
            DeleteRoomList();
            PopulateRoomList();
        }
    }

    // Used whenever we return to the main menu. Resets UI to HomeScreen.
    public void BackToHomeScreen() {
        // Add home page and loading screen
        HomeScreen.SetActive(true);
        LoadingImage.gameObject.SetActive(true);
        LoadingText.gameObject.SetActive(true);

        // Remove anything else & reset values
        CreateRoomScreen.SetActive(false);
        JoinRoomScreen.SetActive(false);
        EnterRoomScreen.SetActive(false);
        EnterRoomScreen.GetComponentInChildren<Button>(true).gameObject.SetActive(false);

        if (PhotonNetwork.insideLobby)
            LoadingImage.sprite = LoadedSprite;
        else
            LoadingImage.sprite = NotLoadedSprite;
        
        DeleteRoomList();           // Join Room Screen
        QuitButtonText.text = QUIT; // Home Screen

        UICanvas.SetActive(true);   // Now that all UI is reset, make UI visible

        // Update state
        GameState = States.HomeScreen;
    }

    // Used in OnReceivedRoomListUpdate by JoinRoomScreen to delete room views
    private void DeleteRoomList() {
        ListView listView = JoinRoomScreen.GetComponentInChildren<ListView>();
        listView.DestroyAllViews();
    }

    // Used in OnReceivedRoomListUpdate by JoinRoomScreen to add updated rooms
    private void PopulateRoomList() {
        ListView listView = JoinRoomScreen.GetComponentInChildren<ListView>();

        // Add rooms to list
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms) {
            // Add item to listview
            GameObject roomItem = listView.AddView();

            // Edit item name
            roomItem.name = room.name;
            roomItem.GetComponentInChildren<Text>().text = room.name;
        }
    }

    // ALL ONCLICK LISTENER FUNCTIONS FOR BUTTONS
    public void OnClickQuit() {
        switch (GameState) {
            case States.HomeScreen:
                QuitApplication.Quit();
                break;
            default:
                BackToHomeScreen();
                break;
        }
    }

    // Transition from HomeScreen to CreateRoomScreen
    public void OnClickCreateRoom() {
        // Remove home page and loading screen
        HomeScreen.SetActive(false);
        LoadingImage.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);

        // Add in create room menu
        CreateRoomScreen.SetActive(true);

        // Update state
        GameState = States.CreateRoomScreen;
        QuitButtonText.text = BACK;
    }

    // CreateRoomScreen to EnterRoomScreen (if room created successfully. See onJoinedRoom)
    public void OnClickSubmitRoom() {
        // Views in CreateRoomScreen
        InputField roomInputField = CreateRoomScreen
            .GetComponentInChildren<InputField>();
        Text enterRoomText = EnterRoomScreen.GetComponentInChildren<Text>();

        // Change interface for enter room state
        CreateRoomScreen.SetActive(false);      // Create Room Screen state
        LoadingImage.sprite = NotLoadedSprite;  // Enter Room Screen state
        LoadingImage.gameObject.SetActive(true);
        EnterRoomScreen.SetActive(true);

        // Create room with name given by user
        string roomName = roomInputField.text;
        enterRoomText.text = "Creating room:\n<size=40>" + roomName + "</size>";
        PhotonNetwork.CreateRoom(roomName);
    }

    // HomeScreen to JoinRoomScreen
    public void OnClickJoinRoom() {
        Debug.Log("Clicked Join Room");

        // Remove home page and loading screen
        HomeScreen.SetActive(false);
        LoadingImage.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);

        // Add in join room menu
        JoinRoomScreen.SetActive(true);

        // Update state
        GameState = States.JoinRoomScreen;
        QuitButtonText.text = BACK;

        PopulateRoomList();
    }

    // JoinRoomScreen to EnterRoomScreen (if joined room successfully. See onJoinedRoom)
    public void OnClickRoomItem(string roomName) {
        Debug.Log("Clicked on: " + roomName);

        // Change interface for enter room state
        JoinRoomScreen.SetActive(false);        // Join Room Screen state
        LoadingImage.sprite = NotLoadedSprite;  // Enter Room Screen state
        LoadingImage.gameObject.SetActive(true);
        EnterRoomScreen.SetActive(true);
        EnterRoomScreen.GetComponentInChildren<Text>().text =
            "Joining room:\n<size=40>" + roomName + "</size>";

        PhotonNetwork.JoinRoom(roomName);
    }

    // TourParticipantScreen to VideoLogic (separate file)
    public void OnClickToRoom() {
        Debug.Log("To the room now!");

        GameState = States.TourParticipantScreen;
        UICanvas.SetActive(false);
        GetComponent<ParticipantLogic>().SwitchToTourLogic();
    }
}
