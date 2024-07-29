using System.Collections;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.VRTemplateAssets.Scripts
{
    public class GrapichTimeLine : MonoBehaviour
    {
        public RectTransform graphContainer;
        public RectTransform labelTemplateX;
        public RectTransform doteTemplate;
        public RectTransform labelTemplateY;
        public VideoTimeScrubControl videoController;

        public void CreateTimeLine(float time)
        {
            float videoDurationInSeconds = (float) videoController.m_VideoPlayer.clip.length;
            Debug.Log("Time " + videoDurationInSeconds);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}