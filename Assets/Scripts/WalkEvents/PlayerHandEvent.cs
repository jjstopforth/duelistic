using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerHandEvent : WalkEvent
{
    protected GameObject handAnim;

    public PlayerHandEvent(float _start, float _duration)
        : base(_start, _duration, WalkEventTypes.hatTwirl)
    {
        SetHoldFactors(2, 1, 1.5f);
    }

    public override void StartEvent(PlayerBehaviour _currentPlayer)
    {
        //Do an animation, but only if this is the first one to set one up...
        if (handAnim == null)
        {
            handAnim = GameObject.Instantiate<GameObject>(DuelManagerBehaviour.Instance.handAnim);
            handAnim.transform.SetParent(_currentPlayer.transform);
            handAnim.transform.localPosition = new Vector3(0f, -0.2f, 0.5f);
        }
    }

    public override void EndEvent(PlayerBehaviour _currentPlayer)
    {
        if (handAnim != null)
        {
            GameObject.Destroy(handAnim);
            handAnim = null;
        }
    }
}
