using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public VideoTimeScrubControl videoPlayerController;
    public BackGroundController backGroundController;
    public TimeLineController timeLineController;
}
