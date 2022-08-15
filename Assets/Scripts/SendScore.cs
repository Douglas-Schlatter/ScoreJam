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

        resposta.enabled = true;

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
}
