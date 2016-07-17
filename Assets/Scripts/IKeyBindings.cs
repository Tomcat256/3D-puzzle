using UnityEngine;
using System.Collections;

public interface IKeyBindings {

	bool MoveCameraLeftPressed ();
	bool MoveCameraRightPressed ();
	bool MoveCameraUpPressed ();
	bool MoveCameraDownPressed ();

	bool ZoomInPressed ();
	bool ZoomOutPressed ();

	bool Dragging ();
	bool RotatePresed ();
}
