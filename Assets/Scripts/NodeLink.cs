using UnityEngine;
using UnityEngine.Events;

public class NodeLink
{
    public GameNodeUI from;
    public GameNodeUI to;

    public NodeLink(GameNodeUI from, GameNodeUI to)
    {
        this.from = from;
        this.to = to;
        from.isLinked = true;
        to.isLinked = true;
    }

    public void DestroyLink()
    {
        from.isLinked = false;
        to.isLinked = false;
    }

    public void DrainPower(GameNodeUI from, GameNodeUI to)
    {
        from.NodeValue -= GameManager.Instance.globalDrainRate;
        if(from.NodeValue <= 0 )
        {
            FromNodeIsDepleted();
        }
        to.NodeValue -= GameManager.Instance.globalDrainRate;
        if(from.NodeValue <= 0)
        {
            ToNodeIsDepleted();
        }
    }

    public void Process()
    {
        if(from.CurrentFaction == to.CurrentFaction) 
        {
            GivePower(from, to);
            return;
        }
        DrainPower(from, to);
    }

    public void GivePower(GameNodeUI from, GameNodeUI to)
    {
        from.NodeValue -= GameManager.Instance.globalDrainRate;
        to.NodeValue += GameManager.Instance.globalDrainRate;
    }

    public void FromNodeIsDepleted()
    {

    }

    public void ToNodeIsDepleted()
    {

    }
}

