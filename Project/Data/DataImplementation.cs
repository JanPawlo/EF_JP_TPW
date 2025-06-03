//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.Data
{
  internal class DataImplementation : DataAbstractAPI
  {

    public Action<List<IBall>>? BallsListUpdated;

    #region ctor

    public DataImplementation()
    {
      //
    }

    #endregion ctor

    #region DataAbstractAPI

    public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(DataImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
      Random random = new Random();
      for (int i = 0; i < numberOfBalls; i++)
      {
        Vector startingPosition = new Vector(random.Next(100, 400 - 100), random.Next(100, 400 - 100));
        Vector initialVelocity = new Vector((random.NextDouble() - 1.5) * 2.0, (random.NextDouble() - 1.5) * 2.0);

        Ball newBall = new(startingPosition, initialVelocity);
        upperLayerHandler(startingPosition, newBall);
        BallsList.Add(newBall);
      }
        foreach (Ball ball in BallsList)
        {
            var thread = new Thread(() => BallThreadLoop(ball, _cts.Token));
            _threads.Add(thread);
            thread.Start();
        }
      BallsListUpdated?.Invoke(BallsList.Cast<IBall>().ToList());
    }

        public override void Stop()
        {
            _cts.Cancel(); // Signal threads to stop

            // Wait for all threads to finish
            foreach (var thread in _threads)
            {
                thread.Join(); // Wait for each thread to complete
            }

            _threads.Clear(); // Clear the threads list

            lock (_ballsLock)
            {
                BallsList.Clear();
            }

            BallsListUpdated?.Invoke(new List<IBall>());

            // Reset the CancellationTokenSource for potential future use
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }




        #endregion DataAbstractAPI

            #region IDisposable

        protected virtual void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          //
          lock (_ballsLock)
             {
                BallsList.Clear();
                BallsListUpdated?.Invoke(new List<IBall>());
             }
             }
                Disposed = true;
                _cts.Cancel();
                foreach (var thread in _threads)
                {
                    thread.Join();
                }
                _threads.Clear();
                _cts.Dispose();

            }

            else
        throw new ObjectDisposedException(nameof(DataImplementation));
    }

    public override void Dispose()
    {
        /*
         _cts.Cancel();
        foreach (var thread in _threads)
        {
            thread.Join();
        }
        _threads.Clear();
        _cts.Dispose();

        lock (_ballsLock)
        {
            BallsList.Clear();
            BallsListUpdated?.Invoke(new List<IBall>());
        }
        **/
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }



    #endregion IDisposable

    #region private

    //private bool disposedValue;
    private bool Disposed = false;

    private Random RandomGenerator = new();
    private readonly object _ballsLock = new object();

    private List<Ball> BallsList = [];

    private CancellationTokenSource _cts = new CancellationTokenSource();
    private List<Thread> _threads = new List<Thread>();



        private void BallThreadLoop(Ball ball, CancellationToken token)
        {
            const int frameTimeMs = 20; // Target: 50 FPS (1000ms / 50 = 20ms per frame)
            Timer timer = null;

            // Timer callback (executed on a ThreadPool thread)
            TimerCallback timerCallback = _ =>
            {
                try
                {
                    if (token.IsCancellationRequested)
                    {
                        timer?.Dispose(); // Stop the timer if cancellation is requested
                        return;
                    }

                    lock (_ballsLock)
                    {
                        ball.Move();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ball update error: {ex.Message}");
                }
            };

            // Create and start the timer (first run immediately, then every `frameTimeMs`)
            timer = new Timer(timerCallback, null, 0, frameTimeMs);

            // Ensure cleanup when cancellation is requested
            token.Register(() =>
            {
                timer?.Dispose();
            });
        }



        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
    internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
    {
      returnBallsList(BallsList);
    }

    [Conditional("DEBUG")]
    internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
    {
      returnNumberOfBalls(BallsList.Count);
    }

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}