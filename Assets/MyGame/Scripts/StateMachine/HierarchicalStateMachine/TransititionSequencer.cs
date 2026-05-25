using System.Collections.Generic;

public class TransititionSequencer
{
    public readonly HierarchicalStateMachine Machine;

    public TransititionSequencer(HierarchicalStateMachine machine)
    {
        Machine = machine;
    }

    public void RequestTransition(HierarchicalState from, HierarchicalState to)
    {
        Machine.ChangeState(from, to);
    }

    // Tìm cha chung gần nhất của hai state a và b
    public static HierarchicalState Lca(HierarchicalState a, HierarchicalState b)
    {
        // Đường đi từ a đến gốc
        var ap = new HashSet<HierarchicalState>();
        for (var s = a; s != null; s = s.Parent)
        {
            ap.Add(s);
        }

        // Đi từ b lên gốc, tìm state đầu tiên xuất hiện trong ap
        for (var s = b; s != null; s = s.Parent)
        {
            if (ap.Contains(s))
            {
                return s;
            }
        }

        return null;
    }
}
