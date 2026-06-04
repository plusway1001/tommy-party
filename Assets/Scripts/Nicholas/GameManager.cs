using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    [SerializeField] private int fps;

    [SerializeField] private GameObject starterBulletPrefab;
    [SerializeField] private Sprite coinIcon;

    public int Currency { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        WeaponsList.Init(starterBulletPrefab);
        LootList.Init(coinIcon);

        Application.targetFrameRate = fps;
        //Cursor.visible = false;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }
}
