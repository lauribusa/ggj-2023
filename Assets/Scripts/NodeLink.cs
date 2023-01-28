using UnityEngine;
using UnityEngine.Events;

public class NodeLink
{
    public GameNodeUI from;
    public GameNodeUI to;

    public void DrainPower()
    {

    }

    public void Process()
    {
        if(from.CurrentFaction == to.CurrentFaction) 
        {
            GivePower();
            return;
        }
    }

    public void GivePower()
    {

    }
}

