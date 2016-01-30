using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RabbitEvent : WalkEvent
{
    static GameObject rabbitAnim = null;

    public RabbitEvent(float _start, float _duration)
        : base(_start, _duration, WalkEventTypes.wind)
    {
        SetMashFactors(Random.Range(3f, 5f), Random.Range(3f, 5f), Random.Range(3f, 5f), Random.Range(3f, 5f));
        float negativeMash = Random.Range(-4f, -3f);

        switch (Mathf.RoundToInt(Random.Range(0.5f, 4.499f)))
        {
            case 1: gumptionMashDelta = negativeMash; break;
            case 2: mannersMashDelta = negativeMash; break;
            case 3: breedingMashDelta = negativeMash; break;
            case 4: bottleMashDelta = negativeMash; break;
        }
    }

    public override void StartEvent(PlayerBehaviour _currentPlayer)
    {
        //Do an animation, but only if this is the first one to set one up...
        if (rabbitAnim == null)
        {
            rabbitAnim = GameObject.Instantiate<GameObject>(DuelManagerBehaviour.Instance.rabbitAnim);
            rabbitAnim.transform.position = new Vector3(0f, Random.Range(-2f, -3f), 0f);
        }
    }

    public override void EndEvent(PlayerBehaviour _currentPlayer)
    {
        if (rabbitAnim != null)
        {
            GameObject.Destroy(rabbitAnim);
            rabbitAnim = null;
        }
    }
}
