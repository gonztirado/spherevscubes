using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IACameraController : CameraController
{

	[Header ("IA Settings")]
	public bool enableIA;
	public float checkChangeCameraMinInterval = 2;
	public float checkChangeCameraMaxInterval = 5;


	private float _nextHorizontalCameraChange;
	private float _moveHorizontalCamera;

	protected override void CheckCameraRotation()
	{
		if (enableIA)
			CheckCameraRotationByIA();
		else
			base.CheckCameraRotation();
	}

	private void CheckCameraRotationByIA()
	{
		_nextHorizontalCameraChange -= Time.deltaTime;
		if (_nextHorizontalCameraChange <= 0)
		{
			_moveHorizontalCamera = Random.Range(-1, 2);
			_nextHorizontalCameraChange = Random.Range(checkChangeCameraMinInterval, checkChangeCameraMaxInterval);
		}
		RotateCamera(_moveHorizontalCamera);
	}
}
