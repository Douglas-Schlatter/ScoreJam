using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using TMPro;


public class GameController : MonoBehaviour
{
    public static GameController iCont { get; private set; }

    public GameObject player;
    public GameObject holder;
    public int vida = 3;
    public static bool morto = false;
    public SpriteRenderer Vida1;
    public SpriteRenderer Vida2;
    public SpriteRenderer Vida3;
    public Sprite CoraçãoCheio;
    public Sprite CoraçãoVazio;
    public PlayerController pc;
    public AstarPath aPath;

    //score related
    public int score;
    public int multplier;
    public float testHit;
    public TextMeshProUGUI ScoreValue;
    public TextMeshProUGUI mulValue;

    //Time Related
    public float timer = 0.0f;
    public float lastSpwan = 0.0f;
    public float lastHitPlayer;


    //Spwan Related
    public GameObject[] spawanLocations;
    public GameObject[] enemies;
    public List <GameObject> sEnemies;// spwaned enemies, at the moment in the scene
    public int rEnem;
    public int rSpwan;

    //scene loader
    //public static bool carregou;
    private void Awake()
    {
        iCont = iCont is null ? this : iCont;
        morto = false;
        vida = pc.life;
        //DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //carregou = false;
        //morto = false;
        // vida = pc.life;
        // aPath.Scan();
        //Pegar sem precisar referencia
        iCont = GameObject.Find("GameController").GetComponent<GameController>();
        holder = GameObject.Find("Holder");
        player =  GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //print(vida);
        vida = pc.life;
        if( vida == 3)
        {
            Vida3.sprite = CoraçãoCheio;
            Vida2.sprite = CoraçãoCheio;
        }
        if (vida == 2)
        {
            Vida3.sprite = CoraçãoVazio;
            Vida2.sprite = CoraçãoCheio;
        }
        else if (vida == 1)
        {
            Vida2.sprite = CoraçãoVazio;
        }else if (vida <= 0)
        {
            morto = true;
            holder.GetComponent<Holder>().scoreCopy = score;
            SceneManager.LoadScene(5);
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        lastHitPlayer = pc.lastHitSnap;
        genMult();
        if (vida > 0)
        {
            switch (multplier)
            {

                case 1:
                    SpawnByTime(6, 2.0,2);
                    break;
                case 2:
                    SpawnByTime(7,1.7,2);
                    break;
                case 4:
                    SpawnByTime(8,1.3,2);
                    break;
                case 6:
                    SpawnByTime(9,1.0,3);
                    break;
                case 10:
                    SpawnByTime(10,0.5,3);
                    break;

                default:
                    SpawnByTime(6, 2.0,2);
                    break;
            }
        }

        /*
         if (6 > sEnemies.Count) 
         {
             if ((timer - lastSpwan) > 2.0)
             {
                 rSpwan = Random.Range(0, spawanLocations.Length);
                 rEnem = Random.Range(0, enemies.Length);
                 GameObject test = (GameObject)Instantiate(enemies[rEnem], spawanLocations[rSpwan].transform.position, spawanLocations[rSpwan].transform.rotation);
                 //st.TryGetComponent(Pathfinding.AIDestinationSetter)
                 sEnemies.Add(test);
                 lastSpwan = timer;
             }
         }
        */
    }
    private void LateUpdate()
    {
        mulValue.text = multplier.ToString();
        ScoreValue.text = score.ToString();
    }

    void SpawnByTime(int max,double dif,int hp)
    {
        if (max > sEnemies.Count)
        {
            if ((timer - lastSpwan) > dif)
            {
                rSpwan = Random.Range(0, spawanLocations.Length);
                rEnem = Random.Range(0, enemies.Length);
                GameObject test = (GameObject)Instantiate(enemies[rEnem], spawanLocations[rSpwan].transform.position, spawanLocations[rSpwan].transform.rotation);
                test.GetComponent<AIDestinationSetter>().target = player.transform;
                //st.TryGetComponent(Pathfinding.AIDestinationSetter)
                test.GetComponent<BaseEnemyController>().health = hp;
                sEnemies.Add(test);
                lastSpwan = timer;
            }
        }
    }
    public void genMult()
    {
        testHit = (timer - lastHitPlayer);
        if ((0 < testHit) && (testHit <= 10))
        {
            multplier = 1;
        }
        else if ((10 < testHit) && (testHit <= 15))
        {
            multplier = 2;
        }
        else if ((15 < testHit) && (testHit <= 20))
        {
            multplier = 4;
        }
        else if ((20 < testHit) && (testHit <= 30))
        {
            multplier = 6;
        }
        else if (testHit > 30)
        {
            multplier = 10;
        }
    }
    public void GiveScore(int addScore)
    {

        genMult();
        score += addScore * multplier;
           
 
    }
}
