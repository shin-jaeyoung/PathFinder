using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    [SerializeField]
    private GameObject playerPrefab;
    private Player player;

    [Header("Scene State")]
    [SerializeField]
    private SceneType curScene;
    private SceneType preScene;

    [Header("Camera Settings")]
    [SerializeField] private GameObject cameraGroupPrefab;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confiner;

    [Header("Sound Settings")]
    [SerializeField]
    private List<BGMData> bgmList;
    [SerializeField]
    private AudioMixer mainMixer;
    private SoundManager soundManager;
    private AudioSource bgmSource;

    [Header("Save & Load")]
    [SerializeField] 
    private SaveManager saveManager;

    public Dictionary<int, PortalData> resistedPortal = new Dictionary<int, PortalData>();
    public event Action OnResistPortal;

    // properties
    public Player Player => player;
    public SceneType CurScene => curScene;
    public SceneType PreScene => preScene;
    public SoundManager SoundManager => soundManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SetupSoundSystem();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupSoundSystem()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        if (mainMixer != null)
        {
            var groups = mainMixer.FindMatchingGroups("BGM");
            if (groups.Length > 0)
            {
                bgmSource.outputAudioMixerGroup = groups[0];
            }
        }
        soundManager = new SoundManager();
        soundManager.Init(bgmSource, bgmList, mainMixer);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void StartGame(bool isLoad)
    {
        InitializePlayer();
        InitializeCamera();

        if (isLoad)
        {
            LoadGame();
            SetScene(SceneType.Town);
            SceneManager.LoadScene(SceneType.Town.ToString());
            player.transform.position = Vector3.zero;
        }
        else
        {
            SetScene(SceneType.Town);
            SceneManager.LoadScene(SceneType.Town.ToString());

            HiddenManager.instance.InitializeHiddens();
            player.transform.position = Vector3.zero;
        }
    }

    private void InitializePlayer()
    {
        // 이미 존재한다면 중복 생성 방지
        if (player != null) return;

        player = UnityEngine.Object.FindAnyObjectByType<Player>();

        if (player == null && playerPrefab != null)
        {
            GameObject go = Instantiate(playerPrefab);
            go.name = "Player";
            player = go.GetComponent<Player>();
            if (player != null) player.enabled = true;
            player.FullInit();
            player.PlayerController.ControllerInit();
            DontDestroyOnLoad(player);
            Debug.Log("GameManager: 플레이어 객체를 생성했습니다.");
        }
    }

    private void InitializeCamera()
    {
        if (virtualCamera != null) return;

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

    // 세이브로드

    public void SaveGame()
    {
        if (saveManager == null) saveManager = GetComponent<SaveManager>();
        if (player == null) return;

        SaveData data = player.GetSaveData();

        data.savedPortals = new List<PortalData>(resistedPortal.Values);
        HiddenManager.instance.Save(data);

        saveManager.SaveToFile(data);
        GlobalEvents.Notify("게임이 저장되었습니다.");
    }

    public void LoadGame()
    {
        if (saveManager == null) saveManager = GetComponent<SaveManager>();

        SaveData data = saveManager.GetLoadedData();
        if (data != null)
        {
            player.LoadGame(data);

            resistedPortal.Clear();
            if (data.savedPortals != null)
            {
                foreach (PortalData pData in data.savedPortals)
                {
                    resistedPortal.Add(pData.ID, pData);
                }
            }
            HiddenManager.instance.Load(data);
            OnResistPortal?.Invoke();
            player.Inventory.OnEquipmentChanged?.Invoke();

            Debug.Log("GameManager: 포탈 데이터를 포함한 모든 데이터를 복구했습니다.");
        }
    }


    public void SetScene(SceneType scene)
    {
        preScene = curScene;
        curScene = scene;
        soundManager.OnSceneLoaded(curScene);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        // 카메라 컨파이너 재설정
        if (virtualCamera != null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();

            GameObject boundary = GameObject.FindGameObjectWithTag("CameraBoundery");
            if (boundary != null && confiner != null)
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
        return resistedPortal.ContainsKey(id);
    }
    public void MovePlayer(Vector3 arrival)
    {
        if (player != null)
            player.transform.position = arrival;
    }
}