using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiffMatchPatch;

[ExecuteInEditMode]
public class DiffTest : MonoBehaviour {

    private void Awake()
    {
        diff_match_patch dmp = new diff_match_patch();
        List<Patch> patchs = dmp.patch_make("The cow is in the road", "The chicken is crossing the road");
        Debug.Log("Patch: " + patchs[0].ToString());
    }

    // Update is called once per frame
    void Update () {
		
	}
}
