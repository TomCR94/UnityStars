
using UnityEngine;
using GameSparks.Api.Requests;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Ionic.Zip;
using System.IO;

public class ChallengeGame : MonoBehaviour
{
    public PopulateGames populateGames;
    [Space]
    //ChallengeId is the important variable here, we use to reference the specific challenge we are playing
    public string challengeId, challenged, challenger, NextPlayer;
    public Text inviteNameLabel, gameStatus;
    public Button playButton;
    public List<string> names;

    // Use this for initialization
    void Start()
    {
        Debug.Log("names length: " + names.Count);
        inviteNameLabel.text = challenger + " VS " + challenged;

        if (GameSparksManager.getInstance().getPlayerID() == NextPlayer)
        {
            gameStatus.text = "Your turn";
            playButton.interactable = true;
        }
        else
        {
            gameStatus.text = "Waiting for other player to take turn";
            playButton.interactable = false;
        }

        new GetChallengeRequest()
            .SetChallengeInstanceId(challengeId)
            .Send((response) => {
                NextPlayer = response.Challenge.NextPlayer;
            });

    }

    private void Update()
    {
        if (GameSparksManager.getInstance().getPlayerID() == NextPlayer)
        {
            gameStatus.text = "Your turn";
            playButton.interactable = true;
        }
        else
        {
            gameStatus.text = "Waiting for other player to take turn";
            playButton.interactable = false;
        }
    }

    //This in the function we call OnClick
    public void PlayChallenge()
    {
        new GetChallengeRequest()
            .SetChallengeInstanceId(challengeId)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Response GD: " + response.Challenge.ScriptData.GetString("GD"));
                    Debug.Log("Response s: " + response.Challenge.ScriptData.GetString("s"));
                    Debug.Log("Response d: " + response.Challenge.ScriptData.GetString("d"));
                    Debug.Log("Response pl1: " + response.Challenge.Challenger.Name);
                    Debug.Log("Response cR1: " + response.Challenge.ScriptData.GetString("cR1"));
                    foreach(GameSparks.Api.Responses.GetChallengeResponse._Challenge._PlayerDetail det in response.Challenge.Challenged)
                    Debug.Log("Response pl2: " + det.Name);
                    Debug.Log("Response cR2: " + response.Challenge.ScriptData.GetString("cR2"));
                    if (!string.IsNullOrEmpty(response.Challenge.ScriptData.GetString("GD")))
                    {/*
                         * Gets data from the challenge object and creates a game from it 
                         */
                        NewGameInit.instance.setGameName(challengeId);
                        NewGameInit.instance.setDensity(int.Parse(response.Challenge.ScriptData.GetString("d")));
                        NewGameInit.instance.setSize(int.Parse(response.Challenge.ScriptData.GetString("s")));
                        NewGameInit.instance.setPlayerRace(JsonUtility.FromJson<Race>(response.Challenge.ScriptData.GetString("cR1")), 0);
                        NewGameInit.instance.setPlayerRace(JsonUtility.FromJson<Race>(response.Challenge.ScriptData.GetString("cR2")), 1);
                        NewGameInit.instance.setPlayerName(response.Challenge.Challenger.Name, 0);
                        foreach (GameSparks.Api.Responses.GetChallengeResponse._Challenge._PlayerDetail det in response.Challenge.Challenged)
                            NewGameInit.instance.setPlayerName(det.Name, 1);

                        DownloadAFile(response.Challenge.ScriptData.GetString("GD"));


                    }
                    else
                    {

                        /*
                         * Gets data from the challenge object and creates a game from it 
                         */
                        NewGameInit.instance.setGameName(challengeId);
                        NewGameInit.instance.setDensity(int.Parse(response.Challenge.ScriptData.GetString("d")));
                        NewGameInit.instance.setSize(int.Parse(response.Challenge.ScriptData.GetString("s")));
                        NewGameInit.instance.setPlayerRace(JsonUtility.FromJson<Race>(response.Challenge.ScriptData.GetString("cR1")), 0);
                        NewGameInit.instance.setPlayerRace(JsonUtility.FromJson<Race>(response.Challenge.ScriptData.GetString("cR2")), 1);
                        NewGameInit.instance.setPlayerName(response.Challenge.Challenger.Name, 0);
                        foreach (GameSparks.Api.Responses.GetChallengeResponse._Challenge._PlayerDetail det in response.Challenge.Challenged)
                            NewGameInit.instance.setPlayerName(det.Name, 1);

                        NewGameInit.instance.loadOnlineGame();
                    }
                }
            });
    }

    public void ConcedeChallenge()
    {
        
    }

    //When we want to download our uploaded image
    public void DownloadAFile(string uploadID)
    {
        //Get the url associated with the uploadId
        new GetUploadedRequest().SetUploadId(uploadID).Send((response) =>
        {
            //pass the url to our coroutine that will accept the data
            StartCoroutine(DownloadFile(response.Url));
        });
    }


    public IEnumerator DownloadFile(string downloadUrl)
    {
        WWW www = new WWW(downloadUrl);

        yield return www;

        byte[] bytes = www.bytes;

        string path = Application.persistentDataPath + "/Game/serverFile.zip";

        if (new FileInfo(path).Exists)
        {
            new FileInfo(path).Delete();
        }

        File.WriteAllBytes(path, bytes);

        if (new DirectoryInfo(Application.persistentDataPath + "/Game/serverFile").Exists)
        {
            new DirectoryInfo(Application.persistentDataPath + "/Game/serverFile").Delete(true);
        }

        ZipFile zip = ZipFile.Read(path);
        zip.ExtractAll(Application.persistentDataPath + "/Game/serverFile");

        NewGameInit.instance.setLoading(true);
        NewGameInit.instance.loadOnlineGame();
    }
}
