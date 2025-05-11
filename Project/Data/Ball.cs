using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity, double mass=10.0, double radius=1.0)
        {
            double x = initialPosition.x;
            double y = initialPosition.y;
            Mass = mass;
            Radius = radius;

            MaxX = 400 - Radius;
            MaxY = 420 - Radius;
            MinX = 0 + Radius;
            MinY = 0 + Radius;

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
        public double Mass { get; init; } = 1.0; 
        public double Radius { get; init; } = 10.0;


        #endregion IBall

        #region private

        private Vector Position;
        private readonly Stopwatch _stopwatch;

        private double MaxX;
        private double MaxY;
        private double MinX;
        private double MinY;

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }

        internal void Move(Vector delta)
        {
            double dx = Velocity.x;
            double dy = Velocity.y;

            double newX = Position.x + dx;
            double newY = Position.y + dy;

            // os X
            if (newX - Radius < MinX)
            {
                newX = MinX + Radius;
                dx = -dx;
            }
            else if (newX + Radius >= MaxX)
            {
                newX = MaxX - Radius;
                dx = -dx;
            }

            // os Y
            if (newY - Radius < MinY)
            {
                newY = MinY + Radius;
                dy = -dy;
            }
            else if (newY + Radius >= MaxY)
            {
                newY = MaxY - Radius;
                dy = -dy;
            }

            Velocity = new Vector(dx, dy);
            Position = new Vector(newX, newY);
            RaiseNewPositionChangeNotification();
        }



        #endregion private
    }
}