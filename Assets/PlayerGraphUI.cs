using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGraphUI : MonoBehaviour
{
    public PlayerBehaviour PlayerBehaviour;

    public RectTransform GumptionBar;
    public RectTransform MannersBar;
    public RectTransform BreedingBar;
    public RectTransform BottleBar;

    private float _maxSize = 250f;
    public float LerpScale = 0.2f;

	// Use this for initialization
	void Start () 
    {
	    if (GumptionBar != null)
        {
            _maxSize = GumptionBar.sizeDelta.x;
	    }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    if (PlayerBehaviour != null)
	    {
	        UpdateBar(GumptionBar, PlayerBehaviour.gumption);
	        UpdateBar(MannersBar, PlayerBehaviour.manners);
	        UpdateBar(BreedingBar, PlayerBehaviour.breeding);
	        UpdateBar(BottleBar, PlayerBehaviour.bottle);
	    }
    }

    void UpdateBar(RectTransform bar, float value)
    {
        if (bar != null)
        {
            Vector2 vec = bar.sizeDelta;
            if (Mathf.Approximately(PlayerBehaviour.maxFactorValue, 0f))
            {
                vec.x = 0f;
            }
            else
            {

                vec.x = _maxSize * (Mathf.Clamp(value, 0f, PlayerBehaviour.maxFactorValue)/PlayerBehaviour.maxFactorValue);
            }
            bar.sizeDelta = Vector2.Lerp(bar.sizeDelta, vec, LerpScale);
        }
    }
}
