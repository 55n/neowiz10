using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Darkness
{
    public static class Narrative
    {

        public static string DamageTaken(string enemy, string damage)
        {
            return $"[{enemy}]에게 {damage}의 데미지를 받았습니다";
        }

        public static string SkillActivated(string skill)
        {
            return $"[{skill}] 스킬이 발동됩니다.";
        }

        public static string[] FirstRoomScript()
        {
            return new string[]
            {
                "아무것도 보이지 않는다",
                "장비가 어딘가로 날아간 것 같다",
                "소지품이 부서진 것 같다",
                "당신은 숨을 죽이고 주변을 살피려고 했다",
                "어둠에 시야가 적응하도록 기다렸다",
                "여전히 아무것도 보이지 않는다",
                "당신은 손을 더듬어 바닥을 살핀다",
                "무언가가 손에 잡혔다!",
            };
        }

        public static string EquipmentBroken(string equipment)
        {
            return $"[{equipment}] 이(가) 파손되었습니다";
        }

        public static string GetItem(string item)
        {
            return $"[{item}] 을(를) 얻었습니다";
        }

        public static string[] DefaultRoomEnterMessage()
        {
            return new string[] { "공간에는 적막이 맴돌고 있다", "당신은 신중하게 탐색을 시작했다" };
        }

        public static string[] SecondRoomEnterMessages()
        {
            return new string[]
            {
                "기척이 느껴진다",
                "무언가가 긁는 소리를 낸다"
            };
        }

        public static string MonsterAction()
        {
            return "??? 은(는) 크르륵 거린다";
        }

        public static string Goblin()
        {
            return "고블린";
        }

        public static string MonsterAttackStarted()
        {
            return "어둠 속의 정체가 드러났다. 고블린이 달려든다";
        }

        public static string PlayerDodged()
        {
            return "당신은 가까스로 공격을 피했다";
        }

        public static string PlayerHit(string monster, int damage)
        {
            return $"{monster}의 공격이 스쳤다. 생명력 {damage}을(를) 잃었다";
        }

        public static string PlayerDied()
        {
            return "당신은 어둠 속에서 쓰러졌다";
        }

        public static string MonsterReaction(int selectedEncounterAction, bool itemThrown)
        {
            switch (selectedEncounterAction)
            {
                case -1:
                    if (itemThrown)
                    {
                        return "던져진 물건이 부딪히자 ??? 이(가) 그쪽으로 짧게 달려든다";
                    }

                    return "소지품을 뒤적이는 소리에 ??? 이(가) 신경질적으로 긁어댄다";
                case 0:
                    return "긁는 소리가 잠깐 멎는다. 놈도 당신의 숨을 듣고 있다";
                case 1:
                    return "어둠 속의 무언가가 바닥을 긁으며 한 칸 가까워진 듯하다";
                case 2:
                    return "당신이 다가서자 거친 숨소리가 낮게 깔리고, 날카로운 것이 바닥을 찍는다";
                case 3:
                    return "소리에 반응해 ??? 이(가) 고개를 튼 듯 긁는 방향이 바뀐다";
                case 4:
                    return "벽을 더듬는 소리에 맞춰 ??? 이(가) 조심스럽게 위치를 바꾼다";
                case 5:
                    return "당신의 공격 의도를 느낀 듯 ??? 이(가) 먼저 튀어나올 준비를 한다";
                case 6:
                    return "물러나는 기척을 따라 ??? 이(가) 느리게 추적해 온다";
                default:
                    return MonsterAction();
            }
        }

        public static string[] MonsterEncounterActions()
        {
            return new string[]
            {
                "기척에 집중한다",
                "기다린다",
                "살짝 다가간다",
                "소리를 낸다",
                "손을 뻗어 더듬는다",
                "전투준비",
                "돌아간다"
            };
        }

        public static string[] CombatActions()
        {
            return new string[]
            {
                "돌아가기",
                "공격하기",
                "방어하기",
                "스킬사용"
            };
        }

        public static string NormalAttackResult(string monster, int damage)
        {
            return $"당신은 {monster} 을(를) 공격해 {damage}의 데미지를 주었다";
        }

        public static string SkillAttackResult(string skill, string monster, int damage)
        {
            return $"당신은 {skill} 을(를) 사용해 {monster} 에게 {damage}의 데미지를 주었다";
        }

        public static string MonsterDefeated(string monster)
        {
            return $"{monster} 이(가) 쓰러졌다";
        }

        public static string DoorBehindGoblinFound()
        {
            return "고블린이 쓰러지자 그 뒤에 가려져 있던 문이 드러났다";
        }

        public static string LootTaken(string item)
        {
            return $"{item} 을(를) 얻었다";
        }

        public static string NothingToLoot()
        {
            return "더 얻을 것은 없다";
        }

        public static string NotImplementedYet()
        {
            return "아직 구현되지 않았다";
        }

        public static string[] InventoryActions()
        {
            return new string[]
            {
                "선택 취소",
                "사용하기",
                "던지기"
            };
        }

        public static string[] ThrowItemPrompt(string item)
        {
            return new string[]
            {
                $"{item} 을(를) 던집니까?"
            };
        }

        public static string[] ThrowItemActions()
        {
            return new string[]
            {
                "던진다",
                "도로 집어넣는다"
            };
        }

        public static string ItemThrown(string item)
        {
            return $"당신은 {item} 을(를) 던졌다";
        }

        public static string[] EquipmentActions()
        {
            return new string[]
            {
                "선택 취소",
                "장비",
                "던지기"
            };
        }

        public static string BackAction()
        {
            return "뒤로가기";
        }

        public static string[] MoveActions()
        {
            return new string[]
            {
                "방에 머문다",
                "앞으로 간다"
            };
        }

        public static string[] BranchMoveActions()
        {
            return new string[]
            {
                "방에 머문다",
                "앞으로 간다",
                "왼쪽으로 간다",
                "오른쪽으로 간다"
            };
        }

        public static string Door()
        {
            return "문";
        }

        public static string EmptySlotFound()
        {
            return "아무것도 없다";
        }

        public static string DoorFound()
        {
            return "문을 발견했다";
        }

        public static string[] RestImage()
        {
            return new string[]
            {

                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⢠⢪⡪⡢⡂⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⢠⢔⣖⢖⣕⢗⢗⢗⣝⢮⣳⣳⣳⡳⣜⢜⢎⢣⢣⡣⣥⣀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⡹⣕⣗⢗⣗⣽⡹⣕⣗⢗⣗⣳⡳⣝⢮⡻⣦⡳⣽⡺⣮⡺⠀⠀⠀⠀⠀",
                "⠀⠀⠀⢜⡧⡯⣗⣗⣗⣯⡳⣯⣻⣺⣺⢝⡾⡵⣝⡷⡝⠾⡝⡇⢯⠀⠀⠀⠀⠀",
                "⠀⠀⡰⣕⣝⢝⣗⣗⣗⡷⣝⡷⣽⣺⣺⢕⡯⣯⠪⣩⢪⢑⠪⣞⣞⠀⠀⠀⠀⠀",
                "⠀⠀⡯⡮⣪⡳⣕⢗⢗⡯⣗⢽⣳⣳⢯⣳⢯⢷⢽⢽⡢⡣⡣⠵⡽⡀⠀⠀⠀⠀",
                "⠀⠀⠉⡚⠘⠸⣪⡫⡳⣝⡷⣽⣺⣺⡳⣽⢽⡽⡽⣝⣧⢑⢐⠌⣟⡆⠀⠀⠀⠀",
                "⠀⢠⣸⢀⠠⠐⠈⠘⣹⣺⡳⣳⣳⢷⢝⡷⣽⣺⢽⡺⡾⡜⡐⠌⢺⣺⡀⠀⠀⠀",
                "⠈⠑⠉⠁⠀⠐⠀⠄⢳⢷⢝⣗⢯⠯⠳⠫⠳⠯⢳⢽⣫⣗⠅⡅⠕⠓⠝⠆⠄⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠋⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢌⠢⡁⠂⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠑⠈⠀⠀⠀⠀⠀"
            };
        }

        public static string[] ExitImage()
        {
            return new string[]
            {
                "                 ;============",
                "      =@@@@@@@@@@@@@@@@@@@@@@@",
                "      =@------------------@@@@",
                "      =@~               @@@@@@",
                "      =@~*             @@@@@@@",
                "=@@@; =@~*            @@@@@@@@",
                " *@@@:  ;@~         @@@@@@@@@@",
                "    ; ;::::~.     =@@@@@@@@@@@",
                "    #@@@@@=     =@@@@@@@@@@@@@",
                "  @@@@@@ *@!    =@@@@@@@@@@@@@",
                "@@     @@,      =@@@@@@@@@@@@@",
                "     @@@=@      =@@@@@@@@@@@@@",
                "       @@@.     =@@@@@@@@@@@@ ",
                ",   @@@~        =@@@@@@@@@@   ",
                "; @@$@;         =@@@@@@@@     ",
                ".@@.  @!        =@@@@@@@      ",
                "!@-    #@##$    =@@@@@        ",
                "!@,       ,!=$# =@@@          ",
            };
        }
    }
}
