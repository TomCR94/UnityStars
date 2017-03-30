using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[System.Serializable]
public class FleetGameObject : MonoBehaviour {
    
    public GameGameObject game;

    [SerializeField]
    public Fleet fleet;

    public int waypointCount = 0;
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

        if ((fleet.getOwner() != null && fleet.getOwner().getID() == Settings.instance.playerID))
        {
            GetComponentInChildren<UILineRenderer>().enabled = fleet.getOrbiting() == null && fleet.getWaypoints().Count > 1;
            //RenderPath
            if (waypointCount != GetComponentInChildren<UILineRenderer>().Points.Length)
            {
                waypointCount = fleet.getWaypoints().Count;
                Vector2[] points = new Vector2[waypointCount + 1];
                points[0] = Vector2.zero;
                for (int i = 1; i < points.Length; i++)
                {
                    Vector2 pos = new Vector2();
                    pos.x = fleet.getX() - fleet.getWaypoints()[i - 1].getX();
                    pos.x = pos.x * -1;
                    pos.y = fleet.getY() - fleet.getWaypoints()[i - 1].getY();
                    pos.y = pos.y * -1;
                    points[i] = pos;
                }
                GetComponentInChildren<UILineRenderer>().Points = points;
            }
        }

        //Remove
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
