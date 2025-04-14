using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity)
        {
            double x = initialPosition.x;
            double y = initialPosition.y;

            if (x < MinX) x = MinX;
            else if (x > MaxX) x = MaxX;

            if (y < MinY) y = MinY;
            else if (y > MaxY) y = MaxY;

            Position = new Vector(x, y);
            Velocity = initialVelocity;
            _stopwatch = Stopwatch.StartNew();
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }

        #endregion IBall

        #region private

        private Vector Position;
        private readonly Stopwatch _stopwatch;
        private long _lastUpdateTime = 0;
        private const double MinUpdateIntervalMs = 200; // 16ms = ~60fps

        private const double Radius = 10;
        private const double MaxX = 400 - Radius;
        private const double MaxY = 420 - Radius;
        private const double MinX = 0 + Radius;
        private const double MinY = 0 + Radius;

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }

        internal void Move(Vector delta)
        {
            long currentTime = _stopwatch.ElapsedMilliseconds;
            long timeSinceLastUpdate = currentTime - _lastUpdateTime;

            // Only update if at least 16ms have passed since last update
            if (timeSinceLastUpdate < MinUpdateIntervalMs)
            {
                return;
            }

            _lastUpdateTime = currentTime;

            double newX = Position.x + delta.x;
            double newY = Position.y + delta.y;

            // oś X
            if (newX - Radius <= MinX)
            {
                newX = MinX + Radius;
                Velocity = new Vector(-Velocity.x, Velocity.y); // bounce
            }
            else if (newX + Radius >= MaxX)
            {
                newX = MaxX - Radius;
                Velocity = new Vector(-Velocity.x, Velocity.y); // bounce
            }

            // oś Y
            if (newY - Radius <= MinY)
            {
                newY = MinY + Radius;
                Velocity = new Vector(Velocity.x, -Velocity.y); // bounce
            }
            else if (newY + Radius >= MaxY)
            {
                newY = MaxY - Radius;
                Velocity = new Vector(Velocity.x, -Velocity.y); // bounce
            }

            Position = new Vector(newX, newY);
            RaiseNewPositionChangeNotification();
        }

        #endregion private
    }
}