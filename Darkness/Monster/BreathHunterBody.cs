using System.Collections.Generic;

namespace Darkness
{
    public class BreathHunterBody
    {
        private readonly Dictionary<BreathHunterPartRole, Monster> parts;

        public int CombatTurn { get; private set; }

        public BreathHunterBody()
        {
            parts = new Dictionary<BreathHunterPartRole, Monster>();
        }

        public void AddPart(
            BreathHunterPartRole role,
            Monster monster)
        {
            parts[role] = monster;
        }

        public bool IsAlive(BreathHunterPartRole role)
        {
            Monster monster;
            return parts.TryGetValue(role, out monster) &&
                   monster != null && monster.IsAlive;
        }

        public bool IsAbdomenAlive
        {
            get { return IsAlive(BreathHunterPartRole.Abdomen); }
        }

        public void AdvanceTurn()
        {
            CombatTurn++;
        }
    }

    public enum BreathHunterPartRole
    {
        LeftForeleg,
        Head,
        Abdomen,
        Spinneret,
        RightForeleg
    }
}
