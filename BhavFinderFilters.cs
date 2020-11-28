using BhavFinder.DBPF.Bhav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BhavFinder
{
    public partial class BhavFinderForm : Form
    {
        private BhavFilter GetFilters(Dictionary<int, HashSet<uint>> strLookupByIndexLocal, Dictionary<int, HashSet<uint>> strLookupByIndexGlobal)
        {
            BhavFilter filter = new TrueFilter();

            if (comboBhavInGroup.Text.Length > 0)
            {
                Match m = HexGroupRegex.Match(comboBhavInGroup.Text);

                if (m.Success)
                {
                    filter = new GroupFilter(Convert.ToUInt32(m.Value, 16));
                }
            }

            if (comboOpCode.Text.Length > 0)
            {
                Match m = HexOpCodeRegex.Match(comboOpCode.Text);

                if (m.Success)
                {
                    uint opcode = Convert.ToUInt32(m.Value, 16);
                    int version = -1;

                    if (opcode > 0x2000)
                    {
                        if (comboOpCodeInGroup.Text.Length > 0)
                        {
                            Match g = HexGroupRegex.Match(comboOpCodeInGroup.Text);

                            if (g.Success)
                            {
                                filter = new SemiGlobalsFilter(Convert.ToUInt32(g.Value, 16), filter);
                            }
                        }
                    }

                    if (comboVersion.Text.Length > 0)
                    {
                        Match v = HexOpCodeRegex.Match(comboVersion.Text);

                        if (v.Success)
                        {
                            version = Convert.ToInt16(v.Value, 16);
                        }
                    }

                    filter.InstFilter = new OpCodeFilter(opcode, version);
                }
            }

            for (int i = 0; i <= 15; ++i)
            {
                if (operands[i].Text.Length > 0 && Hex2Regex.IsMatch(operands[i].Text))
                {
                    ushort value = Convert.ToUInt16(operands[i].Text, 16);
                    ushort mask = 0xFF;

                    if (masks[i].Text.Length > 0 && Hex2Regex.IsMatch(masks[i].Text))
                    {
                        mask = Convert.ToUInt16(masks[i].Text, 16);
                    }

                    InstructionFilter operandFilter = new OperandFilter(i, value, mask);

                    if (filter.InstFilter != null)
                    {
                        operandFilter.InnerFilter = filter.InstFilter;
                    }
                    filter.InstFilter = operandFilter;
                }
            }

            if (strLookupByIndexLocal != null && strLookupByIndexGlobal != null)
            {
                InstructionFilter strFilter = new StrIndexFilter(Convert.ToInt32(comboUsingOperand.Text, 10), strLookupByIndexLocal, strLookupByIndexGlobal);

                if (filter.InstFilter != null)
                {
                    strFilter.InnerFilter = filter.InstFilter;
                }

                filter.InstFilter = strFilter;
            }

            return filter;
        }

        private abstract class BhavFilter
        {
            public BhavFilter InnerFilter { get; set; } = null;
            public InstructionFilter InstFilter { get; set; } = null;

            public abstract Boolean wanted(Bhav bhav);

            public BhavFilter()
            {
            }

            public BhavFilter(BhavFilter innerFilter)
            {
                this.InnerFilter = innerFilter;
            }

            public BhavFilter(BhavFilter innerFilter, InstructionFilter instFilter)
            {
                this.InnerFilter = innerFilter;
                this.InstFilter = instFilter;
            }

            public Boolean isWanted(Bhav bhav)
            {
                if ((InnerFilter == null || InnerFilter.isWanted(bhav)) && wanted(bhav))
                {
                    if (InstFilter == null)
                    {
                        return true;
                    }

                    foreach (Instruction inst in bhav.Instructions)
                    {
                        if (InstFilter.isWanted(bhav.Group, inst))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private abstract class InstructionFilter
        {
            public InstructionFilter InnerFilter { get; set; } = null;

            public abstract Boolean wanted(uint group, Instruction inst);

            public InstructionFilter()
            {
            }

            public InstructionFilter(InstructionFilter innerFilter)
            {
                this.InnerFilter = innerFilter;
            }

            public Boolean isWanted(uint group, Instruction inst)
            {
                return ((InnerFilter == null || InnerFilter.isWanted(group, inst)) && wanted(group, inst));
            }
        }

        private class TrueFilter : BhavFilter
        {
            public override Boolean wanted(Bhav bhav)
            {
                return true;
            }
        }

        private class GroupFilter : BhavFilter
        {
            uint group;

            public GroupFilter(uint group)
            {
                this.group = group;
            }

            public override Boolean wanted(Bhav bhav)
            {
                return (bhav.Group == group);
            }
        }

        private class SemiGlobalsFilter : BhavFilter
        {
            uint semiglobals;

            public SemiGlobalsFilter(uint semiglobals, BhavFilter innerFilter)
            {
                this.semiglobals = semiglobals;
                this.InnerFilter = innerFilter;
            }

            public override Boolean wanted(Bhav bhav)
            {
                uint semigroup;

                return (semiglobalsByGroupID.TryGetValue(bhav.Group, out semigroup) && (semigroup == semiglobals));
            }
        }

        private class OpCodeFilter : InstructionFilter
        {
            uint opcode;
            int version = -1;

            public OpCodeFilter(uint opcode, int version)
            {
                this.opcode = opcode;
                this.version = version;
            }

            public override Boolean wanted(uint group, Instruction inst)
            {
                return (inst.OpCode == opcode && (version == -1 || inst.NodeVersion == version));
            }
        }

        private class OperandFilter : InstructionFilter
        {
            int operand;
            ushort value;
            ushort mask = 0xFF;

            public OperandFilter(int operand, ushort value, ushort mask)
            {
                this.operand = operand;
                this.value = value;
                this.mask = mask;
            }

            public override Boolean wanted(uint group, Instruction inst)
            {
                return ((inst.Operands[operand] & mask) == value);
            }
        }

        private class StrIndexFilter : InstructionFilter
        {
            private int operand;
            private Dictionary<int, HashSet<uint>> strLookupByIndexLocal;
            private Dictionary<int, HashSet<uint>> strLookupByIndexGlobal;

            public StrIndexFilter(int operand, Dictionary<int, HashSet<uint>> strLookupByIndexLocal, Dictionary<int, HashSet<uint>> strLookupByIndexGlobal)
            {
                this.operand = operand;
                this.strLookupByIndexLocal = strLookupByIndexLocal;
                this.strLookupByIndexGlobal = strLookupByIndexGlobal;
            }

            public override Boolean wanted(uint group, Instruction inst)
            {
                int index = inst.Operands[operand];
                HashSet<uint> groups;

                if (strLookupByIndexLocal != null && strLookupByIndexLocal.TryGetValue(index, out groups))
                {
                    // The group this BHAV is in?
                    if (groups.Contains(group)) return true;

                    // The semiglobals group this BHAV references?
                    uint semigroup;
                    if (semiglobalsByGroupID.TryGetValue(group, out semigroup) && groups.Contains(semigroup)) return true;
                }

                if (strLookupByIndexGlobal != null && strLookupByIndexGlobal.TryGetValue(index, out groups))
                {
                    // The group this BHAV is in?
                    if (groups.Contains(group)) return true;

                    // The globals group?
                    if (groups.Contains(0x7FD46CD0)) return true;

                    // The semiglobals group this BHAV references?
                    uint semigroup;
                    if (semiglobalsByGroupID.TryGetValue(group, out semigroup) && groups.Contains(semigroup)) return true;
                }

                return false;
            }
        }

    }
}
