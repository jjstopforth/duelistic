using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WalkEventTypes
{
    footStep,
    hatTwirl, //Temporary events yo...
    owlSalute,
    mustacheFridge,
    invertedJosephine
}

public class WalkEvent : IComparable
{
    protected float start, duration, gumptionDelta, mannersDelta, breedingDelta, bottleDelta;
    protected WalkEventTypes walkEvent;

    public WalkEventTypes MyEvent
    {
        get { return walkEvent; }
    }

    public float Start
    {
        get { return start; }
    }

    public float Duration
    {
        get { return duration; }
    }

    public WalkEvent(float _start, float _duration, WalkEventTypes _event, float _gumptionDelta = 0f, float _mannersDelta = 0f, float _breedingDelta = 0f, float _bottleDelta = 0f)
    {
        start = _start;
        duration = _duration;
        walkEvent = _event;
        gumptionDelta = _gumptionDelta;
        mannersDelta = _mannersDelta;
        breedingDelta = _breedingDelta;
        bottleDelta = _bottleDelta;
    }

    public void SetFactors(float _gumptionDelta, float _mannersDelta, float _breedingDelta, float _bottleDelta)
    {
        gumptionDelta = _gumptionDelta;
        mannersDelta = _mannersDelta;
        breedingDelta = _breedingDelta;
        bottleDelta = _bottleDelta;
    }

    public int CompareTo(object _o)
    {
        WalkEvent otherWalkEvent = _o as WalkEvent;

        if (otherWalkEvent != null)
        {
            return start.CompareTo(otherWalkEvent.start);
        }
        return 0;
    }
}
