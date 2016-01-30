using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WalkFootstepEvent : WalkEvent
{
    
    public WalkFootstepEvent(float _start, float _duration)
        : base(_start, _duration, WalkEventTypes.footStep)
    {
        SetTapFactors(Random.Range(10f, 15f), 0f, 0f, 0f);
    }

    public override void StartEvent(PlayerBehaviour _currentPlayer)
    {
        //Do a footstep thing? Like, play a sound? Why isn't this using Unity's drag and drop?
        if (DuelManagerBehaviour.Instance.footstepAnim != null)
        {
            GameObject step = (GameObject)GameObject.Instantiate(DuelManagerBehaviour.Instance.footstepAnim, _currentPlayer.transform.position, Quaternion.identity);
        }
    }
}
