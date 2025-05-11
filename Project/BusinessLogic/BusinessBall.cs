//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

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

        #region IBall

        public event EventHandler<IPosition>? NewPositionNotification;

        #endregion IBall

        #region private

        private void RaisePositionChangeEvent(object? sender, Data.IVector e)
        {
            CheckCollisionWithWalls(e);
            NewPositionNotification?.Invoke(this, new Position(e.x, e.y));
        }

        private void CheckCollisionWithWalls(Data.IVector e)
        {
            double minX = 0, maxX = 400, minY = 0, maxY = 420;
            double radius = 10; // Przyjmujemy, że piłka ma promień 10 jednostek

            // Sprawdzamy kolizję z lewą i prawą ścianą (oś X)
            if (e.x <= minX + radius || e.x >= maxX - radius)
            {
                // Zmiana kierunku prędkości w osi X (odbicie)
                _dataBall.Velocity.x = -_dataBall.Velocity.x;
                _dataBall.Velocity.y = _dataBall.Velocity.y; 
            }

            // Sprawdzamy kolizję z górną i dolną ścianą (oś Y)
            if (e.y <= minY + radius || e.y >= maxY - radius)
            {
                // Zmiana kierunku prędkości w osi Y (odbicie)
                _dataBall.Velocity.x = _dataBall.Velocity.x;
                _dataBall.Velocity.y = -_dataBall.Velocity.y;
            }
        }

        #endregion private
    }
}

