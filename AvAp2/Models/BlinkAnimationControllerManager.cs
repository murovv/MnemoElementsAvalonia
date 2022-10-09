using System;
using System.ComponentModel;
using System.Globalization;
using System.Reactive.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Threading;

namespace AvAp2.Models
{
    /*#region BlinkAnimationController Class
    /// <summary>
    /// Controller to animate syncronously blinking UI objects. 
    /// All Thread needed syncrhonous blinking should have a DataObjectProvider referring
    /// to this class. DataObjectProvider should call GetInstance() method for retrieving 
    /// an instance of this class. The BlinkAnimationController Instance is created into the calling Thread.
    /// </summary>
    public class BlinkAnimationController : INotifyPropertyChanged
    {
        private double blinkOpacity = 0;
        private Dispatcher callingDispatcherThread = null;

        /// <summary>
        /// The Property you should bind into UI Thread for Syncronizing Blinking
        /// </summary>
        public double BlinkOpacity
        {
            get => blinkOpacity;
            set { blinkOpacity = value; }
        }

        private BlinkAnimationController()
        {
            callingDispatcherThread = Dispatcher.UIThread;

            BlinkAnimationControllerManager bcm = BlinkAnimationControllerManager.GetInstance();
            bcm.BlinkOpacityChangedEvent += BlinkAnimationControllerManager_testEvent;
        }

        /// <summary>
        /// This method allow threads creating their own BlinkAnimationController instance.
        /// </summary>
        /// <returns></returns>
        public static BlinkAnimationController GetInstance()
        {
            /// Forcing Thread using this method for retriving BlinkAnimationController instance
            /// allow the main Thread having full control over it.
            return new BlinkAnimationController();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handling BlinkOpacityProperty changes from Main BlinkAnimationControllerManager and
        /// Change the BlinkOpacity of this class that is running into calling thread.
        /// </summary>
        /// <param name="blinkOpacityNewValue"></param>
        private void BlinkAnimationControllerManager_testEvent(double blinkOpacityNewValue)
        {
            if (PropertyChanged != null && callingDispatcherThread != null)
            {
                callingDispatcherThread.InvokeAsync(delegate
                {
                    try
                    {
                        blinkOpacity = blinkOpacityNewValue;
                        PropertyChanged(this, new PropertyChangedEventArgs("BlinkOpacity"));
                    }
                    catch (Exception) { }
                });
                /*callingDispatcherThread.BeginInvoke((ThreadStart)delegate
                {
                    try
                    {
                        blinkOpacity = blinkOpacityNewValue;
                        PropertyChanged(this, new PropertyChangedEventArgs("BlinkOpacity"));
                    }
                    catch (Exception) { }
                });#1#
            }
        }

    }
    #endregion*/

    #region BlinkAnimationControllerManager Class
    /// <summary>
    /// Univoque Controller to animate syncronously blinking UI objects
    /// </summary>
    public class BlinkAnimationControllerManager : Control
    {

        public delegate void BlinkOpacityChangedHandler(double blinkOpacityNewValue);
        public event BlinkOpacityChangedHandler BlinkOpacityChangedEvent;

        /// <summary>
        /// The Singleton BlinkAnimationControllerManager Object
        /// </summary>
        private static BlinkAnimationControllerManager? blinkAnimationControllerManager = null;

        //// Using a DependencyProperty as the backing store for BlinkOpacity.  
        //// This enables animation, styling, binding, etc...
        
        public double BlinkOpacity
        {
            get => GetValue(BlinkOpacityProperty);
            set => SetValue(BlinkOpacityProperty, value);
        }
        public static StyledProperty<double> BlinkOpacityProperty = AvaloniaProperty.Register<BlinkAnimationControllerManager, double>("BlinkOpacity", 0.0);
        /// <summary>
        /// Private - Avoid multiple instances.
        /// </summary>
        private BlinkAnimationControllerManager()
        {
            
            BlinkOpacityProperty.Changed.Subscribe(OnBlinkOpacityPropertyChanged);
            try
            {
                /*KeyFrame start = new KeyFrame();
                start.Cue = Cue.Parse("0%", CultureInfo.CurrentCulture);
                start.Setters.Add(new Setter(BlinkOpacityProperty, 0.0f));
                KeyFrame end = new KeyFrame();
                end.Cue = Cue.Parse("100%", CultureInfo.CurrentCulture);
                end.Setters.Add(new Setter(BlinkOpacityProperty, 1.0f));
                Animation BlinkAnimation = new Animation
                {
                    Duration = TimeSpan.FromSeconds(.5),
                    PlaybackDirection = PlaybackDirection.Alternate,
                    IterationCount = IterationCount.Infinite,
                    Children = {start,end}
                };
                var clock = new TestClock();
                
                BlinkAnimation.RunAsync(this, clock);
                clock.Step(TimeSpan.FromMilliseconds(200));*/

                #region KeyFrames

                //DiscreteDoubleKeyFrame ddkf1 = new DiscreteDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)));
                //DiscreteDoubleKeyFrame ddkf2 = new DiscreteDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300)));
                //DiscreteDoubleKeyFrame ddkf3 = new DiscreteDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(600)));
                //DiscreteDoubleKeyFrame ddkf4 = new DiscreteDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1000)));

                //BlinkAnimation.KeyFrames.Add(ddkf1);
                //BlinkAnimation.KeyFrames.Add(ddkf2);
                //BlinkAnimation.KeyFrames.Add(ddkf3);
                //BlinkAnimation.KeyFrames.Add(ddkf4); 

                #endregion KeyFrames

            }
            catch (Exception) { }
        }

        private void OnBlinkOpacityPropertyChanged(AvaloniaPropertyChangedEventArgs<double> obj)
        {
            if (blinkAnimationControllerManager == null)
                return;

            if (Math.Abs((double)obj.OldValue.Value - (double)obj.NewValue.Value) > 10e-9)
                blinkAnimationControllerManager.RaiseBlinkOpacityChangedEvent((double)obj.NewValue.Value);
        }

        /// <summary>
        /// This Class implements Singleton Pattern. Use this method for retriving the class instance.
        /// </summary>
        public static BlinkAnimationControllerManager? GetInstance()
        {
            
            if (blinkAnimationControllerManager == null)            
                blinkAnimationControllerManager = new BlinkAnimationControllerManager();
            return blinkAnimationControllerManager;
        }
        
        
        public static void ResetBlinkAnimationControllerManager()
        {
            blinkAnimationControllerManager = null;
        }

        private void RaiseBlinkOpacityChangedEvent(double blinkOpacityNewValue)
        {
            if (BlinkOpacityChangedEvent != null)
                BlinkOpacityChangedEvent(blinkOpacityNewValue);
        }

    }
    #endregion
}