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
        player = Object.FindAnyObjectByType<Player>();

        if (player == null && playerPrefab != null)
        {
            GameObject go = Instantiate(playerPrefab);

            player = go.GetComponent<Player>();
            Debug.Log("플레이어를 새로 생성했습니다.");
        }
    }
    public void SetScene(SceneType scene)
    {
        preScene = curScene;
        curScene = scene;
    }
}