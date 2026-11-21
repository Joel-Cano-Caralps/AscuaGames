using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Transform camTransform;
	[SerializeField] private bool lockX, lockY, lockZ;

	void Start()
	{
		FindCamera();
	}

	void LateUpdate()
	{
		if (camTransform != null)
		{
			// Mira hacia la cámara
			transform.LookAt(transform.position + camTransform.forward);

			// Guardamos la rotación calculada
			Vector3 euler = transform.rotation.eulerAngles;

			// Bloqueamos los ejes seleccionados
			if (lockX) euler.x = 0f;
			if (lockY) euler.y = 0f;
			if (lockZ) euler.z = 0f;

			// Aplicamos la rotación corregida
			transform.rotation = Quaternion.Euler(euler);
		}
	}

	private void FindCamera()
	{
		GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

		if (cameraObject != null)
			camTransform = cameraObject.transform;
		else
			Debug.LogWarning("MainCamera not found for Billboard object: " + gameObject.name);
	}
}