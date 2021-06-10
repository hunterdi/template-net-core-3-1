using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Domains.Character
{
    public class Character
    {
        public ICollection<CharacterAttribute> Attributes { get; set; }
        
        public Character()
        {
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Strength,
                Type = AttributeType.Physical
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Constitution,
                Type = AttributeType.Physical
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Dexterity,
                Type = AttributeType.Physical
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Agility,
                Type = AttributeType.Physical
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Charisma,
                Type = AttributeType.Mental
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Willpower,
                Type = AttributeType.Mental
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Inteligence,
                Type = AttributeType.Mental
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Wisdom,
                Type = AttributeType.Mental
            });
            this.Attributes.Add(new CharacterAttribute
            {
                Value = 1,
                Attribute = Attribute.Soul,
                Type = AttributeType.Spirit
            });
        }
    }
}
