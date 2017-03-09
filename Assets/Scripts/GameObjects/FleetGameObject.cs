using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class FleetGameObject : MonoBehaviour {

    public GameGameObject game;

    [SerializeField]
    public Fleet fleet;

	// Use this for initialization
	void Start () {
		
	}

    public void setFleet(Fleet fleet)
    {
        fleet.FleetGameObject = this;
        this.fleet = fleet;
    }

    public Fleet getFleet()
    {
        return fleet;
    }

    // Update is called once per frame
    private void Update()
    {
        GetComponent<Image>().enabled = GetComponent<Button>().enabled = fleet.getOrbiting() == null;
        
        transform.localPosition = new Vector3(fleet.getX() - game.getGame().getWidth() / 2, fleet.getY() - game.getGame().getHeight() / 2);
        
        if (fleet.isScrapped())
        {
            GameObject.Destroy(gameObject);
        }
    }

    public string toJson()
    {
        return JsonUtility.ToJson(fleet, true);
    }
}
