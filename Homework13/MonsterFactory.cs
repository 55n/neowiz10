using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework13
{
    abstract class CreatureFactory
    {
        public abstract Creature CreateCreature();
    }

    class SlugFactory : CreatureFactory
    {
        public override Creature CreateCreature()
        {
            return new Slug();
        }
    }
    class SlimeFactory : CreatureFactory
    {
        public override Creature CreateCreature()
        {
            return new Slime();
        }
    }
    class MushMomFactory : CreatureFactory
    {
        public override Creature CreateCreature()
        {
            return new MushMom();
        }
    }
    
}
