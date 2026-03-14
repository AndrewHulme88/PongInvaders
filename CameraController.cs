using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeDuration = 0.25f;

    private Vector3 camOriginPos;
    private bool isShaking = false;

    void Start()
    {
        camOriginPos = mainCamera.transform.position;
    }

    private void Update()
    {
        if(isShaking)
        {
            StartCoroutine(CameraShakeRoutine());
        }
    }

    public void ShakeCamera()
    {
        if (!isShaking)
        {
            isShaking = true;
        }
    }

    IEnumerator CameraShakeRoutine()
    {
        mainCamera.transform.position = camOriginPos + (Random.insideUnitSphere * shakeIntensity);
        yield return new WaitForSeconds(shakeDuration);
        mainCamera.transform.position = camOriginPos;
        isShaking = false;
    }
}
