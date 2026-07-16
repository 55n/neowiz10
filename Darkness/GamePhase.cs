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

        public void Exploration()
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
            const int backAction = 0;
            const int searchAction = 1;
            const int stayInRoomAction = 0;
            const int useInventoryAction = 4;
            const int findPathAction = 5;

            EncounterScreen.DrawSlots(View.Display, _roomSlots, _revealedSlots);
            string roomMessage = Narrative.DefaultRoomEnterMessage();

            while (true)
            {
                View.Message.Draw(roomMessage);
                bool canMove = !string.IsNullOrEmpty(_roomSlots[2]);
                bool canExplore = _revealedSlots.Any(revealed => !revealed);
                int selectedRoomAction = Selection.ChooseLeft(
                    View.Message,
                    Narrative.RoomActions(canMove, canExplore),
                    1);

                if (selectedRoomAction == statusAction)
                {
                    StatusScreen.Show(
                        View.Message,
                        1,
                        10,
                        1,
                        5,
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
                    InventoryScreen.Show(View.Message, _itemStacks, 4);
                    continue;
                }

                int moveAction = canExplore ? 3 : 2;
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
                    return;
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

                return;
            }

            string[] secondRoomSlots = new string[5];
            bool[] revealedSecondRoomSlots = new bool[5];
            secondRoomSlots[2] = "???";

            EncounterScreen.DrawSlots(
                View.Display,
                secondRoomSlots,
                revealedSecondRoomSlots);
            string[] secondRoomMessages = Narrative.SecondRoomEnterMessages();
            Utility.PlayMessages(secondRoomMessages);

            while (true)
            {
                View.Message.Draw(secondRoomMessages);
                bool canExplore = revealedSecondRoomSlots.Any(revealed => !revealed);
                int selectedRoomAction = Selection.ChooseLeft(
                    View.Message,
                    Narrative.RoomActions(false, canExplore),
                    secondRoomMessages.Length);

                if (selectedRoomAction == statusAction)
                {
                    StatusScreen.Show(
                        View.Message,
                        1,
                        10,
                        1,
                        5,
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
                    InventoryScreen.Show(View.Message, _itemStacks, 4);
                    continue;
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
                    InventoryScreen.Show(View.Message, _itemStacks, 4);
                    EncounterScreen.DrawSlots(
                        View.Display,
                        secondRoomSlots,
                        revealedSecondRoomSlots);
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

                Utility.PlayMessage(Narrative.MonsterAction());
            }
        }
    }
}
