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
    public const string OutsideWavesSound = "event:/Outside/Waves";
    public const string BackgroundSound = "event:/Backgroundtrack";
    public const string OutsidePeopleSound = "event:/Outside";
    public const string TabletShootSound = "event:/Tabletshot";
    public const string TabletStickSound = "event:/Tabletstick";
    public const string LaserSound = "event:/Laser";

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
    public const string TutorialStartGermanSound = "event:/TutorialG/TStartG";
    public const string TutorialShakeGermanSound = "event:/TutorialG/TShakeG";
    public const string TutorialPickingUpGermanSound = "event:/TutorialG/TPickingupG";
    public const string TutorialAslidersGermanSound = "event:/TutorialG/TAslidersG";
    public const string TutorialFslidersGermanSound = "event:/TutorialG/TFslidersG";
    public const string TutorialEndingGermanSound = "event:/TutorialG/TEndingG";
    public const string TutorialSandMoveGermanSound = "event:/TutorialG/TSandmoveG";
    public const string TutorialSliderMoveGermanSound = "event:/TutorialG/TSlidermoveG";

    public enum Language
    {
        Dutch, German
    };

    public static Language LanguageSelected = Language.Dutch;

    public const string DutchQuestion1 = "Vond je het spel leuk?";
    public const string DutchQuestion2 = "Vind je resonantie interessant?";
    public const string DutchQuestion3 = "Heeft het spel je wat geleerd over resonantie?";
    public const string DutchQuestionThanks = "Bedankt voor het spelen!";

    public const string GermanQuestion1 = "Hat dir das Spiel spaß gemacht?";
    public const string GermanQuestion2 = "Findest du Resonanzen interessant?";
    public const string GermanQuestion3 = "Hast du etwas über Resonanzen gelernt?";
    public const string GermanQuestionThanks = "Danke für's teilnehmen!";
}