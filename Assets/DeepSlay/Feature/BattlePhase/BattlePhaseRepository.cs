namespace DeepSlay
{
    public class BattlePhaseRepository
    {
        public BattlePhase BattlePhase { get; set; }
    }

    public enum BattlePhase
    {
        None,
        Draw,
        Roll,
        Discard,
        Merge,
        Attack,
    }
}