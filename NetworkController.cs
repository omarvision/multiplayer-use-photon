using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    //Purpose:
    //  this is the multiplayer game connect screen. The jobs of this scene are...
    //  1. connect to photon server
    //  2. create, join game room
    //  3. load scene

    public Text txtStatus = null;
    public GameObject btnStart = null;
    public GameObject btnCancel = null;
    public byte MaxPlayers = 4;
    public int SceneIndex = 1;

    private void Start()
    {
        Status(string.Format("Settings = {0}", PhotonNetwork.PhotonServerSettings));
        PhotonNetwork.ConnectUsingSettings();

        Status(string.Format("Connecting to server {0}...", PhotonNetwork.ServerAddress));
        btnStart.SetActive(false);
        btnCancel.SetActive(false);
    }

    #region --- overrides ---
    public override void OnConnectedToMaster()
    {
        //Settings located at:  Assets > Photon > PhotonUnityNetworking > Resources
        base.OnConnectedToMaster();        
        PhotonNetwork.AutomaticallySyncScene = true; //all players same scene
        Status(string.Format("Connected to server {0}", PhotonNetwork.ServerAddress));
        btnStart.SetActive(true);
        btnCancel.SetActive(false);
    }    
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Status(string.Format("Created room {0}", PhotonNetwork.CurrentRoom.Name));
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Status(string.Format("Create room failed, returnCode:{0}, message:{1}", returnCode, message));
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Status(string.Format("Joined room {0}, Master:{1}", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.IsMasterClient));
        SceneManager.LoadScene("Level");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Status(string.Format("Join room failed, returncode:{0}, message:{1}", returnCode, message));
    }
    
    #endregion

    public void btnStart_Click()
    {
        string roomname = "Room1"; // "Room" + Random.Range(0, 1000).ToString();

        RoomOptions roomoptions = new RoomOptions();
        roomoptions.IsOpen = true;
        roomoptions.IsVisible = true;
        roomoptions.MaxPlayers = MaxPlayers;

        PhotonNetwork.JoinOrCreateRoom(roomname, roomoptions, TypedLobby.Default);

        txtStatus.text = "Joining or Creating room...";
        btnStart.SetActive(false);
        btnCancel.SetActive(true);        
    }
    public void btnCancel_Click()
    {
        PhotonNetwork.LeaveRoom();

        txtStatus.text = "Connected to server";
        btnCancel.SetActive(false);
        btnStart.SetActive(true);
    }
    private void Status(string msg)
    {
        Debug.Log(msg);
        txtStatus.text = msg;
    }
}
