using UnityEngine;
using System.Collections;

public class PuzzlePart : MonoBehaviour {


	public uint Row;
	public uint Column;

	public Game Game;

    private Rigidbody _rb;

    // Use this for initialization
    void Start () {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    public void SetTexture(Texture texture, Vector2 offset, Vector2 tiling)
    {
        GetComponent<Renderer>().material.mainTextureScale = tiling;
        GetComponent<Renderer>().material.mainTextureOffset = offset;

        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material.mainTexture = texture;
    }

    public PuzzleGroup GetPuzzleGroup()
    {
        return gameObject.transform.parent.gameObject.GetComponent<PuzzleGroup>();
    }

    // Update is called once per frame
    void Update () {
    }
}
