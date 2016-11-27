using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleGroup : MonoBehaviour {

    private bool isFixed = false;

    public List<PuzzlePart> Parts
    {
        get
        {
            List<PuzzlePart> results = new List<PuzzlePart>();
            gameObject.transform.GetComponentsInChildren<PuzzlePart>(results);
            return results;
        }
    }

    public uint MinRow
    {
        get
        {
            uint mr = 0;
            foreach(PuzzlePart part in Parts)
            {
                if(part.Row < mr)
                {
                    mr = part.Row;
                }
            }

            return mr;
        }
    }

    public uint MaxRow
    {
        get
        {
            uint mr = 0;
            foreach (PuzzlePart part in Parts)
            {
                if (part.Row > mr)
                {
                    mr = part.Row;
                }
            }

            return mr;
        }
    }

    public uint MinColumn
    {
        get
        {
            uint mc = 0;
            foreach (PuzzlePart part in Parts)
            {
                if (part.Column < mc)
                {
                    mc = part.Column;
                }
            }

            return mc;
        }
    }

    public uint MaxColumn
    {
        get
        {
            uint mc = 0;
            foreach (PuzzlePart part in Parts)
            {
                if (part.Column > mc)
                {
                    mc = part.Column;
                }
            }

            return mc;
        }
    }


    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
        if (isFixed)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
	}

    //public void Join(PuzzleGroup other)
    //{
    //    List<PuzzlePart> otherChildren = other.Parts;
    //    foreach (PuzzlePart part in otherChildren)
    //    {
    //        part.gameObject.transform.parent = gameObject.transform;
    //    }
    //}

    public void AddPart(PuzzlePart part)
    {
        part.gameObject.transform.parent = gameObject.transform;
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();

        if(collider == null)
        {
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().convex = true;
            collider = gameObject.GetComponent<MeshCollider>();
        }

        MeshCollider partCollider = part.gameObject.GetComponent<MeshCollider>();

        if (collider.sharedMesh == null)
        {
            collider.sharedMesh = partCollider.sharedMesh;
        }

        //CombineInstance[] combines = new CombineInstance[2];
        //combines[0].mesh = collider.sharedMesh;
        //combines[1].mesh = partCollider.sharedMesh;
        //collider.sharedMesh.CombineMeshes(combines);
    }

    public void SetMass(int mass)
    {
        gameObject.GetComponent<Rigidbody>().mass = mass;
    }

    public void Fix()
    {
        SetMass(1);
        isFixed = true;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void Unfix()
    {
        SetMass(0);
        isFixed = false;
    }

    private void RecalculatePartPositions()
    {

    }
}
