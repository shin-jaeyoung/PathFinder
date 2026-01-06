using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    private Player player; 

    // 외부에서 Player에 접근할 때 사용할 프로퍼티
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayer();
        }
        else
        {
            Destroy(gameObject);
        }
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
}