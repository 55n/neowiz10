using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Darkness
{
    public class GamePhase
    {
        private readonly string[] _roomSlots = new string[5];
        private readonly bool[] _revealedSlots = new bool[5];
        private readonly List<string> _itemStacks = new List<string>
        {
            "깨진 약병 x2"
        };
        private readonly Random _random = new Random();
        private const int MaxHealth = 10;
        private const int MaxFocus = 5;
        private const int NormalAttackDamage = 10;
        private const int FocusSkillDamage = NormalAttackDamage * 2;
        private const int GoblinAttackDamage = 5;
        private int _health = 1;
        private int _focus = 1;
        private int _goblinHealth = 10;
        private bool _goblinLooted;
        private readonly List<string> _goblinItemStacks = new List<string>
        {
            "작은 마석"
        };

        public GameSignal Intro()
        {
            string[] introImage = Narrative.IntroImage();

            View.Display.DrawCenteredLineByLine(introImage);
            Utility.PlayMessages(Narrative.Instruction());
            Utility.PlayMessages(Narrative.IntroNarration());

            string[] title = Narrative.Title();
            View.Message.DrawCentered(title);
            Thread.Sleep(700);
            int selected = Selection.Choose(
                View.Message,
                Narrative.MainMenu(),
                title.Length);

            if (selected == 0)
            {
                PlayFallingEffect(introImage.Length);
                return GameSignal.START_GAME;
            }
            else
            {
                Utility.PlayMessages(Narrative.EndNarration());
                return GameSignal.EXIT_GAME;
            }
            
        }

        private void PlayFallingEffect(int imageHeight)
        {
            View.Message.Clear();

            for (int markerRow = 0; markerRow < imageHeight; markerRow++)
            {
                View.Message.DrawLineCentered(markerRow, Narrative.FallMarker());

                int imageRow = imageHeight - 1 - markerRow * 2;
                if (imageRow >= 0)
                {
                    View.Display.ClearLine(imageRow);
                    if (imageRow - 1 >= 0)
                    {
                        View.Display.ClearLine(imageRow - 1);
                    }
                }

                Thread.Sleep(100);
            }

            View.Message.DrawLineCentered(imageHeight, Narrative.FallImpact());
            Thread.Sleep(1000);
        }

        public GameSignal Exploration()
        {
            string enemy = "낙하";
            string damage = "999999";
            string skill = "수호의 가호";
            string skillEffect = "남은 체력을 1로 변경합니다";
            string equipment = "수호의 부적";
            string item1 = "금이 간 수호의 부적";
            string item2 = "평범한 한손검";

            Utility.PlayMessage(Narrative.DamageTaken(enemy, damage));
            Utility.PlayMessages(new string[] { Narrative.SkillActivated(skill), skillEffect });
            Utility.PlayMessage(Narrative.EquipmentBroken(equipment));
            Utility.PlayMessage(Narrative.GetItem(item1));
            Utility.PlayMessages(Narrative.FirstRoomScript());
            Utility.PlayMessage(Narrative.GetItem(item2));

            const int exploreAction = 2;
            const int statusAction = 0;
            const int inventoryAction = 1;
            const int lootAction = 3;
            const int moveAction = 4;
            const int returnAction = 5;
            const int backAction = 0;
            const int searchAction = 1;
            const int stayInRoomAction = 0;
            const int moveForwardAction = 1;
            const int moveLeftAction = 2;
            const int moveRightAction = 3;
            const int useInventoryAction = 4;
            const int findPathAction = 5;
            const int combatPreparationAction = 6;
            bool restUsed = false;
            string[] armorSpiritSlots = new string[5];
            bool[] revealedArmorSpiritSlots = new bool[5];
            armorSpiritSlots[2] = "???";
            string[] waterGhostSlots = new string[5];
            bool[] revealedWaterGhostSlots = new bool[5];
            waterGhostSlots[2] = "???";

        FirstRoom:
            EncounterScreen.DrawSlots(View.Display, _roomSlots, _revealedSlots);
            string roomMessage = Narrative.DefaultRoomEnterMessage();

            while (true)
            {
                View.Message.Draw(roomMessage);
                bool canMove = !string.IsNullOrEmpty(_roomSlots[2]);
                bool canExplore = _revealedSlots.Any(revealed => !revealed);
                int selectedRoomAction = Selection.ChooseLeft(
                    View.Message,
                    BuildRoomActions(canExplore, false, canMove, false),
                    1,
                    0);

                if (selectedRoomAction == statusAction)
                {
                    StatusScreen.Show(
                        View.Message,
                        _health,
                        MaxHealth,
                        _focus,
                        MaxFocus,
                        new string[0],
                        new string[]
                        {
                            "[무기]: 평범한 한손검(2/5)",
                            "[방어구]: 평범한 갑옷(1/5)",
                            "[장신구]: 금이 간 수호의 부적"
                        });
                    continue;
                }

                if (selectedRoomAction == inventoryAction)
                {
                    InventoryOutcome inventoryOutcome = InventoryScreen.Show(
                        View.Message,
                        _itemStacks,
                        4,
                        View.Display,
                        _roomSlots,
                        _revealedSlots);
                    EncounterScreen.DrawSlots(View.Display, _roomSlots, _revealedSlots);
                    continue;
                }

                if (canMove && selectedRoomAction == moveAction)
                {
                    int selectedMoveAction = Selection.ChooseLeft(
                        View.Message,
                        Narrative.MoveActions());

                    if (selectedMoveAction == stayInRoomAction)
                    {
                        continue;
                    }

                    break;
                }

                if (!canExplore || selectedRoomAction != exploreAction)
                {
                    return GameSignal.EXIT_GAME;
                }

                int selectedSlot = EncounterScreen.ChooseSlot(
                    View.Display,
                    _roomSlots,
                    _revealedSlots);
                int selectedEncounterAction = Selection.ChooseLeft(
                    View.Message,
                    Narrative.EncounterActions());

                if (selectedEncounterAction == backAction)
                {
                    EncounterScreen.DrawSlots(View.Display, _roomSlots, _revealedSlots);
                    continue;
                }

                if (selectedEncounterAction == searchAction)
                {
                    _revealedSlots[selectedSlot] = true;
                    if (selectedSlot == 2)
                    {
                        _roomSlots[selectedSlot] = Narrative.Door();
                        roomMessage = Narrative.DoorFound();
                    }
                    else
                    {
                        roomMessage = Narrative.EmptySlotFound();
                    }

                    EncounterScreen.DrawSlots(View.Display, _roomSlots, _revealedSlots);
                    continue;
                }

                return GameSignal.EXIT_GAME;
            }

            string[] secondRoomSlots = new string[5];
            bool[] revealedSecondRoomSlots = new bool[5];
            secondRoomSlots[2] = "???";

            string[] secondRoomMessages = Narrative.SecondRoomEnterMessages();
            View.Display.Clear();
            Utility.PlayMessages(secondRoomMessages);

        SecondRoomJunction:
            EncounterScreen.DrawSlots(
                View.Display,
                secondRoomSlots,
                revealedSecondRoomSlots);

            while (true)
            {
                View.Message.Draw(secondRoomMessages);
                bool canExplore = revealedSecondRoomSlots.Any(revealed => !revealed);
                bool goblinDefeated = _goblinHealth <= 0;
                int selectedRoomAction = Selection.ChooseLeft(
                    View.Message,
                    BuildRoomActions(
                        canExplore,
                        goblinDefeated && !_goblinLooted,
                        goblinDefeated,
                        true),
                    secondRoomMessages.Length,
                    0);

                if (selectedRoomAction == statusAction)
                {
                    StatusScreen.Show(
                        View.Message,
                        _health,
                        MaxHealth,
                        _focus,
                        MaxFocus,
                        new string[0],
                        new string[]
                        {
                            "[무기]: 평범한 한손검(2/5)",
                            "[방어구]: 평범한 갑옷(1/5)",
                            "[장신구]: 금이 간 수호의 부적"
                        });
                    continue;
                }

                if (selectedRoomAction == inventoryAction)
                {
                    InventoryOutcome inventoryOutcome = InventoryScreen.Show(
                        View.Message,
                        _itemStacks,
                        4,
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    EncounterScreen.DrawSlots(
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    if (inventoryOutcome != InventoryOutcome.None)
                    {
                        GameSignal? gameSignal = ResolveMonsterTurn(
                            useInventoryAction,
                            inventoryOutcome == InventoryOutcome.Thrown,
                            secondRoomSlots,
                            revealedSecondRoomSlots);
                        if (gameSignal.HasValue)
                        {
                            return gameSignal.Value;
                        }
                    }
                    continue;
                }

                if (selectedRoomAction == lootAction)
                {
                    LootGoblin();
                    continue;
                }

                if (selectedRoomAction == moveAction)
                {
                    int selectedMoveAction = Selection.ChooseLeft(
                        View.Message,
                        Narrative.BranchMoveActions());

                    if (selectedMoveAction == stayInRoomAction)
                    {
                        continue;
                    }

                    if (selectedMoveAction == moveForwardAction)
                    {
                        goto RestRoom;
                    }

                    if (selectedMoveAction == moveLeftAction)
                    {
                        goto ArmorSpiritRoom;
                    }

                    if (selectedMoveAction == moveRightAction)
                    {
                        goto WaterGhostRoom;
                    }

                    continue;
                }

                if (selectedRoomAction == returnAction)
                {
                    goto FirstRoom;
                }

                if (!canExplore || selectedRoomAction != exploreAction)
                {
                    continue;
                }

                int selectedSlot = EncounterScreen.ChooseSlot(
                    View.Display,
                    secondRoomSlots,
                    revealedSecondRoomSlots);
                int selectedEncounterAction = Selection.ChooseLeft(
                    View.Message,
                    Narrative.MonsterEncounterActions());

                if (selectedEncounterAction == useInventoryAction)
                {
                    InventoryOutcome inventoryOutcome = InventoryScreen.Show(
                        View.Message,
                        _itemStacks,
                        4,
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    if (inventoryOutcome == InventoryOutcome.None)
                    {
                        EncounterScreen.DrawSlots(
                            View.Display,
                            secondRoomSlots,
                            revealedSecondRoomSlots);
                        continue;
                    }

                    EncounterScreen.DrawSlots(
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    GameSignal? gameSignal = ResolveMonsterTurn(
                        selectedEncounterAction,
                        inventoryOutcome == InventoryOutcome.Thrown,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    if (gameSignal.HasValue)
                    {
                        return gameSignal.Value;
                    }
                    continue;
                }
                else if (selectedEncounterAction == combatPreparationAction)
                {
                    GameSignal? combatSignal = HandleCombatPreparation(
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    EncounterScreen.DrawSlots(
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    if (combatSignal.HasValue)
                    {
                        return combatSignal.Value;
                    }
                    continue;
                }
                else if (selectedEncounterAction == findPathAction)
                {
                    revealedSecondRoomSlots[selectedSlot] = true;
                    string searchResult = selectedSlot == 2
                        ? secondRoomMessages[1]
                        : Narrative.EmptySlotFound();

                    EncounterScreen.DrawSlots(
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                    Utility.PlayMessage(searchResult);
                }
                else
                {
                    EncounterScreen.DrawSlots(
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
                }

                GameSignal? turnSignal = ResolveMonsterTurn(
                    selectedEncounterAction,
                    false,
                    secondRoomSlots,
                    revealedSecondRoomSlots);
                if (turnSignal.HasValue)
                {
                    return turnSignal.Value;
                }
            }

        RestRoom:
            View.Display.DrawCentered(Narrative.RestImage());
            Utility.PlayMessages(new string[]
            {
                "낮은 돌계단 끝에 바람이 거의 멎은 공간이 있다",
                "이곳에서는 아무것도 당신을 쫓아오지 않는 듯하다"
            });

            while (true)
            {
                View.Message.Draw("차가운 벽에 기대 잠시 숨을 고를 수 있다");
                int selectedRestAction = Selection.ChooseLeft(
                    View.Message,
                    new SelectionOption[]
                    {
                        new SelectionOption("상태창", true, ""),
                        new SelectionOption("소지품", true, ""),
                        new SelectionOption("휴식", !restUsed, ""),
                        new SelectionOption("되돌아가기", true, "")
                    },
                    1,
                    0);

                if (selectedRestAction == statusAction)
                {
                    StatusScreen.Show(
                        View.Message,
                        _health,
                        MaxHealth,
                        _focus,
                        MaxFocus,
                        new string[0],
                        new string[]
                        {
                            "[무기]: 평범한 한손검(2/5)",
                            "[방어구]: 평범한 갑옷(1/5)",
                            "[장신구]: 금이 간 수호의 부적"
                        });
                    View.Display.DrawCentered(Narrative.RestImage());
                    continue;
                }

                if (selectedRestAction == inventoryAction)
                {
                    InventoryScreen.Show(View.Message, _itemStacks, 4);
                    View.Display.DrawCentered(Narrative.RestImage());
                    continue;
                }

                if (selectedRestAction == exploreAction)
                {
                    _health = Math.Min(MaxHealth, _health + MaxHealth / 2);
                    _focus = MaxFocus;
                    restUsed = true;
                    Utility.PlayMessage("당신은 휴식했다. 생명력과 집중력이 회복되었다");
                    View.Display.DrawCentered(Narrative.RestImage());
                    continue;
                }

                goto SecondRoomJunction;
            }

        ArmorSpiritRoom:
            View.Display.Clear();
            Utility.PlayMessages(new string[]
            {
                "왼쪽 통로 끝에서 금속이 바닥을 긁는 소리가 난다",
                "사슬이 천천히 끌리고, 누군가 낮게 숨을 고른다"
            });
            EncounterScreen.DrawSlots(
                View.Display,
                armorSpiritSlots,
                revealedArmorSpiritSlots);

            while (true)
            {
                View.Message.Draw(new string[]
                {
                    "쇠 냄새와 오래된 마석의 기운이 느껴진다",
                    "무언가가 말없이 당신을 기다린다"
                });
                int selectedArmorAction = Selection.ChooseLeft(
                    View.Message,
                    Narrative.MonsterEncounterActions(),
                    2);

                if (selectedArmorAction == useInventoryAction)
                {
                    InventoryOutcome inventoryOutcome = InventoryScreen.Show(
                        View.Message,
                        _itemStacks,
                        4,
                        View.Display,
                        armorSpiritSlots,
                        revealedArmorSpiritSlots);
                    EncounterScreen.DrawSlots(
                        View.Display,
                        armorSpiritSlots,
                        revealedArmorSpiritSlots);
                    if (inventoryOutcome == InventoryOutcome.Thrown)
                    {
                        armorSpiritSlots[2] = "갑주령";
                        revealedArmorSpiritSlots[2] = true;
                        EncounterScreen.DrawSlots(
                            View.Display,
                            armorSpiritSlots,
                            revealedArmorSpiritSlots);
                        Utility.PlayMessage("갑주령은 떨어진 물건보다 당신의 마석 주머니에 관심을 둔다");
                    }
                    continue;
                }

                if (selectedArmorAction == combatPreparationAction)
                {
                    armorSpiritSlots[2] = "갑주령";
                    revealedArmorSpiritSlots[2] = true;
                    EncounterScreen.DrawSlots(
                        View.Display,
                        armorSpiritSlots,
                        revealedArmorSpiritSlots);
                    Utility.PlayMessage("갑주령은 검을 들지 않는다. 지금은 거래를 기다리는 듯하다");
                    continue;
                }

                if (selectedArmorAction == 7)
                {
                    goto SecondRoomJunction;
                }

                armorSpiritSlots[2] = "갑주령";
                revealedArmorSpiritSlots[2] = true;
                EncounterScreen.DrawSlots(
                    View.Display,
                    armorSpiritSlots,
                    revealedArmorSpiritSlots);

                if (selectedArmorAction == 0)
                {
                    Utility.PlayMessage("갑옷 안은 비어 있지만, 사슬 사이에서 낡은 목소리가 울린다");
                }
                else if (selectedArmorAction == 3)
                {
                    Utility.PlayMessage("\"마석이 있나.\" 갑주령이 처음으로 말을 건다");
                }
                else if (selectedArmorAction == findPathAction)
                {
                    Utility.PlayMessage("갑주령 뒤쪽 벽에는 막힌 통로의 흔적만 있다");
                }
                else
                {
                    Utility.PlayMessage("갑주령은 움직이지 않고 당신의 반응을 재고 있다");
                }
            }

        WaterGhostRoom:
            View.Display.Clear();
            Utility.PlayMessages(new string[]
            {
                "오른쪽 통로는 젖어 있고 발밑에서 물이 찰박인다",
                "어둠 속 어딘가에서 물방울이 거꾸로 떨어지는 듯한 소리가 난다"
            });
            EncounterScreen.DrawSlots(
                View.Display,
                waterGhostSlots,
                revealedWaterGhostSlots);

            while (true)
            {
                View.Message.Draw(new string[]
                {
                    "축축한 냄새가 폐 안쪽까지 들러붙는다",
                    "무언가가 물가를 자기 영역처럼 지키고 있다"
                });
                int selectedWaterAction = Selection.ChooseLeft(
                    View.Message,
                    Narrative.MonsterEncounterActions(),
                    2);

                if (selectedWaterAction == useInventoryAction)
                {
                    InventoryOutcome inventoryOutcome = InventoryScreen.Show(
                        View.Message,
                        _itemStacks,
                        4,
                        View.Display,
                        waterGhostSlots,
                        revealedWaterGhostSlots);
                    EncounterScreen.DrawSlots(
                        View.Display,
                        waterGhostSlots,
                        revealedWaterGhostSlots);
                    if (inventoryOutcome == InventoryOutcome.Thrown)
                    {
                        Utility.PlayMessage("던진 물건이 물을 치자 물귀신의 기척이 그쪽으로 미끄러진다");
                    }
                    continue;
                }

                if (selectedWaterAction == 7)
                {
                    goto SecondRoomJunction;
                }

                if (selectedWaterAction == 2 || selectedWaterAction == combatPreparationAction)
                {
                    waterGhostSlots[2] = "물귀신";
                    revealedWaterGhostSlots[2] = true;
                    EncounterScreen.DrawSlots(
                        View.Display,
                        waterGhostSlots,
                        revealedWaterGhostSlots);
                    Utility.PlayMessage("젖은 손이 발목을 노렸지만, 당신은 몸을 뒤로 뺐다");
                    continue;
                }

                waterGhostSlots[2] = "물귀신";
                revealedWaterGhostSlots[2] = true;
                EncounterScreen.DrawSlots(
                    View.Display,
                    waterGhostSlots,
                    revealedWaterGhostSlots);

                if (selectedWaterAction == 0)
                {
                    Utility.PlayMessage("물소리 아래에서 숨소리 같은 거품이 솟는다");
                }
                else if (selectedWaterAction == 3)
                {
                    Utility.PlayMessage("당신의 목소리가 물가에서 젖은 목소리로 되돌아온다");
                }
                else if (selectedWaterAction == findPathAction)
                {
                    Utility.PlayMessage("손끝에 문 대신 젖은 벽과 미끄러운 이끼만 닿는다");
                }
                else
                {
                    Utility.PlayMessage("물귀신은 물가를 벗어나지 않은 채 당신을 맴돈다");
                }
            }
        }

        private GameSignal? HandleCombatPreparation(
            string[] monsterSlots,
            bool[] revealedMonsterSlots)
        {
            const int backAction = 0;
            const int attackAction = 1;
            const int defendAction = 2;
            const int skillAction = 3;
            const int skillBackAction = 0;
            const int focusSkillAction = 2;

            int selectedCombatAction = Selection.ChooseLeft(
                View.Message,
                Narrative.CombatActions());

            if (selectedCombatAction == backAction)
            {
                return null;
            }

            if (selectedCombatAction == defendAction)
            {
                Utility.PlayMessage(Narrative.NotImplementedYet());
                return null;
            }

            if (selectedCombatAction == attackAction)
            {
                ApplyPlayerAttack(
                    monsterSlots,
                    revealedMonsterSlots,
                    NormalAttackDamage,
                    null);
                if (_goblinHealth <= 0)
                {
                    return null;
                }

                return ResolveMonsterTurn(6, false, monsterSlots, revealedMonsterSlots);
            }

            if (selectedCombatAction != skillAction)
            {
                return null;
            }

            View.Message.Clear();
            int selectedSkill = Selection.ChooseLeft(
                View.Message,
                BuildSkillOptions(),
                2,
                0);

            if (selectedSkill == skillBackAction)
            {
                return null;
            }

            if (selectedSkill == focusSkillAction)
            {
                _focus = Math.Max(0, _focus - 1);
                ApplyPlayerAttack(
                    monsterSlots,
                    revealedMonsterSlots,
                    FocusSkillDamage,
                    "기척 추적");
                if (_goblinHealth <= 0)
                {
                    return null;
                }

                return ResolveMonsterTurn(6, false, monsterSlots, revealedMonsterSlots);
            }

            return null;
        }

        private SelectionOption[] BuildRoomActions(
            bool canExplore,
            bool canLoot,
            bool canMove,
            bool canReturn)
        {
            return new SelectionOption[]
            {
                new SelectionOption("상태창", true, ""),
                new SelectionOption("소지품", true, ""),
                new SelectionOption("탐색", canExplore, ""),
                new SelectionOption("루팅", canLoot, ""),
                new SelectionOption("이동", canMove, ""),
                new SelectionOption("되돌아가기", canReturn, "")
            };
        }

        private SelectionOption[] BuildSkillOptions()
        {
            return new SelectionOption[]
            {
                new SelectionOption(
                    "돌아가기",
                    true,
                    ""),
                new SelectionOption(
                    "수호의 가호",
                    false,
                    "죽음에 이르는 데미지를 받았을 때 한번 생명력을 1로 만든다"),
                new SelectionOption(
                    "기척 추적",
                    _focus >= 1,
                    "집중력 1을 사용해 고블린의 위치를 짚고 강하게 공격한다")
            };
        }

        private void LootGoblin()
        {
            if (_goblinLooted || _goblinItemStacks.Count == 0)
            {
                Utility.PlayMessage(Narrative.NothingToLoot());
                return;
            }

            foreach (string itemStack in _goblinItemStacks)
            {
                _itemStacks.Add(itemStack);
                Utility.PlayMessage(Narrative.LootTaken(itemStack));
            }

            _goblinItemStacks.Clear();
            _goblinLooted = true;
        }

        private void ApplyPlayerAttack(
            string[] monsterSlots,
            bool[] revealedMonsterSlots,
            int damage,
            string skillName)
        {
            monsterSlots[2] = Narrative.Goblin();
            revealedMonsterSlots[2] = true;
            EncounterScreen.DrawSlots(
                View.Display,
                monsterSlots,
                revealedMonsterSlots);

            _goblinHealth = Math.Max(0, _goblinHealth - damage);
            if (skillName == null)
            {
                Utility.PlayMessage(Narrative.NormalAttackResult(
                    Narrative.Goblin(),
                    damage));
            }
            else
            {
                Utility.PlayMessage(Narrative.SkillAttackResult(
                    skillName,
                    Narrative.Goblin(),
                    damage));
            }

            if (_goblinHealth <= 0)
            {
                Utility.PlayMessage(Narrative.MonsterDefeated(Narrative.Goblin()));
                monsterSlots[2] = Narrative.Door();
                revealedMonsterSlots[2] = true;
                EncounterScreen.DrawSlots(
                    View.Display,
                    monsterSlots,
                    revealedMonsterSlots);
                Utility.PlayMessage(Narrative.DoorBehindGoblinFound());
            }
        }

        private GameSignal? ResolveMonsterTurn(
            int selectedEncounterAction,
            bool itemThrown,
            string[] monsterSlots,
            bool[] revealedMonsterSlots)
        {
            if (_goblinHealth <= 0)
            {
                return null;
            }

            if (!MonsterAttacks(selectedEncounterAction, itemThrown))
            {
                Utility.PlayMessage(Narrative.MonsterReaction(
                    selectedEncounterAction,
                    itemThrown));
                return null;
            }

            monsterSlots[2] = Narrative.Goblin();
            revealedMonsterSlots[2] = true;
            EncounterScreen.DrawSlots(
                View.Display,
                monsterSlots,
                revealedMonsterSlots);
            Utility.PlayMessage(Narrative.MonsterAttackStarted());

            if (_random.Next(2) == 0)
            {
                Utility.PlayMessage(Narrative.PlayerDodged());
                return null;
            }

            _health = Math.Max(0, _health - GoblinAttackDamage);
            Utility.PlayMessage(Narrative.PlayerHit(
                Narrative.Goblin(),
                GoblinAttackDamage));

            if (_health <= 0)
            {
                Utility.PlayMessage(Narrative.PlayerDied());
                return GameSignal.EXIT_GAME;
            }

            return null;
        }

        private static bool MonsterAttacks(int selectedEncounterAction, bool itemThrown)
        {
            return selectedEncounterAction == 2 ||
                   selectedEncounterAction == 6 ||
                   itemThrown;
        }
    }
}
