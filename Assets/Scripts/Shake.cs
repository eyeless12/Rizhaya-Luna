using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    private Transform _tf;

    private void Awake()
    {
        _tf = GetComponent<Transform>();
    }

    private IEnumerator Shaking(float magnitude, float duration)
    {
        var startPosition = new Vector3(0, 0, -20);
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var strength = curve.Evaluate(elapsedTime / duration);
            _tf.localPosition = startPosition + (Vector3)Random.insideUnitCircle * strength * magnitude;
            yield return new WaitForEndOfFrame();
        }

        _tf.localPosition = startPosition;
    }

    public void ActivateShake(float magnitude, float duration = 1f) => StartCoroutine(Shaking(magnitude, duration));
}
