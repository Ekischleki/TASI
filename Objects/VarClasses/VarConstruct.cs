﻿namespace TASI
{
    public class VarConstruct
    {

        public static bool operator ==(VarConstruct a, VarConstruct b)
        {
            if (a.type != b.type)
                return false;
            if (a.type == VarType.@object && !ReferenceEquals(a.ObjectDefinition, b.ObjectDefinition))
                return false;
            return true;
        }
        public static bool operator !=(VarConstruct a, VarConstruct b)
        {
            
            return !(a == b);
        }

        public enum VarType
        {
            num, @string, @bool, @void, @int, list, all, @object
        }
        private TASIObjectDefinition? objectDefinition;

        


        public TASIObjectDefinition ObjectDefinition
        {
            get
            {
                if (objectDefinition == null)
                {
                    if (type == VarType.@object)
                    {
                        throw new InternalInterpreterException("Internal: Object definition for Var def was null");
                    } 
                    else
                    {
                        throw new InternalInterpreterException("Internal: Tryed to access Object definition of non object var type");
                    }
                }
                return objectDefinition;
            }
        }

        public string name;
        public VarType type;
        public bool isLink;
        public bool isConstant;

        public VarConstruct(TASIObjectDefinition objectDefinition)
        {
            this.name = "";
            this.type = VarType.@object;
            this.isLink = false;
            this.isConstant = false;
            this.objectDefinition = objectDefinition;
        }

        public VarConstruct(VarType type, string name, bool isLink = false, bool isConst = false)
        {
            this.name = name.ToLower();
            this.type = type;
            this.isLink = isLink;
            this.isConstant = isConst;
        }
    }
}