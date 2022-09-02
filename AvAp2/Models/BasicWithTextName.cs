using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class BasicWithTextName:BasicMnemoElement
    {
        internal protected Pen PenBlack;
        internal protected Pen PenBlack1;
        internal protected Pen PenWhite1;
        public BasicWithTextName() : base()
        {
            PenBlack = new Pen(Brushes.Black, .5);
            PenBlack.ToImmutable(); 
            PenBlack1 = new Pen(Brushes.Black, 1);
            PenBlack1.ToImmutable();
            PenWhite1 = new Pen(Brushes.WhiteSmoke, 1);
            PenWhite1.ToImmutable();
        }
        
    }
}