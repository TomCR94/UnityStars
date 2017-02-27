using UnityEngine;
using System.Collections;

public class Minimise : MonoBehaviour {

    public GameObject Canvas;
    bool minimised = false;
    Vector3 scale;

    void Start()
    {
        scale = Canvas.transform.localScale;
    }

    void Update()
    {
        minimise();
    }

    public void invert()
    {
        minimised = !minimised;
    }

    public void minimise()
    {
        if (minimised)
        {
            Canvas.transform.localScale = Vector3.Lerp(Canvas.transform.localScale, Vector3.zero, 0.5f);
        }
        else
            Canvas.transform.localScale = Vector3.Lerp(Canvas.transform.localScale, scale, 0.5f);

    }
}
