
using UnityEngine;
using System.Collections;
using GameSparks.Api.Requests;
using UnityEngine.UI;

public class GameInvite : MonoBehaviour
{
    public inviteAccepter inviteAccept;

    //ChallengeId is the important variable here, we use to reference the specific challenge we are playing
    public string inviteName, inviteExpiry, challengeId;

    public string uniSize, uniDensity;
    public Race challengerRace;

    //We use canDestroy to let the Tween Alpha know it's safe to remove the gameObject OnFinish animating
    public bool canDestroy = false;

    public Text inviteNameLabel, inviteExpiryLabel;

    // Use this for initialization
    void Start()
    {
        inviteNameLabel.text = inviteName + "  has invited you to play";
        inviteExpiryLabel.text = "Expires on " + inviteExpiry;
    }

    public void showAcceptChallenge()
    {
        inviteAccept.gameObject.SetActive(true);
        inviteAccept.setChallengeID(challengeId);
        inviteAccept.setUniverse(uniDensity, uniSize);
        inviteAccept.setChallengerRace(challengerRace);
        inviteAccept.setChallengerName(inviteName);
    }

    public void DeclineChallenge()
    {
        //We set the Challenge Instance Id and Message of DeclineChallengeRequest
        new DeclineChallengeRequest().SetChallengeInstanceId(challengeId)
                .Send((response) =>
                {
                    if (response.HasErrors)
                    {
                        Debug.Log(response.Errors);
                    }
                    else
                    {
                        //Once we decline the challenge we can go ahead and remove the gameObject from the scene
                        Destroy(transform.gameObject);
                    }
                });
    }
}
