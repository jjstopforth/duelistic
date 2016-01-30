using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerHatEvent : WalkEvent
{
    protected GameObject hatAnim;

    public PlayerHatEvent(float _start, float _duration)
        : base(_start, _duration, WalkEventTypes.hatTwirl)
    {
        SetHoldFactors(0, 3, 1.5f);
    }

    public override void StartEvent(PlayerBehaviour _currentPlayer)
    {
        //Do an animation, but only if this is the first one to set one up...
        if (hatAnim == null)
        {
            hatAnim = GameObject.Instantiate<GameObject>(DuelManagerBehaviour.Instance.hatAnim);
            hatAnim.transform.SetParent(_currentPlayer.transform);
            hatAnim.transform.localPosition = new Vector3(0f, 1f, 0.5f);
        }
    }

    public override void EndEvent(PlayerBehaviour _currentPlayer)
    {
        if (hatAnim != null)
        {
            GameObject.Destroy(hatAnim);
            hatAnim = null;
        }
    }
}
