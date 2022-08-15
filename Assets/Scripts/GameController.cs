using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController iCont;// { get; private set; }
    public GameObject player;
    public static int vida = 3;
    public static bool morto = false;
    public SpriteRenderer Vida1;
    public SpriteRenderer Vida2;
    public SpriteRenderer Vida3;
    public Sprite CoraçãoVazio;

    //Time Related
    public float timer = 0.0f;
    public float lastSpwan = 0.0f;

    //Spwan Related
    public GameObject[] spawanLocations;
    public GameObject[] enemies;
    public List <GameObject> sEnemies;// spwaned enemies, at the moment in the scene
    public int rEnem;
    public int rSpwan;

    private void Awake()
    {
        iCont = iCont is null ? this : iCont;
        morto = false;
        vida = 3;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        print(vida);
        if (vida == 2)
        {
            Vida3.sprite = CoraçãoVazio;
        }
        else if (vida == 1)
        {
            Vida2.sprite = CoraçãoVazio;
        }else if (vida == 0)
        {
            morto = true;
            SceneManager.LoadScene(4);
        }
    }

    static public void TakeDamage()
    {
        vida--;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        timer += Time.deltaTime;
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
    }
}
