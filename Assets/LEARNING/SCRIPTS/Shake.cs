using UnityEngine;
using NaughtyAttributes;

public class Shake : MonoBehaviour
{
    [Header("Shake Effect")]
    [SerializeField] private float shakeSpeed = 2f;
    [SerializeField] private float shakeAmplitudeY = 0.1f;
    [SerializeField] private float shakeAmplitudeX = 0.05f;
    [SerializeField] private float shakeAmplitudeZ = 0.05f;
    
    private Vector3 originalPosition;

    // Semillas distintas por eje para que el ruido de cada eje no esté correlacionado
    private float seedX, seedY, seedZ;

    void Start()
    {
        originalPosition = transform.localPosition;

        seedX = Random.Range(0f, 1000f);
        seedY = Random.Range(0f, 1000f);
        seedZ = Random.Range(0f, 1000f);
    }

    void Update()
    {
        // Perlin noise devuelve [0,1]; lo remapeamos a [-1,1] antes de aplicar la amplitud
        float offsetX = (Mathf.PerlinNoise(seedX, Time.time * shakeSpeed) * 2f - 1f) * shakeAmplitudeX;
        float offsetY = (Mathf.PerlinNoise(seedY, Time.time * shakeSpeed) * 2f - 1f) * shakeAmplitudeY;
        float offsetZ = (Mathf.PerlinNoise(seedZ, Time.time * shakeSpeed) * 2f - 1f) * shakeAmplitudeZ;

        transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);
    }
}