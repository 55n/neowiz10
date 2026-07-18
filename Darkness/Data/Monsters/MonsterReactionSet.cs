namespace Darkness
{
    public class MonsterReactionSet
    {
        public MonsterReaction Focus { get; private set; }
        public MonsterReaction Wait { get; private set; }
        public MonsterReaction Approach { get; private set; }
        public MonsterReaction MakeNoise { get; private set; }
        public MonsterReaction UseItem { get; private set; }
        public MonsterReaction Search { get; private set; }
        public MonsterReaction Attack { get; private set; }
        public MonsterReaction Retreat { get; private set; }

        public MonsterReactionSet(
            MonsterReaction focus,
            MonsterReaction wait,
            MonsterReaction approach,
            MonsterReaction makeNoise,
            MonsterReaction useItem,
            MonsterReaction search,
            MonsterReaction attack,
            MonsterReaction retreat)
        {
            Focus = focus;
            Wait = wait;
            Approach = approach;
            MakeNoise = makeNoise;
            UseItem = useItem;
            Search = search;
            Attack = attack;
            Retreat = retreat;
        }
    }
}
