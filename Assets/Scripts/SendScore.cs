using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LootLocker.Requests;



public class SendScore : MonoBehaviour
{
    public GameObject holder;
    public TMP_InputField name;
    public TextMeshProUGUI resposta;
    int LeaderBoardID = 5520;
    public TMP_Text PlayersNames;
    public TMP_Text PlayerScores;


    bool _nameSubmited = false;
    bool setuped = false;
    // Start is called before the first frame update
    void Start()
    {
        holder = GameObject.Find("Holder");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Enviar()
    {
        //name.text;
        Debug.Log("Enviou");
        //name.text;
        Debug.Log("Enviou");
        StartCoroutine(LoginRoutine());

        /*
         * 
        if (!setuped)
        {
            StartCoroutine(Setup());
            setuped = true;
        }
        else if (_nameSubmited)
        {
            StartCoroutine(ScoreRoutine());
        }
        */
        //StartCoroutine(Setup());
        //StartCoroutine(ScoreRoutine());

        resposta.enabled = true;

        resposta.enabled = true;

    }

    public IEnumerator Setup()
    {
        yield return LoginRoutine();
    }
    public IEnumerator ScoreRoutine()
    {
        yield return SubmitScoreRoutine();
        yield return FechTopHighScoresRoutine();
    }
    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(name.text,
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
                for (var i = 0; i < members.Length; i++)
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
                Debug.Log("Error" + response.Error);
                done = true;
            }
        }
        LootLockerSDKManager.GetScoreListMain(LeaderBoardID, 10, 0, responseCallback);
        yield return new WaitUntil(() => !done);
    }
    /*
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
    */

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PLayerId", response.player_id.ToString());
                done = true;
            }

            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }


    public IEnumerator SubmitScoreRoutine()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, score: holder.GetComponent<Holder>().scoreCopy, leaderboardId: LeaderBoardID,
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
}
