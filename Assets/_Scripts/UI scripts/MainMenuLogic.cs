using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuLogic : Photon.MonoBehaviour {
    enum States { HomeScreen, CreateRoomScreen, JoinRoomScreen, EnterRoomScreen, TourScreen };

    private const string QUIT = "Quit"; // Quit button strings
    private const string BACK = "Back";

    public byte Version = 1;

    // Quit/Back button
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
    public GameObject ListButtonPrefab;

    private bool ConnectedInUpdate = false;
    private States GameState = States.HomeScreen;
	
	// Update is called once per frame
	public virtual void Update ()
    {
        if (!ConnectedInUpdate) {
            Debug.Log("In update");
            PhotonNetwork.ConnectUsingSettings(Version + ".0");
            ConnectedInUpdate = true;
        }
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        LoadingText.text = "In Lobby!";
        LoadingImage.sprite = LoadedSprite;
        HomeScreen.SetActive(true);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause) {
        Debug.LogError("OnFailedToConnectToPhoton(): " + cause.ToString());
    }

    public virtual void OnJoinedRoom() {
        switch (GameState) {
            case States.CreateRoomScreen:
                EnterRoomScreen.GetComponentInChildren<Text>().text = "Room created!\n<size=40>"
                    + PhotonNetwork.room.name + "</size>";
                break;
            case States.JoinRoomScreen:
                EnterRoomScreen.GetComponentInChildren<Text>().text = "Joined room!\n<size=40>"
                    + PhotonNetwork.room.name + "</size>";
                break;
        }

        LoadingImage.sprite = LoadedSprite;
        EnterRoomScreen.GetComponentInChildren<Button>(true).gameObject.SetActive(true);
        GameState = States.EnterRoomScreen;
    }

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

    public void BackToHomeScreen() {
        // Add home page and loading screen
        HomeScreen.SetActive(true);
        LoadingImage.gameObject.SetActive(true);
        LoadingText.gameObject.SetActive(true);

        // Remove anything else
        CreateRoomScreen.SetActive(false);
        JoinRoomScreen.SetActive(false);
        EnterRoomScreen.SetActive(false);

        // Update state
        GameState = States.HomeScreen;
        QuitButtonText.text = QUIT;
    }

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
    
    public void OnClickToRoom() {
        Debug.Log("To the room now!");
        GameState = States.TourScreen;
        GetComponent<TourLogic>().SwitchToTourLogic();
    }

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

    public virtual void OnReceivedRoomListUpdate() {
        if (GameState == States.JoinRoomScreen) {
            DeleteRoomList();
            PopulateRoomList();
        }
    }

    private void DeleteRoomList() {
        RoomNameHolder[] listedRooms = 
            JoinRoomScreen.GetComponentsInChildren<RoomNameHolder>();
        foreach (RoomNameHolder room in listedRooms)
            Destroy(room.gameObject);
    }

    private void PopulateRoomList() {
        // List Views needed
        VerticalLayoutGroup ListContent = JoinRoomScreen
            .GetComponentInChildren<VerticalLayoutGroup>();

        // Add rooms to list
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms) {
            GameObject roomItem = (GameObject) 
                Instantiate(ListButtonPrefab, ListContent.transform);
            RoomNameHolder itemLogic = roomItem.GetComponent<RoomNameHolder>();
            itemLogic.roomName = room.name;
            itemLogic.gameLogic = this;
            roomItem.name = room.name;
            roomItem.GetComponentInChildren<Text>().text = room.name;
            roomItem.GetComponent<Button>().onClick.AddListener(itemLogic.OnClickRoomItem);
        }
    }
}
