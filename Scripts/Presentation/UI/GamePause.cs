using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Infrastructure.Pause
{
    public static class GamePause
    {
        private static readonly HashSet<object> Sources = new();

        public static bool IsPaused => Sources.Count > 0;

        public static event Action<bool> Changed;

        public static void SetPaused(object source, bool paused)
        {
            if (source == null)
                return;

            bool wasPaused = IsPaused;

            if (paused)
                Sources.Add(source);
            else
                Sources.Remove(source);

            bool isPaused = IsPaused;
            if (wasPaused == isPaused)
                return;
            
            AudioListener.pause = isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            Changed?.Invoke(isPaused);
        }
    }
}