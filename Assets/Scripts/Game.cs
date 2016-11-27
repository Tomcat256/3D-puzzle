using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public uint RowsCount;
	public uint ColumnsCount;
    public string TextureName = null;

	public GameObject PuzzlePartPrefab;
	public GameObject Desktop;
	public GameObject PlaceholderPrefab;
	public Camera MainCamera;

	private GameObject[,] _parts;
    private List<GameObject> _groups;
	private GameObject[,] _placeholders;

    private TextureManager _textureManager;
    private LocationManager _locationManager;
    private Texture _currentTexture;

	private GameObject _draggingObject;

	// Use this for initialization
	void Start () {
		_draggingObject = null;
        _locationManager = new LocationManager(RowsCount, ColumnsCount);
        LoadContext();
        InitTextures();
		CreateParts ();
		SetupPlaceholders ();
		ApplyInitialLayout ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ApplyInitialLayout(){
		List<Vector3> possiblePositions = _locationManager.PrepareInitialPositions ();

        foreach(GameObject goGroup in _groups)
        {
            int positionIndex = Random.Range(0, possiblePositions.Count - 1);
            Vector3 position = possiblePositions[positionIndex];

            goGroup.transform.localPosition = position;
            goGroup.transform.Rotate(LocationManager.RandomRotation());

            possiblePositions.RemoveAt(positionIndex);
        }
    }

	public void ApplyFinalLayout(){
		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				_parts [row, column].transform.localPosition = _locationManager.GetLocationFor(row, column);
			}
	}

    public void OnRotate()
    {
        GameObject goGroup = GetPuzzleGroupUnderCursor();

        if (goGroup == null)
            return;
        goGroup.GetComponent<PuzzleGroup>().Unfix();
        goGroup.transform.Rotate(Vector3.forward * 90);
        goGroup.GetComponent<PuzzleGroup>().Fix();
    }

    public void OnDragBegin()
    {
        GameObject group = GetPuzzleGroupUnderCursor();

        if (group == null)
            return;

        group.GetComponent<PuzzleGroup>().Unfix();
        _draggingObject = group;
    }

    public void OnDragEnd()
    {
        if (!_draggingObject)
            return;

        PuzzleGroup group = _draggingObject.GetComponent<PuzzleGroup>();

        _draggingObject = null;

        stickPartsToLocation(group);
        group.GetComponent<PuzzleGroup>().Fix();
    }

    public void OnDrag()
    {
        doDrag();
    }

    private void LoadContext()
    {
        RowsCount = GlobalContext.Instance.RowsCount;
        ColumnsCount = GlobalContext.Instance.ColumnsCount;
        TextureName = GlobalContext.Instance.TextureName;
    }

    private void InitTextures()
    {
        _textureManager = new TextureManager();
        _currentTexture = _textureManager.GetTexture(TextureName);
    }

	private void CreateParts(){
		_parts = new GameObject[RowsCount, ColumnsCount];
        _groups = new List<GameObject>((int)(RowsCount * ColumnsCount));

        for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {

                GameObject part = (GameObject)Instantiate (PuzzlePartPrefab);

                GameObject group = new GameObject("Group");
                group.transform.parent = gameObject.transform;
                group.AddComponent<PuzzleGroup>();
                group.GetComponent<PuzzleGroup>().AddPart(part.GetComponent<PuzzlePart>());
                group.GetComponent<PuzzleGroup>().SetMass(100);

                part.GetComponent<PuzzlePart>().Row = row;
				part.GetComponent<PuzzlePart>().Column = column;
				part.GetComponent<PuzzlePart> ().Game = this;
                part.GetComponent<PuzzlePart>().SetTexture(_currentTexture, 
                                                            TextureManager.CalculateTextureOffset(row, column, RowsCount, ColumnsCount), 
                                                            TextureManager.CalculateTextureTiling(RowsCount, ColumnsCount));
                _parts [row, column] = part;
                _groups.Add(group);
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

	private GameObject GetPuzzleGroupUnderCursor(){
		GameObject target = GetObjectUnderCursor ();
		if (!target)
			return null;

        if (target.GetComponent<PuzzleGroup>())
        {
            return target;
        }

        if (target.GetComponent<PuzzlePart>())
        {
            GameObject goGroup = target.transform.parent.gameObject;
            if (!goGroup.GetComponent<PuzzleGroup>())
                return null;

            return goGroup;
        }

        return null;
	}

	private Vector3 GetLocalMousePosition(){
		Ray ray = MainCamera.ScreenPointToRay (Input.mousePosition);
		Plane plane = new Plane (Vector3.up, Vector3.zero);
		float dist;
		plane.Raycast (ray, out dist);
		return ray.GetPoint (dist);
	}

	private void stickPartsToLocation(PuzzleGroup group)
    {
        foreach (PuzzlePart part in group.Parts)
        {
            Vector3? stickLocation = _locationManager.GetStickLocation(part);

            if (stickLocation == null)
                continue;

            group.gameObject.transform.position = (Vector3)stickLocation;
            group.gameObject.transform.eulerAngles = LocationManager.NormalizedRotation(part.gameObject.transform.eulerAngles);

            group.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            group.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
	}

	private void SetupPlaceholders(){
		_placeholders = new GameObject[RowsCount, ColumnsCount];

		for (uint row = 0; row < RowsCount; row++)
			for (uint column = 0; column < ColumnsCount; column++) {
				GameObject placeholder = (GameObject)Instantiate (PlaceholderPrefab);
				placeholder.transform.parent = gameObject.transform;
				_placeholders [row, column] = placeholder;
				_placeholders [row, column].transform.localPosition = _locationManager.GetLocationFor(row, column);
			}
	}

}
