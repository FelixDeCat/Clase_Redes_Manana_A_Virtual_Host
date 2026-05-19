using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public static NetworkUI instance;

    [SerializeField] Button host_Button;
    [SerializeField] Button client_Button;

    [SerializeField] RunnerHandler handler;

    [SerializeField] CanvasGroup waitingPanel;

    [Header("Connection Panel")]
    [SerializeField] CanvasGroup connectionPanel;

    [Header("Host Side")]
    [SerializeField] GameObject hostCreationPanel;
    [SerializeField] TMP_InputField ifield_sessionName;
    [SerializeField] Button btn_createSession;
    string sessionName = string.Empty;

    [Header("Join Session Lobby")]
    [SerializeField] GameObject sessionLobbyPanel;
    [SerializeField] TMP_InputField ifield_sessionLobby;
    [SerializeField] Button btn_sessionLobby;
    string lobbyName = string.Empty;

    [Header("Client Side")]
    [SerializeField] CanvasGroup sessionListGroup;
    [SerializeField] SessionListUI sessionList;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        /// Eleccion de Host o Cliente
        host_Button.onClick.AddListener(OnHostModeSelected);
        client_Button.onClick.AddListener(OnClientSelected);

        // Lobby
        sessionLobbyPanel.gameObject.SetActive(true);
        btn_sessionLobby.onClick.AddListener(Btn_OnClick_SessionLobby);
        ifield_sessionLobby.onValueChanged.AddListener(InputField_SessionLobby);
        lobbyName = "GenericLobby";

        // Host
        ifield_sessionName.onValueChanged.AddListener(InputField_HostNameChanged);
        btn_createSession.onClick.AddListener(Btn_OnClick_CreateSession);
        sessionName = "quickGame";

        // Client
        handler.SubscribeToSessionLisUpdate(sessionList.OnListUpdate);
    }


    public void BeginLoad()
    {
        waitingPanel.alpha = 1;
        waitingPanel.blocksRaycasts = true;

        connectionPanel.alpha = 0;
        connectionPanel.blocksRaycasts = false;
    }

    public void EndLoad()
    {
        waitingPanel.alpha = 0;
        waitingPanel.blocksRaycasts = false;

        connectionPanel.alpha = 1;
        connectionPanel.blocksRaycasts = true;
    }


    /////////////// 
    /// Session Lobby
    /// 
    void Btn_OnClick_SessionLobby()
    {
        handler.JoinLobby(lobbyName, OnConnectedToLobby);
        sessionLobbyPanel.gameObject.SetActive(false);
        BeginLoad();
    }
    void OnConnectedToLobby()
    {
        /// sonidos
        /// animacion ,etc
        EndLoad();
    }

    void InputField_SessionLobby(string newValue)
    {
        lobbyName = newValue;
    }

    /////////////
    /// Host
    /// 
    void OnHostModeSelected()
    {
        hostCreationPanel.gameObject.SetActive(true);
    }
    private void Btn_OnClick_CreateSession()
    {
        int pvpGame = 1;
        //int pveGame = 2;
        hostCreationPanel.gameObject.SetActive(false);
        handler.HostOnCreateSession(sessionName, pvpGame, onFinishHostCreation);
    }

    void onFinishHostCreation(string resultMessage)
    {

    }

    private void InputField_HostNameChanged(string newValue)
    {
        sessionName = newValue;
    }

    ///Client
    ///
    void OnClientSelected()
    {
        sessionListGroup.alpha = 1;
        sessionListGroup.blocksRaycasts = true;
    }

}
