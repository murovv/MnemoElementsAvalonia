using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class CAbstractTransformer:BasicEquipment
    {
        private double _translationX;
        private double _translationY;
        public double TranslationX { 
            get => _translationX;
            protected set
            {
                _translationX = value;
            } }
        public double TranslationY { get => _translationY;
            protected set
            {
                _translationY = value;
            }  }

        public CAbstractTransformer()
        {
        }
        
       
    }
}