using System;
using UnityEngine;

namespace Persiafighter.Plugins.AntiAFK.Classes
{
    public sealed class PlayerData
    {
        public Vector3 LastPosition;
        public DateTime LastPositionChange;

        public PlayerData(Vector3 Position, DateTime Time)
        {
            LastPosition = Position;
            LastPositionChange = Time;
        }

        public void Update(Vector3 Position, DateTime Time)
        {
            LastPosition = Position;
            LastPositionChange = Time;
        }
    }
}
