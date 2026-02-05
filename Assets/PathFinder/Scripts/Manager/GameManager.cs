using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    [SerializeField] 
    private GameObject playerPrefab;
    private Player player;

    [Header("Scene")]
    private SceneType curScene;
    private SceneType preScene;

    [Header("Camera Settings")]
    [SerializeField] private GameObject cameraGroupPrefab; 
    private CinemachineVirtualCamera virtualCamera;

    private Dictionary<int, ResistPortal> resistedPortal;
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
        }
        else
        {
            Destroy(gameObject);
        }
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

    public bool ResistPortal(int id, ResistPortal portal)
    {
        if (resistedPortal.ContainsKey(id) || portal == null) return false;

        resistedPortal.Add(id, portal);
        OnResistPortal?.Invoke();
        return true;
    }
}