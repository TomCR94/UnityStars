using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinimumSlider : Slider
{
    [SerializeField]
	public MaximumSlider other;

	protected override void Set(float input, bool sendCallback)
	{
        other = gameObject.transform.parent.GetComponentInChildren<MaximumSlider>();
		float newValue = input;

		if (wholeNumbers)
		{
			newValue = Mathf.Round(newValue);
		}

		if (newValue > other.value-10)
		{
			return;
		}

		base.Set(input, sendCallback);
	}
}
