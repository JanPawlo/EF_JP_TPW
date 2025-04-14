//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

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
    }

    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }

    #endregion IBall

    #region private

    private Vector Position;

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
        double newX = Position.x + delta.x;
        double newY = Position.y + delta.y;
        
        // oś X
        if (newX - Radius <= MinX)
        {
            newX = MinX + Radius;
        }
        else if (newX + Radius >= MaxX)
        {
            newX = MaxX - Radius;
        }
        
        // oś Y
        if (newY - Radius <= MinY)
        {
            newY = MinY + Radius;
        }
        else if (newY + Radius >= MaxY)
        {
            newY = MaxY - Radius;
        }
        
        Position = new Vector(newX, newY);
        RaiseNewPositionChangeNotification();
    }

    #endregion private
  }
}