using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomEffect : MonoBehaviour
{
    [SerializeField] float spinMaxVal = 10f;
    [SerializeField] float spinLockVal = 1f;
    [FormerlySerializedAs("duracion")]
    [Min(0.01f)]
    [SerializeField] float duration = 3f;
    [SerializeField] private Image modImage;
    [SerializeField] Modifiers[] modifiers;
    private GameObject currentBallMod;
    [SerializeField] Scorer [] scorer;
    [SerializeField] Material [] Outlines;
    [SerializeField] Color [] ColorMat;

    float currentSpinVal = 0f;
    int currentModifierIndex = 0;
    GameObject activeEffect;
    bool isSpinning = false;

//TEST
    void Start()
    {
        StartModRoulette();
    }


    public void StartModRoulette()
    {
        if (isSpinning) return;

        if (modifiers == null || modifiers.Length == 0)
        {
            Debug.LogWarning("RandomEffect: no modifiers assigned.", this);
            return;
        }

        //CLEAR IN GAME MODS
        if (activeEffect != null) Destroy(activeEffect);
        activeEffect = null;
        currentBallMod = null;

        for (int i = 0; i < modifiers.Length; i++)
        {
            if (modifiers[i] == null)
            {
                Debug.LogWarning($"RandomEffect: modifiers[{i}] is empty.", this);
                return;
            }
        }

        StartCoroutine(SlowDownRoulette());
    }

    void OnDisable()
    {
        isSpinning = false;
    }

    IEnumerator SlowDownRoulette()
    {
        isSpinning = true;

        float elapsedTime = 0f;
        currentSpinVal = spinMaxVal;

        currentModifierIndex = Random.Range(0, modifiers.Length);
        float slotProgress = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            currentSpinVal = spinLockVal + (spinMaxVal - spinLockVal) * Mathf.Pow(1f - t, 3f);

            slotProgress += currentSpinVal * Time.deltaTime;
            while (slotProgress >= 1f)
            {
                slotProgress -= 1f;
                currentModifierIndex = (currentModifierIndex + 1) % modifiers.Length;
            }
            modImage.sprite = modifiers[currentModifierIndex].GetSprite();
            yield return null;
        }

        currentSpinVal = spinLockVal;
        isSpinning = false;
        currentBallMod = modifiers[currentModifierIndex].GetPrefab();
        ApplyModifier();
    }

    void ApplyModifier()
    {
        if (activeEffect != null) Destroy(activeEffect);

        activeEffect = Instantiate(currentBallMod, transform.position, Quaternion.identity);

        int n = activeEffect.GetComponent<CustomGravity>().positionScore;
        activeEffect.GetComponentInChildren<MeshRenderer>().material = Outlines[n];

        for (int i = 0; i < scorer.Length; i++)
        {
            scorer[i].setBall(activeEffect);
            scorer[i].desactivateCol();
        }
        StartCoroutine(CheckIfAlive(activeEffect));
    }

    static readonly WaitForSeconds aliveCheckDelay = new WaitForSeconds(0.25f);

    IEnumerator CheckIfAlive(GameObject target)
    {
        yield return aliveCheckDelay;
        if (target != null)
            yield return StartCoroutine(CheckIfAlive(target)); // still alive, check again
        else
            OnEffectDestroyed();
    }

    void OnEffectDestroyed()
    {
        activeEffect = null;
        StartModRoulette();
    }


}