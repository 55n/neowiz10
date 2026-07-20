namespace Darkness
{
    public static class SkillCostResolver
    {
        private const string MagicStoneItemId = "magic_stone";

        public static bool CanPay(ISkillUser user, SkillType skill)
        {
            if (user == null || skill == null)
            {
                return false;
            }

            if (skill.CostType == SkillCostType.Focus)
            {
                return user.CurrentFocus >= skill.CostAmount;
            }

            if (skill.CostType == SkillCostType.MagicStone)
            {
                ItemStack magicStones = FindMagicStones(user);
                return magicStones != null &&
                       magicStones.Count >= skill.CostAmount;
            }

            return false;
        }

        public static int CountMagicStones(ISkillUser user)
        {
            ItemStack magicStones = FindMagicStones(user);
            return magicStones == null ? 0 : magicStones.Count;
        }

        public static bool TryPay(ISkillUser user, SkillType skill)
        {
            if (!CanPay(user, skill))
            {
                return false;
            }

            if (skill.CostType == SkillCostType.Focus)
            {
                user.SpendFocus(skill.CostAmount);
                return true;
            }

            if (skill.CostType == SkillCostType.MagicStone)
            {
                ItemStack magicStones = FindMagicStones(user);
                return magicStones != null &&
                       user.Inventory.Discard(
                           magicStones,
                           skill.CostAmount) == skill.CostAmount;
            }

            return false;
        }

        private static ItemStack FindMagicStones(ISkillUser user)
        {
            if (user == null || user.Inventory == null)
            {
                return null;
            }

            return user.Inventory.ItemStacks.Find(
                stack => stack.Item.Type.Id == MagicStoneItemId);
        }
    }
}
