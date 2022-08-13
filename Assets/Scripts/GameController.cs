using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController iCont;// { get; private set; }
    public GameObject player;



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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


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
