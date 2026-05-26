using System.Collections.Generic;
using System.Reflection;

public class StateMachineBuilder
{
    readonly HierarchicalState root;

    public StateMachineBuilder(HierarchicalState root)
    {
        this.root = root;
    }

    void Wire(HierarchicalState state, HierarchicalStateMachine machine, HashSet<HierarchicalState> visited)
    {
        if (state == null) return;
        if (!visited.Add(state)) return;

        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        var machineField = typeof(HierarchicalState).GetField("Machine", flags);
        if (machineField != null) machineField.SetValue(state, machine);

        foreach (var field in state.GetType().GetFields(flags))
        {
            if (!typeof(HierarchicalState).IsAssignableFrom(field.FieldType)) continue;
            if (field.Name == "Parent") continue;

            var child = (HierarchicalState)field.GetValue(state);
            if (child == null) continue;
            if (ReferenceEquals(child.Parent, state)) continue;

            Wire(child, machine, visited);
        }
    }
}
