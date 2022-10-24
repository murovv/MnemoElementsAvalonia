using System;
using Avalonia;
using Avalonia.Threading;

namespace AvAp2.Models
{
    public class BlinkAnimationController:AvaloniaObject
    {
        private static BlinkAnimationController _blinkAnimationController = null;

        public double BlinkOpacity
        {
            get => GetValue(BlinkOpacityProperty);
            set => SetValue(BlinkOpacityProperty, value);
        }

        public static StyledProperty<double> BlinkOpacityProperty =
            AvaloniaProperty.Register<BlinkAnimationController, double>(nameof(BlinkOpacity), 0);

        private BlinkAnimationController()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1 / 10.0);
            double delta = 0.1;
            timer.Tick+= delegate(object? sender, EventArgs args)
            {
                BlinkOpacity += delta;
                if (BlinkOpacity >= 1)
                {
                    BlinkOpacity = 1;
                    delta = -delta;
                }
                if (BlinkOpacity <= 0)
                {
                    BlinkOpacity = 0;
                    delta = -delta;
                }
            };
            timer.Start();
        }

        public static BlinkAnimationController GetInstance()
        {
            if (_blinkAnimationController == null)
            {
                _blinkAnimationController = new BlinkAnimationController();
            }

            return _blinkAnimationController;

        }
    }
}