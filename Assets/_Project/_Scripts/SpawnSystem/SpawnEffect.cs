using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] GameObject spawnVFX;
    [SerializeField] float animationTime = 1f;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, animationTime).SetEase(Ease.OutBack);

        if (spawnVFX != null)
        {
            Instantiate(spawnVFX, transform.position, Quaternion.identity);
        }

        GetComponent<AudioSource>().Play();
    }
}
