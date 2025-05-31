//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________
using System.Diagnostics;


namespace TP.ConcurrentProgramming.BusinessLogic
{
    internal class Ball : IBall
    {

        private readonly Data.IBall _dataBall;

        public Ball(Data.IBall ball)
        {
            _dataBall = ball;
            ball.NewPositionNotification += RaisePositionChangeEvent;
        }

        public void SetOtherBalls(List<Ball> otherBalls)
        {
            _otherBalls = otherBalls;
        }


        #region IBall

        public event EventHandler<IPosition>? NewPositionNotification;

        #endregion IBall

        #region private

        private List<Ball> _otherBalls = new();
        private readonly object _ballsLock = new object();

        private readonly object _positionLock = new();

        private void RaisePositionChangeEvent(object? sender, Data.IVector e)
        {
            lock (_positionLock)
            {
                CheckCollisionWithWalls(e);
                CheckCollisionWithOtherBalls();
                NewPositionNotification?.Invoke(this, new Position(e.x, e.y));
            }
        }


        private void CheckCollisionWithWalls(Data.IVector e)
        {
            double minX = 0, maxX = 400, minY = 0, maxY = 420;
            double radius = 15;

            // Kolizja z lewą i prawą ścianą (oś X)
            if (e.x <= minX + 5)
            {
                // Odbicie od lewej ściany
                
                 
                _dataBall.Velocity.x = -_dataBall.Velocity.x;
            
                _dataBall.Position.x = minX + 5 + Math.Abs(_dataBall.Velocity.x); 
            }
            else if (e.x >= maxX - radius)
            {
                // Odbicie od prawej ściany
                _dataBall.Velocity.x = -_dataBall.Velocity.x;
                _dataBall.Position.x = maxX - radius - Math.Abs(_dataBall.Velocity.x);
            }

            // Kolizja z górną i dolną ścianą (oś Y)
            if (e.y <= minY + 5)
            {
                // Odbicie od górnej ściany
                _dataBall.Velocity.y = -_dataBall.Velocity.y;
                _dataBall.Position.y = minY + 5 + Math.Abs(_dataBall.Velocity.y); 
            }
            else if (e.y >= maxY - radius)
            {
                // Odbicie od dolnej ściany
                _dataBall.Velocity.y = -_dataBall.Velocity.y;
                _dataBall.Position.y = maxY - radius - Math.Abs(_dataBall.Velocity.y); 
            }
        }


        private void CheckCollisionWithOtherBalls()
        {
            foreach (var other in _otherBalls)
            {
                if (other == this) continue;

                // Sprawdź czy są w kolizji
                var dx = _dataBall.Position.x - other._dataBall.Position.x;
                var dy = _dataBall.Position.y - other._dataBall.Position.y;
                var distSq = dx * dx + dy * dy;
                var minDist = 20;

                if (distSq <= minDist * minDist)
                {
                    object firstLock = this.GetHashCode() < other.GetHashCode() ? this : other;
                    object secondLock = this.GetHashCode() < other.GetHashCode() ? other : this;

                    lock (firstLock)
                    {
                        lock (secondLock)
                        {
                            // odbicie idealnie sprezyste (dla tej samej masy)
                            var temp = _dataBall.Velocity;
                            _dataBall.Velocity = other._dataBall.Velocity;
                            other._dataBall.Velocity = temp;

                            // przesunięcie kul, aby nie nachodziły na siebie
                            other._dataBall.Position.x += other._dataBall.Velocity.x;
                            other._dataBall.Position.y += other._dataBall.Velocity.y;
                            _dataBall.Position.x += _dataBall.Velocity.x;
                            _dataBall.Position.y += _dataBall.Velocity.y;
                        }
                    }
                }
            }
        }



        #endregion private
    }
}

