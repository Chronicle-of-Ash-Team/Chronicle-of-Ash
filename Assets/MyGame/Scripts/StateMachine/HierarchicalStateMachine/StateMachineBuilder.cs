using System.Collections.Generic;
using System.Reflection;

public class StateMachineBuilder
{
    readonly HierarchicalState root;

    public StateMachineBuilder(HierarchicalState root)
    {
        this.root = root;
    }

    public HierarchicalStateMachine Build()
    {
        var m = new HierarchicalStateMachine(root);
        Wire(root, m, new HashSet<HierarchicalState>());
        return m;
    }

    void Wire(
        HierarchicalState state,
        HierarchicalStateMachine machine,
        HashSet<HierarchicalState> visited)
    {
        if (state == null)
            return;

        // chống loop
        if (!visited.Add(state))
            return;

        var flags =
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic;

        // wire machine
        var machineField =
            typeof(HierarchicalState)
            .GetField("Machine", flags);

        if (machineField != null)
        {
            machineField.SetValue(state, machine);
        }

        // scan child states
        foreach (var field in state.GetType().GetFields(flags))
        {
            // chỉ lấy field là HierarchicalState
            if (!typeof(HierarchicalState)
                .IsAssignableFrom(field.FieldType))
            {
                continue;
            }

            // bỏ qua self references
            if (field.Name == nameof(HierarchicalState.Parent) ||
                field.Name == nameof(HierarchicalState.ActiveChild))
            {
                continue;
            }

            var child =
                field.GetValue(state) as HierarchicalState;

            if (child == null)
                continue;

            Wire(child, machine, visited);
        }
    }
}
