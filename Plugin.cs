using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MoonCleaners;
using UnityEngine;

namespace MoonSweepers.StationClock;

[BepInPlugin("moon-sweepers.station-clock", "Station Clock", "0.1.1")]
public sealed class Plugin : BasePlugin
{
    private static MoonActionPromptCard card;
    private static float nextUpdate;
    private static BepInEx.Logging.ManualLogSource logger;
    private static bool failed;
    public override void Load()
    {
        logger=Log;
        new Harmony("moon-sweepers.station-clock").PatchAll();
        Log.LogInfo("Station Clock loaded. Local solar time uses a 24-hour lunar-day dial; countdown is real time at current game speed.");
    }
    [HarmonyPatch(typeof(MoonSolarSystem), "LateUpdate")]
    private static class Tick
    {
        private static void Postfix(MoonSolarSystem __instance)
        {
            if(Time.unscaledTime<nextUpdate)return;
            nextUpdate=Time.unscaledTime+0.5f;
            try
            {
                var walker=UnityEngine.Object.FindObjectsOfType<MoonWalker>().FirstOrDefault();
                if(walker==null){card?.Hide();return;}
                var up=walker.transform.position.normalized;
                var t=__instance.LunarTime;
                var height=Vector3.Dot(MoonSolarSystem.SunDirectionAt(t),up);
                var quarter=Vector3.Dot(MoonSolarSystem.SunDirectionAt((t+0.25f)%1f),up);
                var hours=(12+Math.Atan2(-quarter,height)*12/Math.PI+24)%24;
                var minutes=(int)(hours*60)%1440;
                var day=height>0;
                float until=-1, prev=height;
                for(var i=1;i<=1440;i++)
                {
                    float delta=i/1440f;
                    float h=Vector3.Dot(MoonSolarSystem.SunDirectionAt((t+delta)%1f),up);
                    if(day?prev>=0&&h<0:prev<=0&&h>0){until=delta;break;}
                    prev=h;
                }
                var speed=MoonSolarSystem.TimeSpeed;
                var seconds=until*__instance.lunarDaySeconds/Math.Abs(speed);
                string countdown=until<0?"No horizon crossing":speed<=0?"Clock paused/reversed":$"{(day?"Sunset":"Sunrise")} in {Format(seconds)}";
                string title=$"Local time: {minutes/60:00}:{minutes%60:00} | {countdown}";
                if(card==null)
                {
                    card=MoonActionPromptCard.Create(walker.transform,"StationClockHUD",80);
                    card.Root.anchorMin=new Vector2(0,1f);
                    card.Root.anchorMax=new Vector2(0,1f);
                    card.Root.pivot=new Vector2(0,1f);
                    card.Root.anchoredPosition=new Vector2(24,-24);
                    logger.LogInfo("Station Clock HUD created: "+title);
                }
                card.Show("",title,false,-1,false);
                card.Key.gameObject.SetActive(false);
                card.cap.SetActive(false);
                var titleRect=card.Title.GetComponent<RectTransform>();
                titleRect.anchoredPosition=new Vector2(14,titleRect.anchoredPosition.y);
            }
            catch(Exception ex){if(!failed){failed=true;logger.LogError(ex);} }
        }
    }
    private static string Format(double seconds)
    {
        if(double.IsNaN(seconds)||double.IsInfinity(seconds))return "--";
        var span=TimeSpan.FromSeconds(Math.Max(0,seconds));
        return $"{(int)span.TotalHours:00}:{span.Minutes:00}:{span.Seconds:00}";
    }
}
