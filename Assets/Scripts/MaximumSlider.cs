using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaximumSlider : Slider
{
    [SerializeField]
	public MinimumSlider other;

	protected override void Set(float input, bool sendCallback)
    {
        other = gameObject.transform.parent.GetComponentInChildren<MinimumSlider>();
        float newValue = input;

		if (wholeNumbers)
		{
			newValue = Mathf.Round(newValue);
		}

		if (newValue < other.value+10)
		{
			return;
		}

		base.Set(input, sendCallback);
	}
}
