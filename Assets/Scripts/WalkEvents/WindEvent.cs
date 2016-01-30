using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WindEvent : WalkEvent
{
    static GameObject windAnim = null;

    public WindEvent(float _start, float _duration)
        : base(_start, _duration, WalkEventTypes.wind)
    {
        SetMashFactors(Random.Range(-1f, 3f), Random.Range(-1f, 3f), Random.Range(-1f, 3f), Random.Range(-1f, 3f));
    }

    public override void StartEvent(PlayerBehaviour _currentPlayer)
    {
        //Do an animation, but only if this is the first one to set one up...
        if (windAnim == null)
        {
            windAnim = GameObject.Instantiate<GameObject>(DuelManagerBehaviour.Instance.windAnim);
            windAnim.transform.position = new Vector3(Random.Range(-6f, 6f), Random.Range(0f, 2f), 0f);
        }
    }

    public override void EndEvent(PlayerBehaviour _currentPlayer)
    {
        if (windAnim != null)
        {
            GameObject.Destroy(windAnim);
            windAnim = null;
        }
    }
}
