using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MovingObject
{
    
    public abstract List<SubObject> AsList();
    public abstract void SetNextCoordinates(Vector2Int v); 
    
}
public class SubObject : MovingObject
{
    public GameObject obj;
    public bool isTeleport;
    public bool isClone;
    public Vector2Int nextCoordinates;
    public Vector2Int blockCoordinates;

    public SubObject(GameObject b, Vector2Int v)
    {
        obj = b;
        blockCoordinates = v;
        nextCoordinates = v;
        isTeleport = false;
        isClone = false;

    }
    public override void SetNextCoordinates(Vector2Int v)
    {
        nextCoordinates = blockCoordinates + v;
    }
    public void CheckForTeleporter(Vector2Int v)
    {
        //checks for teleporter
        if(isTeleport)
        {     
            Debug.Log(v);
            Debug.Log("teleporting");
            CloneManager2.Teleport(v, obj);
            isTeleport = false; 
        }
        
    }   
    public override List<SubObject> AsList()
    {
        List<SubObject> myList = new List<SubObject>();
        myList.Add(this);
        return myList;
    }

}
public class ObjController : MovingObject
{
    public bool isMoving;
    public bool isRotating;
    public bool isDisabled;
    public Vector2Int nextMove;
    public Vector3 rotation;
    public List<MovingObject> subList;
    

    public ObjController(bool c, GameObject b, Vector2Int v)
    {
        isMoving = false;
        isRotating = false;
        isDisabled = false;
        subList = new List<MovingObject>();
        subList.Add(new SubObject(b,v));
        (subList[0] as SubObject).isClone = c;
    }
    public ObjController()
    {
        isMoving = false;
        isRotating = false;
        subList = new List<MovingObject>();
    }
    public void ChangeLevelComponents(int n)
    {
        SubObject c = this.GetCloneObject();

        foreach(SubObject b in this.AsList())
        {
            if(b.isClone && b!=c)
            {
                b.isClone = false;
                Debug.Log("clone rejected");
            }
            if(!b.isClone)
            {
                if(n==0)
                {
                    CloneManager2.cBoxs.Remove(b.blockCoordinates);
                }
                else
                {
                    Debug.Log("young midoryia!");
                    CloneManager2.cBoxs.Add(b.blockCoordinates, b);
                }
                CloneManager2.cLevelComponents[GridBuilder.stageHeight+1-b.blockCoordinates.y, b.blockCoordinates.x+1] = n;
            }
        }
    }
    public void UpdateCoordinates()
    {
        
        foreach(SubObject b in this.AsList())
        {
            /*
            if(!b.isClone)
            {
                //CloneManager2.cBoxs.Remove(b.blockCoordinates);
                CloneManager2.cBoxs.Add(b.nextCoordinates, b);
                ///CloneManager2.cLevelComponents[11-b.blockCoordinates.y, b.blockCoordinates.x+1] = 0;
                CloneManager2.cLevelComponents[11-b.nextCoordinates.y, b.nextCoordinates.x+1] = 1;
            }
            */
        
            b.blockCoordinates = b.nextCoordinates;

            b.CheckForTeleporter(b.blockCoordinates);
            
        }

        this.ChangeLevelComponents(1);

    }
    public SubObject GetCloneObject()
    {
        List<SubObject> l = this.AsList();
        l.Reverse();
        foreach(SubObject b in l)
        {
            if(b.isClone)
            {
                return b;
            }
        }
        return null;
    }
    public void Rotate(Vector3 r)
    {
        rotation = r;
        isRotating = true;
    }
    public override void SetNextCoordinates(Vector2Int v)
    {
         nextMove = v;
         this.GetCloneObject().SetNextCoordinates(v);
    }
    public List<SubObject> AsMainList()
    {
        if(subList[0] is ObjController)
        {
            return (subList[1] as ObjController).AsList();
        }
        else
        {
            return this.AsList();
        }
    }
    public override List<SubObject> AsList()
    {
        List<SubObject> myList = new List<SubObject>();
        
        foreach(MovingObject b in subList)
        {
            myList.AddRange(b.AsList());
        }
        return myList;
    }
    
}


