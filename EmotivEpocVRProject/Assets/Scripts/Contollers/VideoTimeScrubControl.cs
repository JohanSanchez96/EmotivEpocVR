using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Connects a UI slider control to a video player, allowing users to scrub to a particular time in th video.
    /// </summary>
    public class VideoTimeScrubControl : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Video play/pause button GameObject")]
        GameObject m_ButtonPlayOrPause;

        [SerializeField]
        [Tooltip("Slider that controls the video")]
        Slider m_Slider;

        [SerializeField]
        [Tooltip("Play icon sprite")]
        Sprite m_IconPlay;

        [SerializeField]
        [Tooltip("Pause icon sprite")]
        Sprite m_IconPause;

        [SerializeField]
        [Tooltip("Repeat icon sprite")]
        Sprite m_IconRepeat;
        
        [Tooltip("Repeat Panel")]
        [SerializeField]
        GameObject m_PanelRepeat;

        [SerializeField]
        [Tooltip("Play or pause button image.")]
        Image m_ButtonPlayOrPauseIcon;

        [SerializeField]
        [Tooltip("Text that displays the current time of the video.")]
        TextMeshProUGUI m_VideoTimeText;

        [HideInInspector]
        [Tooltip("Text that displays the current name video.")]
        public TextMeshProUGUI m_VideoNameText;

        [SerializeField]
        [Tooltip("If checked, the slider will fade off after a few seconds. If unchecked, the slider will remain on.")]
        bool m_HideSliderAfterFewSeconds;

        public bool m_IsDragging;
        public bool m_VideoIsPlaying;
        public bool m_VideoJumpPending;
        public long m_LastFrameBeforeScrub;

        public int m_CurrentIndex;
        public VideoPlayer m_VideoPlayer;
        public List<VideoClip> videoClips = new List<VideoClip>();

        private void Start()
        {
           GameManager.Instance.videoPlayerController = this;
           m_VideoPlayer.loopPointReached += OnVideoFinished;
        }

        void OnVideoFinished(VideoPlayer vp)
        {
            m_PanelRepeat.SetActive(true);
        }

        public void First()
        {
            ResetPanel();
            VideoPlay();

            if (m_ButtonPlayOrPause != null)
                m_ButtonPlayOrPause.SetActive(false);
        }

        public void ResetPanel()
        {
            if (m_VideoPlayer != null)
            {
                m_VideoPlayer.frame = 0;
                VideoPlay(); // Ensures correct UI state update if paused.
            }
            m_Slider.value = 0.0f;
            m_Slider.onValueChanged.AddListener(OnSliderValueChange);
            m_Slider.gameObject.SetActive(true);
            m_PanelRepeat.SetActive(false);

            if (m_HideSliderAfterFewSeconds)
                StartCoroutine(HideSliderAfterSeconds());
        }

        void Update()
        {
            if (m_VideoJumpPending)
            {
                // We're trying to jump to a new position, but we're checking to make sure the video player is updated to our new jump frame.
                if (m_LastFrameBeforeScrub == m_VideoPlayer.frame)
                    return;

                // If the video player has been updated with desired jump frame, reset these values.
                m_LastFrameBeforeScrub = long.MinValue;
                m_VideoJumpPending = false;
            }

            if (!m_IsDragging && !m_VideoJumpPending)
            {
                if (m_VideoPlayer.frameCount > 0)
                {
                    var progress = (float)m_VideoPlayer.frame / m_VideoPlayer.frameCount;
                    m_Slider.value = progress;
                }
            }
        }

        public void OnPointerDown()
        {
            m_VideoJumpPending = true;
            VideoStop();
            VideoJump();
        }

        public void OnRelease()
        {
            m_IsDragging = false;
            VideoPlay();
            VideoJump();
        }

        void OnSliderValueChange(float sliderValue)
        {
            UpdateVideoTimeText();
        }

        IEnumerator HideSliderAfterSeconds(float duration = 1f)
        {
            yield return new WaitForSeconds(duration);
            m_Slider.gameObject.SetActive(false);
        }

        public void OnDrag()
        {
            m_IsDragging = true;
            m_VideoJumpPending = true;
        }

        void VideoJump()
        {
            if (m_PanelRepeat.activeInHierarchy)
            {
                m_PanelRepeat.SetActive(false);
            }

            m_VideoJumpPending = true;
            var timeInSeconds = m_VideoPlayer.length * m_Slider.value;
            m_VideoPlayer.time = timeInSeconds;
        }

        public void SetVideo(int index)
        {
            m_CurrentIndex = index;
            m_VideoPlayer.clip = videoClips[m_CurrentIndex];

            //if (m_VideoNameText)
            //{
            //    m_VideoNameText.text = LanguageManager.Instance.GetStringValue(GameManager.Instance.playerStats.toolsModule[index].moduleName);
            //}
        }

        public void PlayOrPauseVideo()
        {
            if (m_VideoIsPlaying)
            {
                VideoStop();
            }
            else
            {
                VideoPlay();
            }
        }

        void UpdateVideoTimeText()
        {
            if (m_VideoPlayer != null && m_VideoTimeText != null)
            {
                var currentTimeTimeSpan = TimeSpan.FromSeconds(m_VideoPlayer.time);
                var totalTimeTimeSpan = TimeSpan.FromSeconds(m_VideoPlayer.length);
                var currentTimeString = string.Format("{0:D2}:{1:D2}",
                    currentTimeTimeSpan.Minutes,
                    currentTimeTimeSpan.Seconds
                );

                var totalTimeString = string.Format("{0:D2}:{1:D2}",
                    totalTimeTimeSpan.Minutes,
                    totalTimeTimeSpan.Seconds
                );
                m_VideoTimeText.SetText(currentTimeString + " / " + totalTimeString);
            }
        }

        public void VideoStop()
        {
            m_VideoIsPlaying = false;
            m_VideoPlayer.Pause();
            m_ButtonPlayOrPauseIcon.sprite = m_IconPlay;
            m_ButtonPlayOrPause.SetActive(true);
        }

        public void RestartVideoPlayer()
        {
            m_PanelRepeat.SetActive(false);
            m_VideoPlayer.Stop();
            m_VideoPlayer.time = 0.0;
            RenderTexture renderTexture;
            renderTexture = m_VideoPlayer.targetTexture;
            renderTexture.Release();
            renderTexture.Create();
            m_VideoPlayer.targetTexture = renderTexture;
            VideoPlay();
        }

        void VideoPlay()
        {
            m_VideoIsPlaying = true;
            m_VideoPlayer.Play();
            m_ButtonPlayOrPauseIcon.sprite = m_IconPause;
            m_ButtonPlayOrPause.SetActive(false);
        }

        private void OnDisable()
        {
           // GameManager.Instance.backGroundController.RestartVideoPlayer(m_VideoPlayer);
        }

    }
}
