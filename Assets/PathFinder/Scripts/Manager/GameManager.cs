using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab; // 플레이어 프리팹을 드래그해서 넣어주세요.
    private Player player; // 실제 씬에 소환된 플레이어 참조

    // 외부에서 Player에 접근할 때 사용할 프로퍼티
    public Player Player
    {
        get
        {
            // 만약 플레이어가 없다면 새로 소환하거나 찾습니다.
            if (player == null)
            {
                InitializePlayer();
            }
            return player;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 시작하자마자 플레이어를 체크합니다.
            InitializePlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayer()
    {
        // 1. 이미 씬에 플레이어가 있는지 먼저 찾아봅니다.
        player = Object.FindAnyObjectByType<Player>();

        // 2. 씬에 플레이어가 없고, 프리팹이 설정되어 있다면 새로 생성합니다.
        if (player == null && playerPrefab != null)
        {
            GameObject go = Instantiate(playerPrefab);
            player = go.GetComponent<Player>();
            Debug.Log("플레이어를 새로 생성했습니다.");
        }
    }
}