using System;
using System.Collections.Generic;
using Godot;

public class StructureNode
{
    public Node2D parent;
    public List<Collum> collumsAttached;

    public List<Beamer> beamsAttached;



    public StructureNode(Node2D par)
    {
        parent = par;

        collumsAttached = new List<Collum>();

        beamsAttached = new List<Beamer>();
    }



    //must be called every blank // also the tower does not fall if the base ones are destroyed ngl, allowing for patchy johns, can fix by adding override when the botton node is destoyed or some
    public double getSupport()
    {
        double totalSupport = 0;

        for (int i = 0; i < collumsAttached.Count; i++)
        {

            totalSupport = totalSupport + collumsAttached[i].supportGiven;
        }

        for (int i = 0; i < beamsAttached.Count; i++)
        {

            totalSupport = totalSupport + beamsAttached[i].supportGiven;
        }

        return totalSupport;
    }


    //WILL ALSO add the support that added them
    public void addSupport(StructureNode newNode)
    {
        if (newNode.parent is Collum)
        {

            collumsAttached.Add((Collum)newNode.parent);

        }

        if (newNode.parent is Beamer)
        {

            beamsAttached.Add((Beamer)newNode.parent);
        }

        newNode.addSupportOnCall(this);

    }



    public void addSupportOnCall(StructureNode Attracher)
    {

        if (Attracher.parent is Collum)
        {

            collumsAttached.Add((Collum)Attracher.parent);

        }

        if (Attracher.parent is Beamer)
        {

            beamsAttached.Add((Beamer)Attracher.parent);
        }

    }


    public String getAttachedString()
    {
        String str = "[";

        for (int i = 0; i < collumsAttached.Count; i++)
        {

            str = str + collumsAttached[i].ID + " ";
        }



        for (int i = 0; i < beamsAttached.Count; i++)
        {

            str = str + beamsAttached[i].ID + " ";
        }

        return str + "]";

    }


}