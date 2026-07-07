using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework9
{
    class SkillType
    {
        public int typeId { get; }
        public string name { get; }
        public string description { get; }

        public SkillType(int typeId, string name, string description)
        {
            this.typeId = typeId;
            this.name = name;
            this.description = description;
        }
    }

    class SkillData
    {
        public SkillType[] skillList { get; }

        public SkillData()
        {
            this.skillList = new SkillType[5]
            {
                new SkillType(0, "발차기", "공격력의 두 배 피해를 줍니다"),
                new SkillType(1, "회복", "HP를 20 회복합니다"),
                new SkillType(2, "회전 박치기", "공격력의 두 배 피해를 줍니다"),
                new SkillType(3, "산성 공격", "공격력의 두 배 피해를 줍니다"),
                new SkillType(4, "바닥 부수기", "공격력의 두 배 피해를 줍니다")
            };
        }
    }

    abstract class Skill
    {
        public abstract void use(Creature target, Creature caster);
    }

    class Kick : Skill
    {
        public string name;
        public string description;

        public Kick()
        {
            SkillData sd = new SkillData();
            this.name = sd.skillList[0].name;
            this.description = sd.skillList[0].description;
        }

        public override void use(Creature target, Creature caster)
        {
            Console.WriteLine($"{caster.name} 이(가) {this.name}을(를) {target.name} 에게 시전했다");
            target.getDamage(10);
        }
    }

    class Heal : Skill
    {
        public string name;
        public string description;

        public Heal()
        {
            SkillData sd = new SkillData();
            this.name = sd.skillList[1].name;
            this.description = sd.skillList[1].description;
        }

        public override void use(Creature target, Creature caster)
        {
            Console.WriteLine($"{caster.name} 이(가) {this.name}을(를) {target.name} 에게 시전했다");
            target.getDamage(10);
        }
    }

    class Sonic : Skill
    {
        public string name;
        public string description;

        public Sonic()
        {
            SkillData sd = new SkillData();
            this.name = sd.skillList[2].name;
            this.description = sd.skillList[2].description;
        }

        public override void use(Creature target, Creature caster)
        {
            Console.WriteLine($"{caster.name} 이(가) {this.name}을(를) {target.name} 에게 시전했다");
            target.getDamage(10);
        }
    }

    class AcidBeam : Skill
    {
        public string name;
        public string description;

        public AcidBeam()
        {
            SkillData sd = new SkillData();
            this.name = sd.skillList[3].name;
            this.description = sd.skillList[3].description;
        }

        public override void use(Creature target, Creature caster)
        {
            Console.WriteLine($"{caster.name} 이(가) {this.name}을(를) {target.name} 에게 시전했다");
            target.getDamage(10);
        }
    }
    class GroundCrusher : Skill
    {
        public string name;
        public string description;

        public GroundCrusher()
        {
            SkillData sd = new SkillData();
            this.name = sd.skillList[4].name;
            this.description = sd.skillList[4].description;
        }

        public override void use(Creature target, Creature caster)
        {
            Console.WriteLine($"{caster.name} 이(가) {this.name}을(를) {target.name} 에게 시전했다");
            target.getDamage(10);
        }
    }
}
