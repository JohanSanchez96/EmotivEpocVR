using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineController: MonoBehaviour
{
    public List<PlayableDirector> playableDirectors = new List<PlayableDirector>();

    private void Start()
    {
        GameManager.Instance.timeLineController = this;
    }

    public void SetCinematics(GameObject Container)
    {
        playableDirectors.Clear();

        foreach (PlayableDirector playable in Container.GetComponentsInChildren<PlayableDirector>())
        {
            playableDirectors.Add(playable);
        }
    }

    public bool StatePlayable (int index)
    {
        if (playableDirectors[index].state == PlayState.Playing)
        {
            return true;
        }
        else
        {
            if (playableDirectors[index].state == PlayState.Paused)
            {
                if (playableDirectors[index].time != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public void Play(int index)
    {
        playableDirectors[index].Play();
        Debug.Log(playableDirectors[index].name);
    }

    public void Pause(int index)
    {
        playableDirectors[index].Pause();
    }

    public void Resume(int index)
    {
        playableDirectors[index].Resume();
    }
}
