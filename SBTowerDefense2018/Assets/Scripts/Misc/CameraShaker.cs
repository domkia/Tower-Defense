using UnityEngine;
using System.Collections;

public class CameraShaker : Singleton<CameraShaker>
{
    private Transform trans;
    private Vector3 startPos;

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    public void Shake(float amount, float duration)
    {
        //startPos = trans.localPosition;
        StartCoroutine(_Shake(amount, duration));
    }

    IEnumerator _Shake(float _amount, float _duration)
    {
        float t = _duration;
        while(t > 0.0f)
        {
            startPos = trans.position;
            t -= Time.deltaTime;
            trans.localPosition = startPos + Random.insideUnitSphere * _amount * t;
            yield return null;
        }
        trans.localPosition = startPos;
    }
}
