using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public float MovementSpeed;
	public float ZoomSpeed;

	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera = (Camera)GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnMoveCameraLeft(){
		_camera.transform.position += new Vector3 (-MovementSpeed, 0, 0);
	}

	public void OnMoveCameraRight(){
		_camera.transform.position += new Vector3 (MovementSpeed, 0, 0);
	}

	public void OnMoveCameraUp(){
		_camera.transform.position += new Vector3 (0, 0, MovementSpeed);
	}

	public void OnMoveCameraDown(){
		_camera.transform.position += new Vector3 (0, 0, -MovementSpeed);
	}

	public void OnCameraZoomIn(){
		_camera.transform.position += new Vector3 (0, -ZoomSpeed, 0);
	}

	public void OnCameraZoomOut(){
		_camera.transform.position += new Vector3 (0, ZoomSpeed, 0);
	}
}
