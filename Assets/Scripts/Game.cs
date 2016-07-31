using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public uint RowsCount;
	public uint ColumnsCount;

	public GameObject PuzzlePartPrefab;
	public GameObject Desktop;
	public GameObject PlaceholderPrefab;
	public Camera MainCamera;

	private GameObject[,] _parts;
	private GameObject[,] _placeholders;
	private float _centersHeight = 0.6F;
    private float _gapBetweenParts = 0.03F;
	private Vector3[,] _centers;
    private TextureManager _textureManager;
    private Texture _currentTexture;

	private GameObject _draggingObject;

	// Use this for initialization
	void Start () {
		_draggingObject = null;
		CreateCenters ();
        InitTextures();
		CreateParts ();
		SetupPlaceholders ();
		ApplyInitialLayout ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ApplyInitialLayout(){
		List<Vector3> possiblePositions = PrepareInitialPositions ();

		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				int positionIndex = Random.Range (0, possiblePositions.Count - 1);
				Vector3 position = possiblePositions [positionIndex];
				possiblePositions.RemoveAt (positionIndex);
				_parts [row, column].transform.localPosition = position;
				_parts [row, column].transform.Rotate (new Vector3 (0, 0, Random.Range (0, 4) * 90));
			}
	}

	public void ApplyFinalLayout(){
		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				_parts [row, column].transform.localPosition = _centers [row, column];
			}
	}

    public void OnRotate()
    {
        GameObject part = GetPuzzlePartUnderCursor();

        if (part == null)
            return;

        part.transform.Rotate(new Vector3(0, 0, 90));
    }

    public void OnDragBegin()
    {
        GameObject part = GetPuzzlePartUnderCursor();

        if (part == null)
            return;

        _draggingObject = part;
        _draggingObject.GetComponent<Rigidbody>().mass = 0;
    }

    public void OnDragEnd()
    {
        if (!_draggingObject)
            return;

        _draggingObject.GetComponent<Rigidbody>().mass = 100;

        locatePart(_draggingObject);

        _draggingObject = null;
    }

    public void OnDrag()
    {
        doDrag();
    }

    private void InitTextures()
    {
        _textureManager = new TextureManager();
        _currentTexture = _textureManager.GetRandomTexture();
    }

    private List<Vector3> PrepareInitialPositions()
    {
        uint initialColumnsCount = ColumnsCount * 5;
        uint initialRowsCount = RowsCount / 2;

        Vector2 topLeft = new Vector2(((int)ColumnsCount - initialColumnsCount) / 2, RowsCount + 2);
        Vector2 bottomRight = new Vector2((ColumnsCount + initialColumnsCount) / 2, RowsCount + initialRowsCount + 2);

        List<Vector3> positions = new List<Vector3>();

        for (float z = topLeft.y; z <= bottomRight.y; z++)
            for (float x = topLeft.x; x <= bottomRight.x; x++)
            {
                positions.Add(new Vector3(x, _centersHeight + Random.value * 3, z));
            }

        return positions;
    }

    private void CreateCenters(){
		_centers = new Vector3[RowsCount, ColumnsCount];

		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				_centers [row, column] = new Vector3 (1.03F * column, _centersHeight, (1F + _gapBetweenParts) * row);
			}		
	}

	private void CreateParts(){
		_parts = new GameObject[RowsCount, ColumnsCount];

		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				GameObject part = (GameObject)Instantiate (PuzzlePartPrefab);
				part.transform.parent = gameObject.transform;
				part.GetComponent<PuzzlePart>().Row = row;
				part.GetComponent<PuzzlePart>().Column = column;
				part.GetComponent<PuzzlePart> ().Game = this;
                part.GetComponent<PuzzlePart>().SetTexture(_currentTexture);
                _parts [row, column] = part;
			}
	}

	private void doDrag(){
		if (!_draggingObject)
			return;

		Vector3 newPosition = GetLocalMousePosition();
		_draggingObject.transform.position = new Vector3(newPosition.x, _draggingObject.transform.position.y, newPosition.z);
	}

	private GameObject GetObjectUnderCursor(){
		RaycastHit hit;
		Physics.Raycast (MainCamera.ScreenPointToRay (Input.mousePosition), out hit);
		if ((object)hit == null) {
			return null;
		}
		return hit.collider.gameObject;
	}

	private GameObject GetPuzzlePartUnderCursor(){
		GameObject go = GetObjectUnderCursor ();
		if (!go) {
			return null;
		}

		if(go.GetComponent<PuzzlePart> ()){
			return go;
		}

		return null;
	}

	private Vector3 GetLocalMousePosition(){
		Ray ray = MainCamera.ScreenPointToRay (Input.mousePosition);
		Plane plane = new Plane (new Vector3 (0, 1, 0), new Vector3 (0, 0, 0));
		float dist;
		plane.Raycast (ray, out dist);
		return ray.GetPoint (dist);
	}

	private void locatePart(GameObject part){
		Vector2 partPos = new Vector2 (part.transform.position.x, part.transform.position.z);

		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				Vector2 centerPos = new Vector2(_centers [row, column].x, _centers [row, column].z);
				if (Vector2.Distance (partPos, centerPos) < 0.5F * part.transform.localScale.x) {
					part.transform.position = _centers [row, column];
					part.transform.eulerAngles = new Vector3 (0, 0, Mathf.Round (part.transform.eulerAngles.z / 90) * 90);
					return;
				}
					
			}
	}

	private void SetupPlaceholders(){
		_placeholders = new GameObject[RowsCount, ColumnsCount];

		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				GameObject placeholder = (GameObject)Instantiate (PlaceholderPrefab);
				placeholder.transform.parent = gameObject.transform;
				_placeholders [row, column] = placeholder;
				_placeholders [row, column].transform.localPosition = _centers [row, column];
			}
	}

}
