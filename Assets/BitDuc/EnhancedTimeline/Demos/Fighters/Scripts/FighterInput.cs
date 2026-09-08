namespace BitDuc.EnhancedTimeline.Demos
{
    public interface FighterInput
    {
        bool Left { get; }
        bool Right { get; }
        bool Attack { get; }
        bool Block { get; }
    }
}