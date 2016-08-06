using UnityEngine;
using System.Collections;

public class PuzzlePart : MonoBehaviour {


	public uint Row;
	public uint Column;

	public Game Game;

	// Use this for initialization
	void Start () {
		MeshRenderer renderer = gameObject.GetComponent<MeshRenderer> ();
		Vector2 offset = new Vector2 ((float)Column / (float)Game.ColumnsCount, (float)Row / (float)Game.RowsCount);
		Vector2 tiling = new Vector2 (1F / (float)Game.ColumnsCount, 1F / (float)Game.RowsCount);
		renderer.material.mainTextureScale = tiling;
		renderer.material.mainTextureOffset = offset;
	}

    public void SetTexture(Texture texture)
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material.mainTexture = texture;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
