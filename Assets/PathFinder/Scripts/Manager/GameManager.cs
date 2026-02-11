using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    [SerializeField] 
    private GameObject playerPrefab;
    private Player player;

    [Header("Scene")]
    [SerializeField]
    private SceneType curScene;
    private SceneType preScene;

    [Header("Camera Settings")]
    [SerializeField] private GameObject cameraGroupPrefab; 
    private CinemachineVirtualCamera virtualCamera;

    private CinemachineConfiner2D confiner;

    public Dictionary<int, PortalData> resistedPortal = new Dictionary<int, PortalData>();
    public event Action OnResistPortal;
    // property

    public Player Player
    {
        get
        {
            if (player == null)
            {
                InitializePlayer();
            }
            return player;
        }
    }
    public SceneType CurScene => curScene;
    public SceneType PreScene => preScene;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayer();
            InitializeCamera();
            InitialSceneSetting();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void InitialSceneSetting()
    {
        curScene = SceneType.GameStart;
        preScene = SceneType.GameStart;
    }
    private void InitializePlayer()
    {
        player = UnityEngine.Object.FindAnyObjectByType<Player>();

        if (player == null && playerPrefab != null)
        {
            GameObject go = Instantiate(playerPrefab);
            go.name = "Player";
            player = go.GetComponent<Player>();
            DontDestroyOnLoad(player);
            Debug.Log("플레이어를 새로 생성했습니다.");
        }
    }
    private void InitializeCamera()
    {
        virtualCamera = UnityEngine.Object.FindAnyObjectByType<CinemachineVirtualCamera>();

        if (virtualCamera == null && cameraGroupPrefab != null)
        {
            GameObject camGo = Instantiate(cameraGroupPrefab);
            camGo.name = "CameraGroup";
            virtualCamera = camGo.GetComponentInChildren<CinemachineVirtualCamera>();
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
            DontDestroyOnLoad(camGo);
        }

        if (virtualCamera != null && player != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
    }
    public void SetScene(SceneType scene)
    {
        preScene = curScene;
        curScene = scene;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (confiner == null && virtualCamera != null)
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();

        if (confiner != null)
        {
            GameObject boundary = GameObject.FindGameObjectWithTag("CameraBoundery");
            if (boundary != null)
            {
                confiner.m_BoundingShape2D = boundary.GetComponent<PolygonCollider2D>();
                confiner.InvalidateCache();
            }
        }
    }

    
    public bool ResistPortal(int id, ResistPortal portal)
    {
        if (resistedPortal.ContainsKey(id) || portal == null) return false;

        resistedPortal.Add(id, new PortalData(portal));
        OnResistPortal?.Invoke();
        return true;
    }
    public bool isResisPortal(int id)
    {
        if (resistedPortal.ContainsKey(id)) 
        { 
            return true; 
        }
        return false;
    }
    public void MovePlayer(Vector3 arrival)
    {
        player.transform.position = arrival;
    }
}