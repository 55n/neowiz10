using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkness
{
    public class ArmorSpiritBehavior : IMonsterBehavior
    {
        private const string MagicStoneId = "magic_stone";

        private readonly Dictionary<int, string> itemIdsBySlot;
        private readonly Dictionary<int, int> pricesBySlot;
        private readonly HashSet<int> purchasedSlots;
        private readonly ItemData itemData;
        private bool discoveryMessagePlayed;

        public ArmorSpiritBehavior()
        {
            itemIdsBySlot = new Dictionary<int, string>
            {
                { 0, "snowflake_pendant" },
                { 1, "chain_sword" },
                { 3, "spirit_armor" },
                { 4, "memory_charm" }
            };
            pricesBySlot = new Dictionary<int, int>
            {
                { 0, 3 },
                { 1, 4 },
                { 3, 4 },
                { 4, 5 }
            };
            purchasedSlots = new HashSet<int>();
            itemData = new ItemData();
            discoveryMessagePlayed = false;
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            if (monster.State == MonsterState.Combat)
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.Attack(),
                    NarrativeTokens.Actor + "이(가) 사슬을 휘두르며 달려든다.");
            }

            bool targetsArmorSpirit =
                action.TargetSlot == perception.CurrentSlot;

            bool searchedArmorSpirit =
                action.Action == PlayerActionType.Search &&
                targetsArmorSpirit;
            if (searchedArmorSpirit && !discoveryMessagePlayed)
            {
                discoveryMessagePlayed = true;
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "\"그래, 그래! 이제야 찾았군! 놀라지 말게. " +
                    "해칠 생각은 없으니!\"");
            }

            bool isRevealed =
                perception.CurrentSlot.State == SlotState.REVEALED;
            if (!isRevealed &&
                action.Action == PlayerActionType.Talk &&
                targetsArmorSpirit)
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "\"여기, 여기일세! 자네가 보고 있는 방향 " +
                    "그대로 가운데로 와보게나!\"");
            }

            if (!isRevealed &&
                action.Action == PlayerActionType.Wait)
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "\"이보게, 그렇게 서 있지만 말고 " +
                    "이쪽으로 좀 와보게나!\"");
            }

            if (!isRevealed &&
                IsMagicStoneThrownAtArmorSpirit(
                    action,
                    perception.CurrentSlot))
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "\"마석인가? 좋지, 좋지만 우선 이쪽으로 와서 " +
                    "이야기부터 듣게나!\"");
            }

            if (action.Action == PlayerActionType.Talk &&
                targetsArmorSpirit)
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    BuildTradeMessage(perception.CurrentSlot));
            }

            if (action.Action == PlayerActionType.ThrowItem &&
                targetsArmorSpirit && action.Item != null &&
                action.Item.Item != null)
            {
                if (action.Item.Item.Type.Id == MagicStoneId)
                {
                    return new MonsterDecision(
                        monster.State,
                        MonsterActionPlan.Wait(),
                        BuildPaymentMessage(perception.CurrentSlot));
                }

                return BecomeHostile(
                    "\"아니, 아니! 마석을 던지라 했지, " +
                    "그걸 던지라곤 안 했네! 이제 거래는 끝일세!\"");
            }

            if (targetsArmorSpirit && IsHostileAction(action))
            {
                return BecomeHostile(
                    "\"해칠 생각은 없다더니 거짓말이었나! " +
                    "좋네, 그렇다면 나도 가만있진 않겠네!\"");
            }

            if (action.Action == PlayerActionType.Search)
            {
                int slotIndex = perception.CurrentRoom.Slots.IndexOf(
                    action.TargetSlot);
                int price;
                if (!pricesBySlot.TryGetValue(slotIndex, out price) ||
                    purchasedSlots.Contains(slotIndex))
                {
                    return MonsterDecision.None(monster.State);
                }

                int payment = CountMagicStones(
                    perception.CurrentSlot.GroundInventory);
                if (payment < price)
                {
                    return BecomeHostile(
                        "\"잠깐, 잠깐! 값도 모자라는데 " +
                        "내 물건에 손을 대다니! 도둑이었군!\"");
                }

                ConsumeMagicStones(
                    perception.CurrentSlot.GroundInventory,
                    price);
                purchasedSlots.Add(slotIndex);
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "\"그래, 그래! 값은 제대로 받았네. " +
                    "그 물건은 이제 자네 것이야!\"");
            }

            return MonsterDecision.None(monster.State);
        }

        private string BuildTradeMessage(RoomSlot armorSpiritSlot)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(
                "\"오, 왔군! 잘 듣게나. 상자마다 쓸 만한 물건이 들어 있네!\"");

            foreach (int slotIndex in new[] { 0, 1, 3, 4 })
            {
                ItemType itemType = itemData.ItemTypes[
                    itemIdsBySlot[slotIndex]];
                message.Append("\"")
                    .Append(GetSlotName(slotIndex))
                    .Append("에는 ")
                    .Append(itemType.Name)
                    .Append("이(가) 있네. ")
                    .Append(itemType.Description)
                    .Append(" 값은 마석 ")
                    .Append(pricesBySlot[slotIndex])
                    .AppendLine("개일세.\"");
            }

            int payment = CountMagicStones(
                armorSpiritSlot.GroundInventory);
            message.Append("\"마석은 내게 던지면 되네! 지금까지 ")
                .Append(payment)
                .Append("개를 받았어. 값을 치른 뒤에만 상자를 열게나!\"");
            return message.ToString();
        }

        private static string BuildPaymentMessage(RoomSlot armorSpiritSlot)
        {
            int payment = CountMagicStones(
                armorSpiritSlot.GroundInventory);
            return "\"좋아, 좋아! 마석은 잘 받았네! 지금 " +
                   payment + "개일세. 원하는 물건의 값이 되면 " +
                   "그 상자를 열게나!\"";
        }

        private static bool IsHostileAction(PlayerActionContext action)
        {
            return action.Action == PlayerActionType.Attack ||
                   action.Action == PlayerActionType.UseSkill;
        }

        private static bool IsMagicStoneThrownAtArmorSpirit(
            PlayerActionContext action,
            RoomSlot armorSpiritSlot)
        {
            return action.Action == PlayerActionType.ThrowItem &&
                   action.TargetSlot == armorSpiritSlot &&
                   action.Item != null &&
                   action.Item.Item != null &&
                   action.Item.Item.Type.Id == MagicStoneId;
        }

        private static MonsterDecision BecomeHostile(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(),
                message);
        }

        private static int CountMagicStones(Inventory inventory)
        {
            return inventory.ItemStacks
                .Where(stack =>
                    stack.Item.Type.Id == MagicStoneId)
                .Sum(stack => stack.Count);
        }

        private static void ConsumeMagicStones(
            Inventory inventory,
            int amount)
        {
            int remaining = amount;
            foreach (ItemStack stack in inventory.ItemStacks
                         .Where(itemStack =>
                             itemStack.Item.Type.Id == MagicStoneId)
                         .ToArray())
            {
                int removed = inventory.Discard(stack, remaining);
                remaining -= removed;
                if (remaining == 0)
                {
                    return;
                }
            }
        }

        private static string GetSlotName(int slotIndex)
        {
            switch (slotIndex)
            {
                case 0:
                    return "맨 왼쪽 상자";
                case 1:
                    return "왼쪽 상자";
                case 3:
                    return "오른쪽 상자";
                case 4:
                    return "맨 오른쪽 상자";
                default:
                    return "상자";
            }
        }
    }
}
