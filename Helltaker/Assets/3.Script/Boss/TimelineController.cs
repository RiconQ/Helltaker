using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private TimelineAsset normalPhase;
    [SerializeField] private TimelineAsset chainPhase;
    [SerializeField] private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.Play();
        director.stopped += OnNormalPhaseEnd;
    }

    private void OnNormalPhaseEnd(PlayableDirector director)
    {
        director.playableAsset = chainPhase;
        director.Play();
        StartCoroutine(ChainPhase_co());
    }

    private IEnumerator ChainPhase_co()
    {
        while(true)
        {
            if(director.state != PlayState.Playing)
            {
                director.Play();
            }
            yield return null;
        }
    }

}
