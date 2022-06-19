using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration, float strength)
    {
        Vector2 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < duration && Time.timeScale == 1)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.localPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

}
