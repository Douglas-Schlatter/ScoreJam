using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
using UnityEngine.UI;
using TMPro;
public class GameController : MonoBehaviour
{
    public static GameController iCont { get; private set; }
    public GameObject player;
    public int vida = 3;
    public static bool morto = false;
    public SpriteRenderer Vida1;
    public SpriteRenderer Vida2;
    public SpriteRenderer Vida3;
    public Sprite CoraçãoVazio;
    public PlayerController pc;

    int LeaderBoardID = 5520;
    public TMP_Text PlayersNames;
    public TMP_Text PlayerScores;
    public TMP_Text CurrentScore;
    public TMP_InputField playerNameInputsField;    

    //score related
    public int score;
    public int multplier;
    public float testHit;

    bool _nameSubmited = false;
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

    bool setuped = false;

    private void Awake()
    {
        iCont = iCont is null ? this : iCont;
        morto = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        vida = pc.life;
    }

    // Update is called once per frame
    void Update()
    {
        print(vida);
        vida = pc.life;
        if (vida == 2)
        {
            Vida3.sprite = CoraçãoVazio;
        }
        else if (vida == 1)
        {
            Vida2.sprite = CoraçãoVazio;
        }
        else if (vida <= 0)
        {
            morto = true;
            CurrentScore.text = "Your Score: " + score;
            Camera.main.transform.position = new Vector3(1000,1000,1000);
            if(!setuped)
            {   
                StartCoroutine(Setup());
                setuped = true;
            }
            else if(_nameSubmited)
            {
                StartCoroutine(ScoreRoutine());
            }
            //TODO> LOAD CREDITS
        }
    }
    public IEnumerator Setup()
    {
        CurrentScore.gameObject.SetActive(true);
        playerNameInputsField.gameObject.SetActive(true);
        yield return LoginRoutine();
    }    
    public IEnumerator ScoreRoutine()
    {
        CurrentScore.gameObject.SetActive(false);
        playerNameInputsField.gameObject.SetActive(false);
        PlayerScores.gameObject.SetActive(true);
        PlayersNames.gameObject.SetActive(true);
        yield return SubmitScoreRoutine();
        yield return FechTopHighScoresRoutine();
    }
    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputsField.text,
            response =>
            {
                if (response.success)
                {
                    Debug.Log("Player name set");
                    _nameSubmited = true;
                }
                else
                {
                    Debug.Log($"Did not Set Player Name {response.Error}");
                    _nameSubmited = true;
                }
            }
        );
    }
    IEnumerator FechTopHighScoresRoutine()
    {
        bool done = false;
        void responseCallback(LootLockerGetScoreListResponse response)
        {
            if (response.success)
            {
                string names = "Names\n";
                string scores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;
                for (var i = 0; i<members.Length; i++)
                {
                    names += members[i].rank + ". ";
                    print(members[i].player.name);
                    if (members[i].player.name != "")
                    {
                        names += members[i].player.name;
                    }
                    else
                    {
                        names += members[i].player.id;
                    }
                    scores += members[i].score + "\n";
                    names += "\n";
                }
                done = true;
                PlayersNames.text = names;
                PlayerScores.text = scores;
            }
            else
            {
                Debug.Log("Error"  +  response.Error);
                done = true;
            }
        }
        LootLockerSDKManager.GetScoreListMain(LeaderBoardID, 10, 0, responseCallback);
        yield return new WaitUntil(() => !done);
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession(resposne =>
        {
            if (resposne.success)
            {
                Debug.Log("Login Successful");
                PlayerPrefs.SetString("PlayerID", $"{resposne.player_id}");
                done = true;
            }
            else
            {
                Debug.Log("Login Failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => !done);
    }
    public IEnumerator SubmitScoreRoutine()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, score: score, leaderboardId: LeaderBoardID,
            resposne =>
            {
                if (resposne.success)
                {
                    Debug.Log("Score Submitted");
                    done = true;
                }
                else
                {
                    Debug.Log("Score Submission Failed" + resposne.Error);
                    done = true;
                }
            }
        );
        yield return new WaitWhile(() => !done);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        lastHitPlayer = pc.lastHitSnap;
        genMult();

        switch (multplier)
        {

            case 1:
            SpawnByTime(6);
            break;
            case 2:
            SpawnByTime(7);
            break;
            case 4:
            SpawnByTime(8);
            break;
            case 6:
            SpawnByTime(9);
            break;
            case 10:
            SpawnByTime(10);
            break;

            default:
            SpawnByTime(6);
            break;
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

    void SpawnByTime(int max)
    {
        if (max > sEnemies.Count)
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
