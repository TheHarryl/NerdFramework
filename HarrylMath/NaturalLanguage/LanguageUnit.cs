using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public enum LanguageUnitType
    {
        Adjective   = 1 << 0,
        Adverb      = 1 << 1,
        Conjunction = 1 << 2,
        Determiner  = 1 << 3,
        Noun        = 1 << 4,
        Preposition = 1 << 5,
        Pronoun     = 1 << 6,
        Verb        = 1 << 7,
        NullTerm    = 1 << 8
    }

    public struct LanguageUnit
    {
        public string value;
        public LanguageUnitType type;

        public List<LanguageUnit> children;

        public LanguageUnit(string value, LanguageUnitType type, List<LanguageUnit> children)
        {
            this.value = value;
            this.type = type;
            this.children = children;
        }
        
        public LanguageUnit(string value, List<LanguageUnit> children)
        {
            this.value = value;
            this.type = (LanguageUnitType)1;
            this.children = children;
        }

        public LanguageUnit(string value)
        {
            this.value = value;
            this.type = (LanguageUnitType)1;
            this.children = new List<LanguageUnit>();
        }

        // Returns true if shifts back to first type
        public bool ShiftType()
        {
            this.type = (LanguageUnitType)((int)this.type << 1);
            if (this.type == LanguageUnitType.NullTerm)
            {
                this.type = (LanguageUnitType)(1 << 0);
                return true;
            }
            return false;
        }

        // Returns true if shifts selected child back to first type
        public bool ShiftChildrenType(int index)
        {
            bool overflow = false;
            while (index >= 0 && children[index].ShiftType())
            {
                index--;
                overflow = true;
            }
            return overflow;
        }

        public bool IsValid()
        {
            return false;
        }

        public bool IsChildrenValid()
        {
            return false;
        }
    }
}
