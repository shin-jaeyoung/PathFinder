using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour, IPoolable
{
    [SerializeField]
    private int id;
    [SerializeField]
    private AudioClip clip;

    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
    }
    private void OnEnable()
    {
        
        if (clip != null)
        {
            StartCoroutine(SoundPlay());
        }
        else
        {
            ReturnToPool();
        }
    }


    private IEnumerator SoundPlay()
    {
        
        source.Play();
        WaitForSeconds wait = new WaitForSeconds(source.clip.length);

        yield return wait;

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        source.Stop();
        
        PoolManager.instance.PoolDic[PoolType.SFX].ReturnPool(this);
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetID()
    {
        return id;
    }
}
