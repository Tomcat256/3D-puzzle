using UnityEngine;
using System.Collections;

public class KeyboardKeyBinding : MonoBehaviour{

	public float SecondsBeforeDragStarts;

	private float _lastMouseDownTS;
	private bool _isDragging;
	private GameObject _listener;

	public void Start(){
		_isDragging = false;
		_lastMouseDownTS = -1;
		_listener = gameObject;
	}

	public void Update(){
		CheckDraggingCases ();
		CheckAllEvents ();
	}

	private bool CheckMoveCameraLeft (){
		return 
			Input.GetKey (KeyCode.LeftArrow) ||
			(Input.GetKey (KeyCode.Mouse1) && Input.GetAxis("Mouse X") > 0);
	}

	private bool CheckMoveCameraRight (){
		return Input.GetKey (KeyCode.RightArrow) ||
			(Input.GetKey (KeyCode.Mouse1) && Input.GetAxis("Mouse X") < 0);
	}

	private bool CheckMoveCameraUp (){
		return Input.GetKey (KeyCode.UpArrow) ||
			(Input.GetKey (KeyCode.Mouse1) && Input.GetAxis("Mouse Y") < 0);
	}

	private bool CheckMoveCameraDown (){
		return Input.GetKey (KeyCode.DownArrow) ||
			(Input.GetKey (KeyCode.Mouse1) && Input.GetAxis("Mouse Y") > 0);
	}

	private bool CheckZoomIn (){
		return Input.mouseScrollDelta.y > 0;
	}

	private bool CheckZoomOut (){
		return Input.mouseScrollDelta.y < 0;
	}

	private void NotifyListeners(string message){
		_listener.SendMessage (message, null, SendMessageOptions.DontRequireReceiver);
	}

	private void CheckAllEvents (){
		if (CheckMoveCameraLeft())
			NotifyListeners ("OnMoveCameraLeft");

		if (CheckMoveCameraRight())
			NotifyListeners ("OnMoveCameraRight");

		if (CheckMoveCameraUp())
			NotifyListeners ("OnMoveCameraUp");

		if (CheckMoveCameraDown())
			NotifyListeners ("OnMoveCameraDown");

		if (CheckZoomIn())
			NotifyListeners ("OnCameraZoomIn");

		if (CheckZoomOut())
			NotifyListeners ("OnCameraZoomOut");
	}

	private void CheckDraggingCases(){
		if (_lastMouseDownTS < 0) {
			if (Input.GetKey(KeyCode.Mouse0)) {
				_lastMouseDownTS = Time.unscaledTime;
			}
		}
		else {
			float timeDiff = Time.unscaledTime - _lastMouseDownTS;

			if (!Input.GetKey(KeyCode.Mouse0)) {
				_lastMouseDownTS = -1;

				if (timeDiff < SecondsBeforeDragStarts) {
					NotifyListeners ("OnRotate");
				}
				else {
					_isDragging = false;
					NotifyListeners ("OnDragEnd");
				}
			}
			else {
				if (timeDiff >= SecondsBeforeDragStarts) {
					if (!_isDragging) {
						_isDragging = true;
						NotifyListeners ("OnDragBegin");
					}
					else {
						NotifyListeners ("OnDrag");
					}
				}
			}

			if (timeDiff < SecondsBeforeDragStarts) {

			}
		}
	}
}
