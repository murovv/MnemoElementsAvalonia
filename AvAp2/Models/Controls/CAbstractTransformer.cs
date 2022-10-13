namespace AvAp2.Models
{
    public abstract class CAbstractTransformer:BasicEquipment
    {
        public double TranslationX { get;protected set; }
        public double TranslationY { get; protected set; }
        protected abstract void InitIsSelected();
        protected abstract void InitMouseOver();
    }
}