using UnityEngine;
using UnityEditor;

public static class GLOB
{
    //Sound paths
    public const string DomeOpeningSound = "event:/Outside/DomeOpening";
    public const string BottleFallSound = "event:/PlayArea/Bottlefall";
    public const string BottlePickupSound = "event:/PlayArea/BottlePickup";
    public const string BouncyBallSound = "event:/PlayArea/Bouncyball";
    public const string CelebrationSound = "event:/PlayArea/Celebration";
    public const string GeneralPickupSound = "event:/PlayArea/GeneralPickup";
    public const string JarFallSound = "event:/PlayArea/JarFall";
    public const string JarPickUpSound = "event:/PlayArea/JarPickup";
    public const string LeverSound = "event:/PlayArea/Lever";
    public const string LeverReleaseSound = "event:/PlayArea/LeverRelease";
    public const string JarPourSandSound = "event:/PlayArea/SandPour";
    public const string JarShakeSound = "event:/PlayArea/JarShake";
    public const string SpotlightSound = "event:/PlayArea/Spotlight";
    public const string TouchingSliderSound = "event:/PlayArea/TouchingSlider";
    public const string LeverClickSound = "event:/PlayArea/LeverClicks";
    public const string MonitorSwitchSound = "event:/PlayArea/MonitorSwitch";
    public const string MonitorTurnOnSound = "event:/PlayArea/MonitorTurnOn";

    //Sounds for tutorial
    //Dutch
    public const string TutorialStartDutchSound = "event:/Tutorial/TStart";
    public const string TutorialShakeDutchSound = "event:/Tutorial/TShake";
    public const string TutorialPickingUpDutchSound = "event:/Tutorial/TPickingup";
    public const string TutorialAslidersDutchSound = "event:/Tutorial/TAsliders";
    public const string TutorialFslidersDutchSound = "event:/Tutorial/TFsliders";
    public const string TutorialEndingDutchSound = "event:/Tutorial/TEnding";
    public const string TutorialSandMoveDutchSound = "event:/Tutorial/TSandmove";
    public const string TutorialSliderMoveDutchSound = "event:/Tutorial/TSlidermove";

    //German
    public const string TutorialStartGermanSound = "event:/Tutorial/TStart";
    public const string TutorialShakeGermanSound = "event:/Tutorial/TShake";
    public const string TutorialPickingUpGermanSound = "event:/Tutorial/TPickingup";
    public const string TutorialAslidersGermanSound = "event:/Tutorial/TAsliders";
    public const string TutorialFslidersGermanSound = "event:/Tutorial/TFsliders";
    public const string TutorialEndingGermanSound = "event:/Tutorial/TEnding";
    public const string TutorialSandMoveGermanSound = "event:/Tutorial/TSandmove";
    public const string TutorialSliderMoveGermanSound = "event:/Tutorial/TSlidermove";

    public enum Language
    {
        Dutch, German
    };

    public static Language LanguageSelected = Language.Dutch;
}